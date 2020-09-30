using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ejada.Business;
using Ejada.Data.UnitOfWork;
using Ejada.Shared;
using Ejada.Shared.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Ejada.API.Controllers
{
    [Route("Token")]
    [ApiController]
    public class TokenController : BaseController
    {
        readonly IUnitOfWork _unitOfWork;
        readonly ITokenManager _tokenManager;

        public TokenController(IUnitOfWork unitOfWork, ITokenManager tokenService)
        {
            this._unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this._tokenManager = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
        }

        [HttpPost]
        [Route("refresh")]
        [AllowAnonymous]
        public IActionResult Refresh(TokenApiModel tokenApiModel)
        {
            if (tokenApiModel is null)
            {
                return BadRequest("Invalid client request");
            }

            string accessToken = tokenApiModel.AccessToken;
            string refreshToken = tokenApiModel.RefreshToken;

            var principal = _tokenManager.GetPrincipalFromExpiredToken(accessToken);
            var Id = principal.Identity.Name; //this is mapped to the Name claim by default

            var user = _unitOfWork.UserRepository.FirstOrDefault(u => u.Id == Id);

            if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                return BadRequest("Invalid client request");
            }

            var newAccessToken = _tokenManager.GenerateAccessToken(principal.Claims);
            var newRefreshToken = _tokenManager.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            _unitOfWork.Save();

            return new ObjectResult(new
            {
                accessToken = newAccessToken,
                refreshToken = newRefreshToken
            });
        }

        [HttpPost, Authorize]
        [Route("revoke")]
        public IActionResult Revoke()
        {
            var username = User.Identity.Name;

            var user = _unitOfWork.UserRepository.FirstOrDefault(u => u.UserName == username);
            if (user == null) return BadRequest();

            user.RefreshToken = null;

            _unitOfWork.Save();

            return NoContent();
        }
    }
}
