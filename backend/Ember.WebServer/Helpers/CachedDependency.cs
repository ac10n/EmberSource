namespace Ember.WebServer.Helpers;

public static class CachedDependencyExtensions
{
    public static Lazy<T> Lazy<T>(this IServiceProvider serviceProvider) where T : notnull
    {
        return new(() => serviceProvider.GetRequiredService<T>());
    }

    public static Lazy<T?> LazyOptional<T>(this IServiceProvider serviceProvider)
    {
        return new(() => serviceProvider.GetService<T>());
    }
}
