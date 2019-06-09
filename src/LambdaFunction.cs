using Microsoft.AspNetCore.Hosting;
using Amazon.Lambda.AspNetCoreServer;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.Reflection;
using System;
using Microsoft.Extensions.Logging;

namespace HelloWorld
{
    public class LambdaFunction : APIGatewayProxyFunction
    {
        protected override void Init(IWebHostBuilder builder) =>
            Program.ConfigureWebHost(builder);

        // https://github.com/aws/aws-lambda-dotnet/blob/1a76fa07ce520a5cfc910adb42df81fa19a1ff32/Libraries/src/Amazon.Lambda.AspNetCoreServer/AbstractAspNetCoreFunction.cs
        // copied from source due to https://github.com/aspnet/AspNetCore/issues/9608
        protected override IWebHostBuilder CreateWebHostBuilder()
        {
            var builder = new WebHostBuilder()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    var env = hostingContext.HostingEnvironment;

                    config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                          .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);

                    if (env.IsDevelopment())
                    {
                        var appAssembly = Assembly.Load(new AssemblyName(env.ApplicationName));
                        if (appAssembly != null)
                        {
                            config.AddUserSecrets(appAssembly, optional: true);
                        }
                    }

                    config.AddEnvironmentVariables();
                })
                .ConfigureLogging((hostingContext, logging) =>
                {
                    logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));

                    if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("LAMBDA_TASK_ROOT")))
                    {
                        logging.AddConsole();
                        logging.AddDebug();
                    }
                    else
                    {
                        logging.AddLambdaLogger(hostingContext.Configuration, "Logging");
                    }
                })
                .UseDefaultServiceProvider((hostingContext, options) =>
                {
                    options.ValidateScopes = hostingContext.HostingEnvironment.IsDevelopment();
                })
                .UseLambdaServer();

            return builder;
        }
    }
}
