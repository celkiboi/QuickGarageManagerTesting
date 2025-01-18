using Microsoft.Playwright;
using NUnit.Framework;
using System.Threading.Tasks;

namespace QuickGarageManagerTesting.Tests;

public class BaseTest
{
    protected IPlaywright? playwright;
    protected IBrowser? browser;
    protected IBrowserContext context;
    protected IPage page;

    [SetUp]
    public async virtual Task SetUp()
    {
        playwright = await Playwright.CreateAsync();
        browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = true
        });
        context = await browser.NewContextAsync();
        page = await context.NewPageAsync();
    }

    [TearDown]
    public async Task TearDown()
    {
        if (browser != null) await browser.CloseAsync();
        playwright?.Dispose();
    }
}