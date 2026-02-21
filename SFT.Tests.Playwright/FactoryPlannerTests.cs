using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;
using SFT.Tests.Playwright.Infrastructure;

namespace SFT.Tests.Playwright;

/// <summary>
/// End-to-end Playwright tests for the Factory Planner page.
///
/// These tests validate that the interactive actions on /factory-planner work
/// correctly — i.e. that the page is rendered with @rendermode InteractiveServer
/// so button clicks are handled by the Blazor circuit rather than being silently
/// ignored in static-SSR mode.
/// </summary>
[TestFixture]
public class FactoryPlannerTests : PageTest
{
    private AppFixture _fixture = null!;
    private string _baseUrl = null!;

    [OneTimeSetUp]
    public async Task StartServer()
    {
        _fixture = new AppFixture();
        await _fixture.StartAsync();
        _baseUrl = _fixture.ServerAddress;
    }

    [OneTimeTearDown]
    public async Task StopServer()
    {
        await _fixture.DisposeAsync();
    }

    /// <summary>
    /// Navigates to the Factory Planner page and waits until the Blazor circuit is
    /// connected and the component is rendered interactively.  The "#blazor-interactive"
    /// span is injected by FactoryPlannerPage.razor only after OnAfterRender fires
    /// (which never happens during static SSR), so its presence confirms that event
    /// handlers are wired and button clicks will be handled.
    /// </summary>
    private async Task GotoFactoryPlannerAsync()
    {
        await Page.GotoAsync($"{_baseUrl}/factory-planner");
        // Wait for the interactive-render marker that FactoryPlannerPage adds via OnAfterRender.
        await Page.Locator("#blazor-interactive").WaitForAsync(
            new LocatorWaitForOptions { State = WaitForSelectorState.Attached, Timeout = 15_000 });
    }

    [Test]
    public async Task ClickAddMineButton_OpensAddMineDialog()
    {
        await GotoFactoryPlannerAsync();

        await Page.Locator("[data-test-id='add-mine-btn']").ClickAsync();

        // The Mine dialog contains an input with data-test-id="mine-dialog-name".
        await Expect(Page.Locator("[data-test-id='mine-dialog-name']"))
            .ToBeVisibleAsync(new LocatorAssertionsToBeVisibleOptions { Timeout = 5_000 });
    }

    [Test]
    public async Task ClickAddFactoryButton_OpensAddFactoryDialog()
    {
        await GotoFactoryPlannerAsync();

        await Page.Locator("[data-test-id='add-factory-btn']").ClickAsync();

        // The Factory dialog contains an input with data-test-id="factory-dialog-name".
        await Expect(Page.Locator("[data-test-id='factory-dialog-name']"))
            .ToBeVisibleAsync(new LocatorAssertionsToBeVisibleOptions { Timeout = 5_000 });
    }

    [Test]
    public async Task AddFactory_SubmitForm_FactoryAppearsInList()
    {
        await GotoFactoryPlannerAsync();

        // Open the Add Factory dialog.
        await Page.Locator("[data-test-id='add-factory-btn']").ClickAsync();
        await Page.Locator("[data-test-id='factory-dialog-name']").WaitForAsync(
            new LocatorWaitForOptions { State = WaitForSelectorState.Visible, Timeout = 5_000 });

        // Fill in the factory name.
        await Page.Locator("[data-test-id='factory-dialog-name']").FillAsync("Test Factory");

        // Submit the form.
        await Page.Locator("[data-test-id='factory-dialog-submit']").ClickAsync();

        // After the dialog closes, the new factory name should appear in the left panel.
        await Expect(Page.GetByText("Test Factory")).ToBeVisibleAsync(
            new LocatorAssertionsToBeVisibleOptions { Timeout = 5_000 });
    }

    [Test]
    public async Task MineDialog_ResourceDropdown_ListsRawResources()
    {
        await GotoFactoryPlannerAsync();

        // Open the Add Mine dialog.
        await Page.Locator("[data-test-id='add-mine-btn']").ClickAsync();
        await Page.Locator("[data-test-id='mine-dialog-name']").WaitForAsync(
            new LocatorWaitForOptions { State = WaitForSelectorState.Visible, Timeout = 5_000 });

        // Click the Resource autocomplete to open the dropdown.
        await Page.Locator("[data-test-id='mine-dialog-resource'] input").ClickAsync();

        // Verify that known raw resources appear in the dropdown.
        await Expect(Page.GetByText("Iron Ore", new() { Exact = true }))
            .ToBeVisibleAsync(new LocatorAssertionsToBeVisibleOptions { Timeout = 5_000 });
        await Expect(Page.GetByText("Copper Ore", new() { Exact = true }))
            .ToBeVisibleAsync(new LocatorAssertionsToBeVisibleOptions { Timeout = 5_000 });

        // Verify that search/filter works: type a partial name and check results.
        await Page.Locator("[data-test-id='mine-dialog-resource'] input").FillAsync("iron");
        await Expect(Page.GetByText("Iron Ore", new() { Exact = true }))
            .ToBeVisibleAsync(new LocatorAssertionsToBeVisibleOptions { Timeout = 5_000 });
        await Expect(Page.GetByText("Copper Ore", new() { Exact = true }))
            .ToBeHiddenAsync(new LocatorAssertionsToBeHiddenOptions { Timeout = 5_000 });
    }
}
