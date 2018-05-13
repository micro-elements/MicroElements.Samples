using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Swashbuckle.AuthWebApi
{
    //see: https://github.com/zarxor/Example.API.Secured
    //see: https://andrewlock.net/introduction-to-authorisation-in-asp-net-core/
    //see: https://stackoverflow.com/questions/42471866/how-to-create-roles-in-asp-net-core-and-assign-them-to-users
    public class BasicAuthMiddleware
    {
        private readonly RequestDelegate next;
        private readonly string realm;
        private BasicAuthService _authService;


        public BasicAuthMiddleware(RequestDelegate next, string realm)
        {
            this.next = next;
            this.realm = realm;
            _authService = new BasicAuthService();
        }

        public async Task Invoke(HttpContext context)
        {
            if (_authService.IsAuthorized(context))
            {
                await next.Invoke(context);
                return;
            }
        
            // Return authentication type (causes browser to show login dialog)
            context.Response.Headers["WWW-Authenticate"] = "Basic";

            // Add realm if it is not null
            if (!string.IsNullOrWhiteSpace(realm))
            {
                context.Response.Headers["WWW-Authenticate"] += $" realm=\"{realm}\"";
            }

            // Return unauthorized
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        }
    }
}