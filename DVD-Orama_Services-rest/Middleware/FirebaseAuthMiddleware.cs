using FirebaseAdmin.Auth;

namespace DVD_Orama_Services_rest.Middleware
{
    public class FirebaseAuthMiddleware
    {
        private readonly RequestDelegate _next;

        public FirebaseAuthMiddleware(RequestDelegate next) => _next = next;

        public async Task InvokeAsync(HttpContext context)
        {
            var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
            if (authHeader != null && authHeader.StartsWith("Bearer "))
            {
                var token = authHeader.Substring(7);
                try
                {
                    if (FirebaseAdmin.FirebaseApp.DefaultInstance != null)
                    {
                        var decoded = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(token);
                        context.Items["FirebaseUid"] = decoded.Uid;
                        if (decoded.Claims.TryGetValue("email", out var email))
                            context.Items["FirebaseEmail"] = email;
                    }
                }
                catch { }
            }
            await _next(context);
        }
    }
}
