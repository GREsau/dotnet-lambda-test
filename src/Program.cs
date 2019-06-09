using System;
using System.Threading.Tasks;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Amazon.Lambda.RuntimeSupport;
using Amazon.Lambda.Serialization.Json;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace HelloWorld
{
    public static class Program
    {
        public static Task Main(string[] args)
        {
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? EnvironmentName.Development;

            if (environmentName.Equals(EnvironmentName.Development, StringComparison.OrdinalIgnoreCase))
            {
                return RunLocalWebHost(args);
            }
            else
            {
                return RunLambdaWebHost();
            }
        }

        private static Task RunLocalWebHost(string[] args)
        {
            var builder = WebHost.CreateDefaultBuilder(args);
            return ConfigureWebHost(builder)
                .Build()
                .RunAsync();
        }

        private static async Task RunLambdaWebHost()
        {
            var lambda = new LambdaFunction();
            Func<APIGatewayProxyRequest, ILambdaContext, Task<APIGatewayProxyResponse>> handler = lambda.FunctionHandlerAsync;
            using (var handlerWrapper = HandlerWrapper.GetHandlerWrapper(handler, new JsonSerializer()))
            using (var bootstrap = new LambdaBootstrap(handlerWrapper))
            {
                await bootstrap.RunAsync();
            }
        }

        public static IWebHostBuilder ConfigureWebHost(IWebHostBuilder builder) =>
            builder.UseStartup<Startup>();
    }
}
