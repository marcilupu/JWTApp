using JWTManager;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ResourceServer.Utils
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        private readonly Uri _identityProviderUrl = new Uri("https://localhost:7186/User/Login");
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            string? token = context.HttpContext.Request.Cookies["naughty-shawty-access-token"];

            var jwtManager = context.HttpContext.RequestServices.GetRequiredService<IJwtManager>();

            if (token == null ||  !jwtManager.ValidateJwt(token))
            {
                QueryBuilder queryBuilder = new QueryBuilder();
                queryBuilder.Add("redirectUrl", context.HttpContext.Request.GetEncodedUrl());
                var redirectUrl = _identityProviderUrl.ToString().TrimEnd('/') + queryBuilder.ToString();
                context.Result = new RedirectResult(redirectUrl);
            }
        }
    }
}
