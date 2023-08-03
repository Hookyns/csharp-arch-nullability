using Microsoft.Extensions.Hosting;

namespace Arch.Nullability.Core.ApplicationLayers;

public class LayerRegistrator
{
	// private const string HostPropertyName = nameof(_registeredLayers);
	public static LayerRegistrator Instance { get; } = new();

	/// <summary>
	/// Collection of registered
	/// </summary>
	private readonly IList<ILayer> _registeredLayers = new List<ILayer>();

	public void Register<TLayer>(IHostBuilder host, TLayer layer)
		where TLayer : class, ILayer
	{
		// if (!host.Properties.ContainsKey(HostPropertyName))
		// {
		//     host.Properties[HostPropertyName] = _registeredLayers.AsReadOnly();
		// }

		_registeredLayers.Add(layer);
	}

	public IReadOnlyCollection<ILayer> GetRegisteredLayers()
	{
		return _registeredLayers.AsReadOnly();
	}
}
