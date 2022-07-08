using System.Security.Claims;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using ecommercestorewithaspcoremvc.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace ecommercestorewithaspcoremvc.Security
{
    public class SecurityManager
    {
        public async void SignIn(HttpContext httpContext, Account account) 
        {
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(getUserClaims(account), 
                CookieAuthenticationDefaults.AuthenticationScheme);
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            await httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, 
                claimsPrincipal);
        }

        public async void SignOut(HttpContext httpContext)
        {
            await httpContext.SignOutAsync();
        }
        private IEnumerable<Claim> getUserClaims(Account account)
        {
            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, account.Username));
            account.RoleAccounts.ToList().ForEach(ac => {
                claims.Add(new Claim(ClaimTypes.Role, ac.Role.Name));
            });
            return claims;
        }
    }
}