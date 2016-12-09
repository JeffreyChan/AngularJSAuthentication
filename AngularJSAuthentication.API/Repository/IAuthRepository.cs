using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using AngularJSAuthentication.API.Entities;
using AngularJSAuthentication.API.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace AngularJSAuthentication.API.Repository
{
    public interface IAuthRepository
    {
        Task<IdentityResult> RegisterUser(UserModel userModel);

        Task<ApplicationUser> FindUser(string userName, string password);

        Client FindClient(string clientId);

        Task<bool> AddRefreshToken(RefreshToken token);

        Task<bool> RemoveRefreshToken(string refreshTokenId);

        Task<bool>  RemoveRefreshToken(RefreshToken refreshToken);

        Task<RefreshToken> FindRefreshToken(string refreshTokenId);

        List<RefreshToken> GetAllRefreshTokens();

        Task<ApplicationUser> FindAsync(UserLoginInfo loginInfo);

        Task<IdentityResult> CreateAsync(ApplicationUser user);

        Task<IdentityResult> AddLoginAsync(string userId, UserLoginInfo login);
    }
}