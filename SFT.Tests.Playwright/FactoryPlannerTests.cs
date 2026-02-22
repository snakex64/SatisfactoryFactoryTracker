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
    public async Task AddMine_SubmitForm_MineAppearsInListWithResourceName()
    {
        await GotoFactoryPlannerAsync();

        // Open the Add Mine dialog.
        await Page.Locator("[data-test-id='add-mine-btn']").ClickAsync();
        await Page.Locator("[data-test-id='mine-dialog-name']").WaitForAsync(
            new LocatorWaitForOptions { State = WaitForSelectorState.Visible, Timeout = 5_000 });

        // Fill in the mine name.
        await Page.Locator("[data-test-id='mine-dialog-name']").FillAsync("Test Iron Mine");

        // Open the resource autocomplete and select Iron Ore.
        await Page.Locator("[data-test-id='mine-dialog-resource'] input").ClickAsync();
        await Page.GetByText("Iron Ore", new() { Exact = true }).First.ClickAsync();

        // Submit the form.
        await Page.Locator("[data-test-id='mine-dialog-submit']").ClickAsync();

        // After the dialog closes, the mine should appear in the left panel
        // with its name and the resource name (not "?").
        await Expect(Page.GetByText("Test Iron Mine")).ToBeVisibleAsync(
            new LocatorAssertionsToBeVisibleOptions { Timeout = 5_000 });
        await Expect(Page.GetByText("Iron Ore")).ToBeVisibleAsync(
            new LocatorAssertionsToBeVisibleOptions { Timeout = 5_000 });
    }

    [Test]
    public async Task EditMine_SubmitForm_MineTextUpdatesInList()
    {
        await GotoFactoryPlannerAsync();

        // First add a mine so we have something to edit.
        await Page.Locator("[data-test-id='add-mine-btn']").ClickAsync();
        await Page.Locator("[data-test-id='mine-dialog-name']").WaitForAsync(
            new LocatorWaitForOptions { State = WaitForSelectorState.Visible, Timeout = 5_000 });
        await Page.Locator("[data-test-id='mine-dialog-name']").FillAsync("Edit Mine Original");
        await Page.Locator("[data-test-id='mine-dialog-resource'] input").ClickAsync();
        await Page.GetByText("Iron Ore", new() { Exact = true }).First.ClickAsync();
        await Page.Locator("[data-test-id='mine-dialog-submit']").ClickAsync();
        await Expect(Page.GetByText("Edit Mine Original")).ToBeVisibleAsync(
            new LocatorAssertionsToBeVisibleOptions { Timeout = 5_000 });

        // Click the edit button for that mine.
        var mineRow = Page.Locator(".mud-treeview-item").Filter(new() { HasText = "Edit Mine Original" });
        await mineRow.Locator("[aria-label='Edit mine']").ClickAsync();
        await Page.Locator("[data-test-id='mine-dialog-name']").WaitForAsync(
            new LocatorWaitForOptions { State = WaitForSelectorState.Visible, Timeout = 5_000 });

        // Change the mine name and resource.
        await Page.Locator("[data-test-id='mine-dialog-name']").FillAsync("Edit Mine Updated");
        await Page.Locator("[data-test-id='mine-dialog-resource'] input").ClickAsync();
        await Page.Locator("[data-test-id='mine-dialog-resource'] input").FillAsync("copper");
        await Page.GetByText("Copper Ore", new() { Exact = true }).First.ClickAsync();
        await Page.Locator("[data-test-id='mine-dialog-submit']").ClickAsync();

        // The updated name and new resource should be visible, and the old values should be gone.
        await Expect(Page.GetByText("Edit Mine Updated")).ToBeVisibleAsync(
            new LocatorAssertionsToBeVisibleOptions { Timeout = 5_000 });
        await Expect(Page.GetByText("Copper Ore")).ToBeVisibleAsync(
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

    [Test]
    public async Task LevelMoving_MoveUpButton_ReordersLevels()
    {
        await GotoFactoryPlannerAsync();

        // Create a factory for this test.
        await Page.Locator("[data-test-id='add-factory-btn']").ClickAsync();
        await Page.Locator("[data-test-id='factory-dialog-name']").WaitForAsync(
            new LocatorWaitForOptions { State = WaitForSelectorState.Visible, Timeout = 5_000 });
        await Page.Locator("[data-test-id='factory-dialog-name']").FillAsync("Level Move Test Factory");
        await Page.Locator("[data-test-id='factory-dialog-submit']").ClickAsync();
        await Expect(Page.GetByText("Level Move Test Factory")).ToBeVisibleAsync(
            new LocatorAssertionsToBeVisibleOptions { Timeout = 5_000 });

        // Expand the factory tree item to reveal its children (including "Add level").
        // The toggle button lives inside `.mud-treeview-item-arrow`.
        var factoryItem = Page.Locator(".mud-treeview-item").Filter(
            new LocatorFilterOptions { Has = Page.Locator(".mud-treeview-item-content").Filter(new() { HasText = "Level Move Test Factory" }) }).First;
        await factoryItem.Locator(".mud-treeview-item-arrow button").ClickAsync();
        await Expect(factoryItem.GetByText("Add level")).ToBeVisibleAsync(
            new LocatorAssertionsToBeVisibleOptions { Timeout = 5_000 });

        // Add first level "Alpha".
        await factoryItem.GetByText("Add level").ClickAsync();
        await Page.Locator("[data-test-id='level-dialog-identifier']").WaitForAsync(
            new LocatorWaitForOptions { State = WaitForSelectorState.Visible, Timeout = 5_000 });
        await Page.Locator("[data-test-id='level-dialog-identifier']").FillAsync("Alpha");
        await Page.Locator("[data-test-id='level-dialog-submit']").ClickAsync();
        await Expect(Page.GetByText("Alpha")).ToBeVisibleAsync(
            new LocatorAssertionsToBeVisibleOptions { Timeout = 5_000 });

        // Add second level "Beta".
        await factoryItem.GetByText("Add level").ClickAsync();
        await Page.Locator("[data-test-id='level-dialog-identifier']").WaitForAsync(
            new LocatorWaitForOptions { State = WaitForSelectorState.Visible, Timeout = 5_000 });
        await Page.Locator("[data-test-id='level-dialog-identifier']").FillAsync("Beta");
        await Page.Locator("[data-test-id='level-dialog-submit']").ClickAsync();
        await Expect(Page.GetByText("Beta")).ToBeVisibleAsync(
            new LocatorAssertionsToBeVisibleOptions { Timeout = 5_000 });

        // Verify initial order: Alpha first (SortIndex 0), Beta second (SortIndex 1).
        var levelRows = Page.Locator("[data-test-id='level-row']");
        await Expect(levelRows.Nth(0)).ToHaveAttributeAsync("data-level-identifier", "Alpha",
            new LocatorAssertionsToHaveAttributeOptions { Timeout = 5_000 });
        await Expect(levelRows.Nth(1)).ToHaveAttributeAsync("data-level-identifier", "Beta",
            new LocatorAssertionsToHaveAttributeOptions { Timeout = 5_000 });

        // Alpha is at index 0 so its "Move level up" button must be disabled.
        await Expect(levelRows.Nth(0).Locator("[aria-label='Move level up']"))
            .ToBeDisabledAsync(new LocatorAssertionsToBeDisabledOptions { Timeout = 5_000 });
        // Beta is at index 1 (last) so its "Move level down" button must be disabled.
        await Expect(levelRows.Nth(1).Locator("[aria-label='Move level down']"))
            .ToBeDisabledAsync(new LocatorAssertionsToBeDisabledOptions { Timeout = 5_000 });

        // Click "Move level up" on Beta (currently at index 1).
        await levelRows.Nth(1).Locator("[aria-label='Move level up']").ClickAsync();

        // After the move, Beta (SortIndex 0) must be first and Alpha (SortIndex 1) second.
        await Expect(levelRows.Nth(0)).ToHaveAttributeAsync("data-level-identifier", "Beta",
            new LocatorAssertionsToHaveAttributeOptions { Timeout = 5_000 });
        await Expect(levelRows.Nth(1)).ToHaveAttributeAsync("data-level-identifier", "Alpha",
            new LocatorAssertionsToHaveAttributeOptions { Timeout = 5_000 });

        // Beta is now at index 0 → its "Move level up" button must be disabled.
        await Expect(levelRows.Nth(0).Locator("[aria-label='Move level up']"))
            .ToBeDisabledAsync(new LocatorAssertionsToBeDisabledOptions { Timeout = 5_000 });
    }
}
