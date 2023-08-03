using Arch.Nullability.Infrastructure;
using Arch.Nullability.Program.Infrastructure;
using Arch.Nullability.Program.Infrastructure.Configurators;

WebApplication
	.CreateBuilder(args)
	.RegisterLayer<InfrastructureLayer>()
	.BuildAndConfigure()
	.Run();
