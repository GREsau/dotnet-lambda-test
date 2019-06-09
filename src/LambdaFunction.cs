using Microsoft.AspNetCore.Hosting;
using Amazon.Lambda.AspNetCoreServer;

namespace HelloWorld
{
    public class LambdaFunction : APIGatewayProxyFunction
    {
        protected override void Init(IWebHostBuilder builder) =>
            Program.ConfigureWebHost(builder);
    }
}
