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
    [ApiController]
    [Route("Account")]
    public class AccountController : ControllerBase
    {
        private IUsersManager _usersManager;
        private readonly AppSettingsViewModel _appSettings;
        private readonly ExceptionManager _exceptionManager;
        private readonly IUnitOfWork _unitOfwork;

        public AccountController(IUnitOfWork unitOfWork,
            IUsersManager userService,
            IOptions<AppSettingsViewModel> appSettings)
        {
            _usersManager = userService;
            _appSettings = appSettings.Value;
            _unitOfwork = unitOfWork;

            _exceptionManager = new ExceptionManager(unitOfWork);
        }
        public ActionResult<Result> Index()
        {
            var result = new Result { Data=true};
            return result;
        }
        /// <summary>
        /// Login using email and password
        /// </summary>
        /// <param name="UserLoginViewModel"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("Login")]
        [HttpPost]
        public async Task<ActionResult<Result>> Login(UserLoginViewModel UserLoginViewModel)
        {
            try
            {
                var result = await _usersManager.Login(UserLoginViewModel.Email, UserLoginViewModel.Password);

                return result;
            }
            catch (Exception ex)
            {
                _exceptionManager.SaveLog(Request.Path, UserLoginViewModel, ex, null);
                return new Result()
                {
                    IsSuccess = false,
                    Errors = new List<string> { Resources.ExceptionMessage }
                };
            }

        }



        /// <summary>
        /// Register using email and password
        /// </summary>
        /// <param name="userViewModel"></param>
        /// <returns></returns>
        [Route("Register")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<Result>> Register(UserViewModel userViewModel)
        {
            try
            {
                var result = await _usersManager.CreateUser(userViewModel);

                return result;
            }
            catch (Exception ex)
            {
                _exceptionManager.SaveLog(Request.Path, userViewModel, ex, null);
                return new Result()
                {
                    IsSuccess = false,
                    Errors = new List<string> { Resources.ExceptionMessage }
                };
            }

        }
      

    }
}