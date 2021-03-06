using Env.Interfaces;
using Env.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Env
{
    public static class BuilderExtensions
    {
        public static IWebHostBuilder UseEnvironmentConfiguration<T>(this IWebHostBuilder hostBuilder) where T : class
        {
            return ConfigureService<T>(hostBuilder);
        }
        
        public static IWebHostBuilder UseEnvironmentConfiguration<T>(this IWebHostBuilder hostBuilder, string fileName, 
            ConfigurationTypeEnum configurationTypeEnum = ConfigurationTypeEnum.PreferEnvironment) where T : class
        {
            return ConfigureService<T>(hostBuilder, fileName, configurationTypeEnum);
        }
        
        public static IWebHostBuilder UseEnvironmentConfiguration<T>(this IWebHostBuilder hostBuilder, T type) where T : class
        {
            return hostBuilder.ConfigureServices(services =>
            {
                services.AddSingleton(typeof(T), type);
            });
        }

        private static IWebHostBuilder ConfigureService<T>(IWebHostBuilder hostBuilder, string fileName = null, 
            ConfigurationTypeEnum configurationType = ConfigurationTypeEnum.PreferEnvironment)
        {
            return hostBuilder.ConfigureServices(services =>
            {
                var configurationParser = new ConfigurationParser(configurationType, fileName);
                var instance = configurationParser.ParseConfiguration<T>();
                services.AddSingleton(typeof(T), instance);
            });
        }
    }
}