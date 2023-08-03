using Arch.Nullability.Program.Infrastructure.Configurators;
using Microsoft.AspNetCore.Diagnostics;

namespace Arch.Nullability.Program.Configurators;

public class ProblemDetailsConfigurator : IConfigurator
{
	public void Configure(WebApplicationBuilder builder)
	{
		builder.Services
			.AddProblemDetails(
				o => o.CustomizeProblemDetails = problemContext =>
				{
					problemContext.ProblemDetails.Instance = Guid.NewGuid().ToString();

					// Log the ProblemDetail
					Exception? error = problemContext.HttpContext.Features.Get<IExceptionHandlerPathFeature>()?.Error;

					if (error != null)
					{
						ILogger? logger = problemContext.HttpContext.RequestServices.GetService<ILogger>();

						logger?.LogError(
							error,
							"Instance: {instance}; {message}",
							problemContext.ProblemDetails.Instance,
							error.Message
						);
					}
				})
			;
	}

	public void Configure(WebApplication app)
	{
		app
			.UseExceptionHandler()
			.UseStatusCodePages()
			// .UseExceptionHandler(JsonExceptionHandler.Handler)
			;

		if (app.Environment.IsDevelopment())
		{
			app.UseDeveloperExceptionPage();
		}
	}
}
