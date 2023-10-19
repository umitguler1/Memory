namespace Memory.WebUI.Middleware
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        public LoggingMiddleware(RequestDelegate next)
        {
                _next = next;
        }
        public async Task InvokeAsync(HttpContext httpContext)
        {
            string content = $"İstek atilan metot : {httpContext.Request.Method}\nYol : {httpContext.Request.Path.Value}\nStatu Kod : {httpContext.Response.StatusCode}\n";
            try
            {
                File.AppendAllText("Logging.txt", content);
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                content += ex.Message;
                File.AppendAllText("Logging.txt", content);
            }
        }
    }
}
