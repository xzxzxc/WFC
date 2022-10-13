using System.Text.Json;
using AcceptanceTests.Common;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection.Extensions;
using WFC.Internals.Abstractions;

namespace WFC.Api.AcceptanceTests;

internal class WfcApiFactory : WebApplicationFactory<Program>
{
	public static string TestPossibleValuesName = nameof(TestPossibleValuesName);

	protected override IHost CreateHost(IHostBuilder builder)
	{
		builder.ConfigureHostConfiguration(
			configBuilder =>
			{
				var configuration = new
				{
					PossibleValues = new
					{
						TestPossibleValuesName = new
						{
							Type = "string",
							Values = TestPossibleValuesCollection.Unicode,
						}
					},
					Random = new
					{
						Seed = 69
					}
				};
				var jsonStream = new MemoryStream();
				JsonSerializer.Serialize(jsonStream, configuration);
				jsonStream.Position = 0;
				configBuilder.AddJsonStream(jsonStream);
			});

		return base.CreateHost(builder);
	}
}
