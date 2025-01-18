using Microsoft.Playwright;
using NUnit.Framework;
using System.Threading.Tasks;

namespace QuickGarageManagerTesting.Tests;

public class NewsPageTests : BaseTest
{
    const string NewsUrl = "https://celkiboi.github.io/QGMBlog/pages/news.html";

    const string ExpectedTitle = "News - QGM Blog";
    const int ExpectedNavLinkCount = 5;
    readonly string[] ExpectedLinks = ["Homepage", "News", "Leasing calculator", "App", "About"];
    const string ExpectedHeadingText = "Editors pick:";
    const string ExpectedFooterText = "Quick garage manager blog. All rights reserved 2023.";
    const int ExpectedMoreArticlesCount = 4;

    [SetUp]
    public async override Task SetUp()
    {
        await base.SetUp();
        await page.GotoAsync(NewsUrl, new PageGotoOptions { Timeout = 10000 });
    }

    [Test]
    public async Task PageTitleShouldBeCorrect()
    {
        string title = await page.TitleAsync();
        string message = $"The page title is incorrect. Expected {ExpectedTitle}, got: {title}";
        Assert.That(title, Is.EqualTo(ExpectedTitle), message);
    }

    [Test]
    public async Task LogoImageShouldBePresent()
    {
        IElementHandle? logo = await page.QuerySelectorAsync("img[src='../images/qgm_blog_logo.png']");
        string message = "Logo image is not present";
        Assert.That(logo, Is.Not.Null, message);
    }

    [Test]
    public async Task NavigationLinksShouldBePresent()
    {
        IReadOnlyList<IElementHandle> navLinks = await page.QuerySelectorAllAsync(".navigation ul li a");
        string message = $"Nav links count is incorrect. Expected {ExpectedNavLinkCount} got {navLinks.Count}";
        Assert.That(navLinks, Has.Count.EqualTo(ExpectedNavLinkCount), message);

        for (int i = 0; i < navLinks.Count; i++)
        {
            IElementHandle navLink = navLinks[i];
            string navText = await navLink.InnerTextAsync();
            message = $"Nav link text is incorrect. Expected {ExpectedLinks[i]}, got: {navText}";
            Assert.That(ExpectedLinks[i], Is.EqualTo(navText), message);
        }
    }

    [Test]
    public async Task EditorsPickSectionShouldBePresent()
    {
        IElementHandle? editorsPickSection = await page.QuerySelectorAsync("#editors_pick_grid");
        string message = "Editors pick section is missing";
        Assert.That(editorsPickSection, Is.Not.Null, message);

        IElementHandle? heading = await editorsPickSection.QuerySelectorAsync("h2");
        message = "Editors pick heading is missing";
        Assert.That(heading, Is.Not.Null, message);

        string headingText = await heading.InnerTextAsync();
        message = $"Editors pick heading text is incorrect. Got: {headingText}, expected: {ExpectedHeadingText}";
        Assert.That(headingText, Is.EqualTo(ExpectedHeadingText), message);
    }

    [Test]
    public async Task ArticlesInGridShouldBePresent()
    {
        IReadOnlyList<IElementHandle> gridItems = await page.QuerySelectorAllAsync(".article_grid .grid_item");
        string message = $"Articles in the grid count is incorrect. Expected 4, got: {gridItems.Count}";
        Assert.That(gridItems, Has.Count.EqualTo(4), message);
    }

    [Test]
    public async Task MoreArticlesSectionShouldBePresent()
    {
        IElementHandle? moreArticlesSection = await page.QuerySelectorAsync(".more_articles_container");
        string message = "More articles section is missing";
        Assert.That(moreArticlesSection, Is.Not.Null, message);

        IReadOnlyList<IElementHandle> smallArticles = await page.QuerySelectorAllAsync(".small_articles_container div");
        IReadOnlyList<IElementHandle> articleDetails = await page.QuerySelectorAllAsync(".article_details");
        int smallArticlesCount = smallArticles.Count - articleDetails.Count;

        message = $"More articles count is incorrect. Expected {ExpectedMoreArticlesCount}, got: {smallArticlesCount}";
        Assert.That(smallArticlesCount, Is.EqualTo(ExpectedMoreArticlesCount), message);
    }

    [Test]
    public async Task FooterShouldBePresent()
    {
        IElementHandle? footer = await page.QuerySelectorAsync(".footer");
        string message = "Footer is missing";
        Assert.That(footer, Is.Not.Null, message);

        IElementHandle? logoFooter = await page.QuerySelectorAsync("img[src='../images/qgm_blog_logo.png']");
        message = "Footer logo is missing";
        Assert.That(logoFooter, Is.Not.Null, message);

        string footerText = await footer.InnerTextAsync();
        message = $"Footer text is incorrect. Expected {ExpectedFooterText}, got {footerText}";
        Assert.That(footerText, Is.EqualTo(ExpectedFooterText), message);
    }
}

