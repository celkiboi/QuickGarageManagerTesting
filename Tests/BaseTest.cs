using Microsoft.Playwright;
using NUnit.Framework;
using System.Threading.Tasks;

namespace QuickGarageManagerTesting.Tests;

public class BaseTest
{
    protected IPlaywright? playwright;
    protected IBrowser? browser;

    [SetUp]
    public async Task SetUp()
    {
        playwright = await Playwright.CreateAsync();
        browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = true
        });
    }

    [TearDown]
    public async Task TearDown()
    {
        if (browser != null) await browser.CloseAsync();
        playwright?.Dispose();
    }
}