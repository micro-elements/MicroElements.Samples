using System;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace Swashbuckle.AuthWebApi
{
    public class BasicAuthService
    {
        public (string UserName, string Password) GetAuth(HttpContext context)
        {
            string authHeader = context.Request.Headers["Authorization"];
            if (authHeader != null && authHeader.StartsWith("Basic "))
            {
                // Get the encoded username and password
                var encodedUsernamePassword = authHeader.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries)[1]?.Trim();

                // Decode from Base64 to string
                var decodedUsernamePassword = Encoding.UTF8.GetString(Convert.FromBase64String(encodedUsernamePassword));

                // Split username and password
                var username = decodedUsernamePassword.Split(':', 2)[0];
                var password = decodedUsernamePassword.Split(':', 2)[1];

                return (username, password);
            }

            return (null ,null);
        }

        public bool IsAuthorized(string username, string password)
        {
            if (username == null)
                return false;


            // Check that username and password are correct
            return username.Equals("admin", StringComparison.InvariantCultureIgnoreCase) && password.Equals("admin");
        }

        public bool IsAuthorized(HttpContext context)
        {
            var auth = GetAuth(context);
            var isAuthorized = IsAuthorized(auth.UserName, auth.Password);

            if (isAuthorized)
            {
                var claims = new Claim[] {new Claim("Role", "Administrator")};
                var principal = new ClaimsPrincipal(new ClaimsIdentity(claims));
                context.User = principal;
            }
            else
            {
                context.User = new ClaimsPrincipal(new ClaimsIdentity());
            }

            return isAuthorized;
        }
    }
}