using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Swashbuckle.AuthWebApi
{
    public class SwaggerAccessDocumentFilter : IDocumentFilter
    {
        private IHttpContextAccessor _httpContextAccessor;
        private BasicAuthService authService;

        public SwaggerAccessDocumentFilter(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            authService = new BasicAuthService();
        }

        public void Apply(SwaggerDocument swaggerDoc, DocumentFilterContext context)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            bool IsAuthenticated = authService.IsAuthorized(httpContext);
            if (!IsAuthenticated)
            {
                foreach (var pathItem in swaggerDoc.Paths.Values)
                {
                    pathItem.Get = null;
                    pathItem.Delete = null;
                    pathItem.Patch = null;
                    pathItem.Post = null;
                    pathItem.Put = null;
                }
            }
        }
    }
}