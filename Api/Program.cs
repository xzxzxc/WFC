using Api;
using Api.Extensions;
using Api.Models;
using Api.Models.Commands;
using Api.Models.Results;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using WFC;
using WFC.DI;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

var rootConfiguration = builder.Configuration.Get<RootConfiguration>();

builder.Host.ConfigureContainer<ContainerBuilder>(
	container =>
	{
		container.RegisterModule(new WfcModule(rootConfiguration.Random.Seed));
		container.RegisterModule<CanvasChangesHistoryModule>();
	});

foreach (var (name, values) in rootConfiguration.PossibleValues)
	builder.Services.AddPossibleValues(name, values.Values);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument(
	settings =>
	{
		settings.Title = "WFC";
		settings.Version = "v1";
		settings.DefaultReferenceTypeNullHandling = NJsonSchema.Generation.ReferenceTypeNullHandling.NotNull;
	});

await using var app = builder.Build();

app.UseOpenApi();
app.UseSwaggerUi3();

app.MapGet("/", static () => Results.Redirect("/swagger"));

app.MapGet(
	"config",
	static (IConfiguration configuration) =>
		((IConfigurationRoot)configuration).GetDebugView());

app.MapGet(
	pattern: "/possibleValues/{type}",
	(string type) =>
		rootConfiguration.PossibleValues
			.Where(kv => kv.Value.Type == type)
			.Select(
				kv => new PossibleValuesDto(
					Name: kv.Key,
					kv.Value.Values)));

app.MapPost(
	pattern: "/api/collapse/",
	handler: static (
		CollapseCommand command,
		IWfcAlgorithm algorithm) =>
	{
		var canvas = command.ToCanvas();

		algorithm.Collapse(canvas);

		return new CollapseResult(
			canvas.Width,
			canvas.Height,
			Values: canvas.Select(c => c.Value!).ToArray());
	});

app.MapPost(
	pattern: "/api/collapse/history",
	handler: static (
		CollapseCommand command,
		IWfcAlgorithm algorithm,
		IHistoryHolder historyHolder,
		ILogger<Program> logger, 
		[FromQuery(Name = "debug")] bool? isDebug) =>
	{
		var canvas = command.ToCanvas();
		
		try
		{
			algorithm.Collapse(canvas);
		}
		catch (Exception ex)
		{
			if (isDebug is null or false)
				throw;
			
			logger.LogError(ex, message: "Error occured during history call");
		}
		
		return new CollapseWithHistoryResult(
			canvas.Width,
			canvas.Height,
			ValuesHistory: historyHolder.History
				.Select(v => v.ToOneDimensionalArray())
				.ToArray());
	});

await app.RunAsync();
