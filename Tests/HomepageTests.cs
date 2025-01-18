using Microsoft.Playwright;
using NUnit.Framework;
using System.Threading.Tasks;

namespace QuickGarageManagerTesting.Tests;

public class HomepageTests : BaseTest
{
    const string HomeUrl = "https://celkiboi.github.io/QGMBlog/";

    const string ExpectedTitle = "Homepage - QGM Blog";
    const int ExpectedNavLinkCount = 5;
    readonly string[] ExpectedLinks = ["Homepage", "News", "Leasing calculator", "App", "About"];
    const string ExpectedHeadingText = "Featured article:";
    const string ExpectedFooterText = "Quick garage manager blog. All rights reserved 2023.";
    const int ExpectedEditorsPickCount = 3;

    [SetUp]
    public async override Task SetUp()
    {
        await base.SetUp();
        await page.GotoAsync(HomeUrl, new PageGotoOptions { Timeout = 10000 });
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
        IElementHandle? logo = await page.QuerySelectorAsync("img[src='images/qgm_blog_logo.png']");
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
    public async Task FeaturedArticleSectionShouldBePresent()
    {
        IElementHandle? featuredArticle = await page.QuerySelectorAsync(".article_image_container");
        string message = "Featured article is missing";
        Assert.That(featuredArticle, Is.Not.Null, message);

        IElementHandle? heading = await featuredArticle.QuerySelectorAsync(".featured_pick h2");
        message = "Featured article heading is missing";
        Assert.That(heading, Is.Not.Null, message);

        string headingText = await heading.InnerTextAsync();
        message = $"Feature article heading text is incorrect. Got: {headingText}, expected: {ExpectedHeadingText}";
        Assert.That(headingText, Is.EqualTo(ExpectedHeadingText), message);
    }

    [Test]
    public async Task FooterShouldBePresent()
    {
        IElementHandle? footer = await page.QuerySelectorAsync(".footer");
        string message = "Footer is missing";
        Assert.That(footer, Is.Not.Null, message);

        IElementHandle? logoFooter = await page.QuerySelectorAsync("img[src='images/qgm_blog_logo.png']");
        message = "Image is missing";
        Assert.That(logoFooter, Is.Not.Null, message);

        string footerText = await footer.InnerTextAsync();
        message = $"Footer text is incorrect. Expected {ExpectedFooterText}, got {footerText}";
        Assert.That(footerText, Is.EqualTo(ExpectedFooterText), message);
    }

    [Test]
    public async Task EditorsPickArticlesShouldBePresent()
    {
        IElementHandle? editorsPickSection = await page.QuerySelectorAsync(".small_articles_container");
        string message = "Editors pick articles are missing";
        Assert.That(editorsPickSection, Is.Not.Null, message);

        IReadOnlyList<IElementHandle> editorsPickArticles = await page.QuerySelectorAllAsync(".small_articles_container div");
        IReadOnlyList<IElementHandle> articleDetails = await page.QuerySelectorAllAsync(".article_details");
        int editorsPickCount = editorsPickArticles.Count - articleDetails.Count;

        message = $"Editors pick count is incorrect. Expected: {ExpectedEditorsPickCount}, got: {editorsPickCount}";
        Assert.That(editorsPickCount, Is.EqualTo(ExpectedEditorsPickCount), message);
    }
}
