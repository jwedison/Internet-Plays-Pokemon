using System.Net.Http;
using internetplays.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace internetplays.Services
{
    public static class ServiceCollectionExtensions
    {
        public static void AddCommonServices(this IServiceCollection collection)
        {
            collection.AddSingleton<IChatLogger, ChatLoggerService>();
            collection.AddSingleton<HttpClient>();
            collection.AddSingleton<GBAClient>();
            collection.AddTransient<MainViewModel>();
        }
    }
}
