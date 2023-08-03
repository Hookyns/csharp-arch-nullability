using Arch.Nullability.Core.ApplicationLayers;

namespace Arch.Nullability.Program.Infrastructure.Configurators;

public static class ConfiguratorsExtension
{
    public static WebApplication BuildAndConfigure(this WebApplicationBuilder builder)
    {
        IReadOnlyCollection<ILayer> layers = LayerRegistrator.Instance.GetRegisteredLayers();
        List<IConfigurator> configurators = typeof(ConfiguratorsExtension).Assembly.GetTypes()
            .Where(t => t.IsAssignableTo(typeof(IConfigurator)) && t != typeof(IConfigurator))
            .Select(t => Activator.CreateInstance(t) as IConfigurator ?? throw new InvalidOperationException())
            .OrderBy(c => c.Order)
            .ToList();

        // Execute Configure() methods with builder.

        foreach (IConfigurator configurator in configurators)
        {
            configurator.Configure(builder);
        }

        foreach (ILayer layer in layers)
        {
            layer.Configure(builder.Host);
        }

        // Build application
        WebApplication app = builder.Build();

        // Execute Configure() methods with host.

        foreach (IConfigurator configurator in configurators)
        {
            configurator.Configure(app);
        }

        foreach (ILayer layer in layers)
        {
            layer.Configure(app);
        }

        return app;
    }
}