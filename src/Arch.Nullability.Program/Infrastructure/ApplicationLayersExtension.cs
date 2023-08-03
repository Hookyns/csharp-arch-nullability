using Arch.Nullability.Core.ApplicationLayers;

namespace Arch.Nullability.Program.Infrastructure;

public static class ApplicationLayersExtension
{
	public static WebApplicationBuilder RegisterLayer<TLayer>(this WebApplicationBuilder builder)
		where TLayer : class, ILayer, new()
	{
		LayerRegistrator.Instance.Register(builder.Host, new TLayer());
		return builder;
	}
}
