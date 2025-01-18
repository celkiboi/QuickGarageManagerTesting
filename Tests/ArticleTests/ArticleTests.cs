using Microsoft.Playwright;
using NUnit.Framework;
using System.Threading.Tasks;

namespace QuickGarageManagerTesting.Tests.ArticleTests;

public abstract class ArticleTests : BaseTest
{
    const int ExpectedNavLinkCount = 5;
    readonly string[] ExpectedLinks = ["Homepage", "News", "Leasing calculator", "App", "About"];
    const string ExpectedAuthorName = "By: Tomislav Celić";

    [SetUp]
    public async override Task SetUp()
    {
        await base.SetUp();
    }

    [Test]
    public async Task NavigationLinksShouldBeCorrect()
    {
        IReadOnlyList<IElementHandle> navLinks = await page.QuerySelectorAllAsync(".navigation ul li a");
        string message = $"Nav links count is incorrect. Expected {ExpectedNavLinkCount}, got {navLinks.Count}";
        Assert.That(navLinks, Has.Count.EqualTo(ExpectedNavLinkCount), message);

        for (int i = 0; i < navLinks.Count; i++)
        {
            string navText = await navLinks[i].InnerTextAsync();
            message = $"Nav link text is incorrect. Expected {ExpectedLinks[i]}, got: {navText}";
            Assert.That(navText, Is.EqualTo(ExpectedLinks[i]), message);
        }
    }

    [Test]
    public async Task AuthorNameShouldBeDisplayed()
    {
        string authorName = await page.InnerTextAsync(".by_author_container h3");
        string message = $"Author name is incorrect. Expected {ExpectedAuthorName}, got: {authorName}";
        Assert.That(authorName, Is.EqualTo(ExpectedAuthorName), message);
    }

    [Test]
    public async Task MainArticleImageShouldBeClickable()
    {
        IElementHandle? articleImage = await page.QuerySelectorAsync("#article_image");
        string message = "Main article image is missing or not clickable.";
        Assert.That(articleImage, Is.Not.Null, message);

        await articleImage.ClickAsync();
    }

    [Test]
    public async Task FooterLogoShouldBeDisplayed()
    {
        IElementHandle? footerLogo = await page.QuerySelectorAsync("img[src='../../images/qgm_blog_logo.png']");
        string message = "Footer logo is not displayed correctly.";
        Assert.That(footerLogo, Is.Not.Null, message);
    }

    [Test]
    public async Task LeasingCalculateButtonShouldBeClickable()
    {
        IElementHandle? calculateButton = await page.QuerySelectorAsync("#leasing_calculte_button");
        string message = "Leasing calculate button is missing or not clickable.";
        Assert.That(calculateButton, Is.Not.Null, message);

        await calculateButton.ClickAsync();
    }

    [Test]
    public async Task StatsTableShouldBeDisplayedCorrectly()
    {
        IReadOnlyList<IElementHandle> tableRows = await page.QuerySelectorAllAsync(".stat_table tbody tr");
        string message = $"Stats table rows are missing or incorrect.";
        Assert.That(tableRows, Has.Count.GreaterThan(0), message);

        for (int i = 0; i < tableRows.Count; i++)
        {
            string rowText = await tableRows[i].InnerTextAsync();
            Assert.That(rowText, Is.Not.Empty, $"Row {i + 1} in the stats table is empty or not displayed correctly.");
        }
    }
}
