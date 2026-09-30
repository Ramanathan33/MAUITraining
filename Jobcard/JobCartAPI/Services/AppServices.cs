using Microsoft.Extensions.DependencyInjection;

namespace JobCartAPI.Services
{
    public static class AppServices
    {
        public static IServiceProvider Current =>
            IPlatformApplication.Current?.Services
            ?? throw new InvalidOperationException("Application services are not available.");

        public static T GetRequired<T>() where T : class =>
            Current.GetRequiredService<T>();
    }
}
