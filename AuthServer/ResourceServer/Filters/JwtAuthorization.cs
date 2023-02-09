using JWTManager;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ResourceServer.Filters
{
    /// <summary>
    /// This attribute is responsible with validate the jwt token. Check if the token exists in the cookies and it is not, get is from the AuthServer.
    /// </summary>
    public class JwtAuthorization : Attribute, IAsyncAuthorizationFilter
    {
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {           
            var httpContext = context.HttpContext;
            // Get the jwt from the cookie (if exists).
            var token = httpContext.Request.Cookies["Jwt"];

            // Check if the kwt is in the cookie
            // If it is not, make a request to the authServer to  get the jwt token.
            if (string.IsNullOrEmpty(token))
            {
                string authQueryCode = httpContext.Request.Query["authorizationCode"];

                // If authorization code is null, redirect the user to the login page.
                if (authQueryCode == null)
                {
                    Redirect(context, "/home/index");
                    return;
                }

                // Get the response of the httpContext.
                HttpClient client = new HttpClient();
                var response = await client.GetAsync("https://localhost:7186/user/gettoken?authorizationCode=" + authQueryCode);

                // Get the cookie from the authorization server
                token = response.Headers.GetValues("Authorization").ToList()[1];

                // Set the jwt in the cookie.
                httpContext.Response.Cookies.Append("Jwt", token);
            }

            // Get the IJwtManager service.
            var jwtManager = httpContext.RequestServices.GetRequiredService<IJwtManager>();

            if (string.IsNullOrEmpty(token) || !jwtManager.ValidateJwt(token))
            {
                Redirect(context, "/home/index");
                return;
            }

            // Set the httpContext dictionary to contain the token to use it in controllers.
            httpContext.Items["jwt"] = token;
        }

        private void Redirect(AuthorizationFilterContext httpContext, string redirectUrl)
        {
            httpContext.Result = new RedirectResult(redirectUrl);
        }
    }
}
