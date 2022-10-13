using Api.Models.Commands;
using Api.Models.Results;
using FluentAssertions;
using Xunit;

namespace WFC.Api.AcceptanceTests;

public class CollapseTests
{
	private readonly HttpClient _client;

	private readonly CollapseCommand _simpleCollapseCommand = new(
		WfcApiFactory.TestPossibleValuesName,
		Width: 3,
		Height: 1,
		Elements: new[]
		{
			new CollapseCommand.Element(X: 0, Y: 0, Value: "┰"),
		});

	public CollapseTests()
	{
		_client = new WfcApiFactory().CreateClient();
	}

	[Fact]
	public async Task ShouldCollapseSimple()
	{
		var response = await PostSimpleCommand($"/api/collapse/");

		var result = await response.Content.ReadFromJsonAsync<CollapseResult>();

		result.Should().BeEquivalentTo(
			new CollapseResult(
				_simpleCollapseCommand.Width,
				_simpleCollapseCommand.Height,
				Values:
				new[]
				{
					"┰", "┨", "┠",
				}));
	}

	[Fact]
	public async Task ShouldCollapseSimpleWithHistory()
	{
		var response = await PostSimpleCommand($"/api/collapse/history");

		var result = await response.Content.ReadFromJsonAsync<CollapseWithHistoryResult>();

		result.Should().BeEquivalentTo(
			new CollapseWithHistoryResult(
				_simpleCollapseCommand.Width,
				_simpleCollapseCommand.Height,
				ValuesHistory:
				new[]
				{
					new[]
					{
						"┰", "┨", null
					},
					new[]
					{
						"┰", "┨", "┠"
					}
				}));
	}

	private async Task<HttpResponseMessage> PostSimpleCommand(string? path)
	{
		var response = await _client.PostAsJsonAsync(
			path,
			_simpleCollapseCommand);
		response.EnsureSuccessStatusCode();
		return response;
	}
}
