using Arch.Nullability.Application.Orders.QueryOrders;
using Arch.Nullability.Program.Infrastructure.Configurators;
using Asp.Versioning;
using Wolverine;

namespace Arch.Nullability.Program.Configurators;

public class ApiConfigurator : IConfigurator
{
	public int Order => int.MinValue;

	public void Configure(WebApplicationBuilder builder)
	{
		builder.Services
			.AddCors()
			.AddHttpContextAccessor()
			.AddEndpointsApiExplorer()
			.AddApiVersioning(
				o =>
				{
					o.AssumeDefaultVersionWhenUnspecified = false;
					o.DefaultApiVersion = ApiVersion.Neutral;
					// o.DefaultApiVersion = new ApiVersion(1, 0); // I do not recommend this in case your API is exposed to 3rd parties.
					o.ReportApiVersions = true;
					o.ApiVersionReader = ApiVersionReader.Combine(
						new QueryStringApiVersionReader("api-version"),
						new HeaderApiVersionReader("x-version"),
						new MediaTypeApiVersionReader("ver"));
				})
			.AddApiExplorer(
				setup =>
				{
					setup.GroupNameFormat = "'v'VVV";
					setup.SubstituteApiVersionInUrl = true;
				})
			// .EnableApiVersionBinding()
			;

		// In case you use MVC this may be handy.
		// builder.Services
		//     .AddMvc(options =>
		//     {
		//         // Delete predefined validation providers
		//         options.ModelValidatorProviders.Clear();
		//         options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
		//     })
		//     .AddJsonOptions(options => { options.JsonSerializerOptions.PropertyNamingPolicy = null; })
		//     ;
	}

	public void Configure(WebApplication app)
	{
		// var version1 = new ApiVersion(1.0);
		// var version2 = new ApiVersion(2.0);
		//
		// var versionSet = app.NewApiVersionSet()
		//     .HasApiVersion(version1)
		//     .HasApiVersion(version2)
		//     .Build();


		var orders = app.NewVersionedApi("Orders");
		var ordersLatest = orders.MapGroup("orders").HasApiVersion(2.0);
		var ordersV1 = orders.MapGroup("orders").HasApiVersion(1.0);

		app.MapGet("/", () => Results.Ok());

		ordersLatest.MapGet(
				"/",
				([AsParameters] OrdersQuery query, IMessageBus bus) => bus.InvokeAsync<OrderResult[]>(query))
			.WithName("Orders");

		ordersV1.MapGet(
				"/{id:int}",
				([AsParameters] OrderQuery query, IMessageBus bus) => bus.InvokeAsync<OrderResult>(query))
			.WithDescription("Some Method Description")
			.MapToApiVersion(1.0)
			.Produces(204)
			.ProducesProblem(400)
			.ProducesProblem(404);

		ordersLatest.MapGet(
				"/{id:int}",
				([AsParameters] OrderQuery query, IMessageBus bus) => bus.InvokeAsync<OrderResult>(query))
			.WithDescription("Some Method Description")
			.MapToApiVersion(2.0)
			.Produces(204)
			.ProducesProblem(400)
			.ProducesProblem(404);


		app
			.UseCors(
				policyBuilder => policyBuilder
					// TODO: Only for development!
					.SetIsOriginAllowed(_ => true)
					.AllowCredentials()
					.AllowAnyMethod()
					.AllowAnyHeader()
			)
			// .UseHttpsRedirection()
			;
	}
}
