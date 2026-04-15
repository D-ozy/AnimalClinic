using Grpc.Core;
using Grpc.Core.Interceptors;

namespace AnimalClinic.Grpc
{
    public class GrpcLoggingInterceptor : Interceptor
    {
        private readonly ILogger<GrpcLoggingInterceptor> _logger;

        public GrpcLoggingInterceptor(ILogger<GrpcLoggingInterceptor> logger)
        {
            _logger = logger;
        }

        public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
            TRequest request,
            ServerCallContext context,
            UnaryServerMethod<TRequest, TResponse> continuation)
        {
            _logger.LogInformation("Request: {Method} | {@Request}\n", context.Method, request);

            var start = DateTime.UtcNow;

            try
            {
                var response = await continuation(request, context);

                var time = (DateTime.UtcNow - start).TotalMilliseconds;

                _logger.LogInformation("Response: {Method} | {Time} ms | {@Response}\n",
                    context.Method, time, response);

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in {Method}\n\n", context.Method);
                throw;
            }
        }
    }
}