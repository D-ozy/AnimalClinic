namespace AnimalClinic
{
    public class LoggerMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger logger;

        public LoggerMiddleware(RequestDelegate next, ILogger<LoggerMiddleware> logger)
        {
            this.next = next;
            this.logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                HttpResponse response = context.Response;
                HttpRequest request = context.Request;

                await next.Invoke(context);

                if(response.StatusCode != 200)
                    logger.LogError($"{DateTime.Now.ToLongTimeString()}\nStatus code: {response.StatusCode}\nMethod: {request.Method}\nPath: {request.Path}\n");
                else
                    logger.LogInformation($"{DateTime.Now.ToLongTimeString()}\nStatus code: {response.StatusCode}\nMethod: {request.Method}\nPath: {request.Path}\n\n");
            }
            catch (Exception ex)
            {
                HttpResponse response = context.Response;
                HttpRequest request = context.Request;
                logger.LogError($"{DateTime.Now.ToLongTimeString()}\nStatus code: {response.StatusCode}\nMethod: {request.Method}\nPath: {request.Path}\nException message: {ex.Message}");
            }
        }
    }
}
