using Ejada.Data;
using Ejada.Data.UnitOfWork;
using Ejada.Entities.IdentityModels;
using Ejada.Shared;
using Ejada.Shared.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Ejada.Business
{
    public interface IUsersManager
    {
        public Task<Result> Login(string username, string password);

        public User GetByUserId(string userId);

        public Task<Result> CreateUser(UserViewModel UserViewModel);


    }
    public class UsersManager : IUsersManager
    {
        private EjadaContext _context;
        private readonly AppSettingsViewModel _appSettings;
        private readonly ITokenManager _tokenManager;
        private IUnitOfWork _unitOfWork;
        UserManager<User> _userManager;

        public UsersManager(EjadaContext context, IOptions<AppSettingsViewModel> appSettings, UserManager<User> userManager, IUnitOfWork unitOfWork , ITokenManager tokenManager)
        {
            _unitOfWork = unitOfWork;
            _context = context;
            _appSettings = appSettings.Value;
            _userManager = userManager;
            _tokenManager = tokenManager;
        }


        public async Task<Result> Login(string username, string password)
        {

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return new Result()
                {
                    IsSuccess = false,
                    Errors = new List<string> { Resources.EmailAndPasswordRequired }
                };

            // get the user to verifty
            var userToVerify = await _userManager.FindByNameAsync(username);

            if (userToVerify == null || !userToVerify.IsActive)
                return new Result()
                {
                    IsSuccess = false,
                    Errors = new List<string> { Resources.EmailOrPasswordInCorrect }
                };

            // check the credentials
            if (await _userManager.CheckPasswordAsync(userToVerify, password))
            {

                #region Get User Roles

                List<RoleViewModel> RoleVmList = new List<RoleViewModel>();
                var RolesIds = _context.UserRoles.Where(ur => ur.UserId == userToVerify.Id && ur.IsActive).Select(u => u.RoleId).ToList();

                foreach (var RoleId in RolesIds)
                {
                    var roleObj = _context.Roles.Where(r => r.Id == RoleId).FirstOrDefault();
                    RoleViewModel RoleVM = new RoleViewModel()
                    {
                        RoleId = roleObj.Id,
                        RoleEnglishName = roleObj.Name,
                        RoleArabicName = roleObj.NormalizedName,
                        IsActive = roleObj.IsActive
                    };
                    RoleVmList.Add(RoleVM);
                }

                #endregion

                

                var claims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub, userToVerify.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.Name, userToVerify.Id)
                };
                claims.AddRange(RoleVmList.Select(role => new Claim(ClaimsIdentity.DefaultRoleClaimType, role.RoleEnglishName)));


                var accessToken = _tokenManager.GenerateAccessToken(claims );
                var refreshToken = _tokenManager.GenerateRefreshToken();
                //save the refresh token and the Refresh Token Expiry Time on user
                userToVerify.RefreshToken = refreshToken;
                userToVerify.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_appSettings.RefreshTokenExpirationDays);
                _context.SaveChanges();

                UserLoggedInViewModel userLoggedInViewModel = new UserLoggedInViewModel();
                userLoggedInViewModel.Email = userToVerify.Email;
                userLoggedInViewModel.FullName = userToVerify.FullName;
                userLoggedInViewModel.Id = userToVerify.Id;
                userLoggedInViewModel.RoleViewModel = RoleVmList;
                userLoggedInViewModel.Token = accessToken;
                userLoggedInViewModel.RefreshToken = refreshToken;   
                return new Result()
                {
                    IsSuccess = true,
                    Data = userLoggedInViewModel
                };
            }

            else
            {
                return new Result()
                {
                    IsSuccess = false,
                    Errors = new List<string> { Resources.EmailOrPasswordInCorrect }
                };
            }

        }

        public User GetByUserId(string userId)
        {
            return _context.Users.FirstOrDefault(u => u.Id == userId);
        }

        public async Task<Result> CreateUser(UserViewModel UserViewModel)
        {
            // validation
            if (string.IsNullOrWhiteSpace(UserViewModel.Password) || string.IsNullOrWhiteSpace(UserViewModel.Email))
                return new Result()
                {
                    IsSuccess = false,
                    Errors = new List<string> { Resources.EmailAndPasswordRequired }
                };

            if (_context.Users.Any(x => x.UserName == UserViewModel.Email))
                return new Result()
                {
                    IsSuccess = false,
                    Errors = new List<string> { Resources.EmailIsAlreadyTake }
                };

            User appUser = new User();
            appUser.UserName = UserViewModel.Email;
            appUser.Email = UserViewModel.Email;
            appUser.FullName = UserViewModel.FullName;
            appUser.IsActive = true;
            appUser.CreationDate = DateTime.Now;

            var result = await _userManager.CreateAsync(appUser, UserViewModel.Password);

            if (result.Succeeded)
            {
                return new Result()
                {
                    Data = appUser,
                    IsSuccess = true,
                    Errors = new List<string> { }
                };
            }
            else
            {
                return new Result()
                {

                    IsSuccess = false,
                    Errors = new List<string> { Resources.PasswordShouldBeMoreThan6Digit }
                };

            }


        }


    }
 
}
