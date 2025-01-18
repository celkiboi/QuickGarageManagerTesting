using Microsoft.Playwright;
using NUnit.Framework;
using System.Threading.Tasks;

namespace QuickGarageManagerTesting.Tests.ArticleTests;

public class RimacArticleTests : ArticleTests
{
    const string ArticleUrl = "https://celkiboi.github.io/QGMBlog/articles/Article1/article.html";
    const string ExpectedTitle = "Rimac - QGM Blog";
    const string ExpectedArticleTitle = "Rimac Nevera tested - the fastest car in the world?";
    const int ExpectedAsideArticleCount = 3;
    readonly string[] ExpectedAsideTitles = [
        "Can a hybrid Bugatti live up to the name?",
        "McLaren Speedtail review - a high tech, 3 seater, bullet",
        "Pagani Huayra - was it a failure?"
    ];

    [SetUp]
    public async override Task SetUp()
    {
        await base.SetUp();
        await page.GotoAsync(ArticleUrl, new PageGotoOptions { Timeout = 10000 });
    }

    [Test]
    public async Task PageTitleShouldBeCorrect()
    {
        string title = await page.TitleAsync();
        string message = $"The page title is incorrect. Expected {ExpectedTitle}, got: {title}";
        Assert.That(title, Is.EqualTo(ExpectedTitle), message);
    }

    [Test]
    public async Task ArticleTitleShouldBeCorrect()
    {
        string articleTitle = await page.InnerTextAsync("#article_title_container h1");
        string message = $"Article title is incorrect. Expected {ExpectedArticleTitle}, got: {articleTitle}";
        Assert.That(articleTitle, Is.EqualTo(ExpectedArticleTitle), message);
    }

    [Test]
    public async Task AsideArticlesShouldBeDisplayedCorrectly()
    {
        IReadOnlyList<IElementHandle> asideArticles = await page.QuerySelectorAllAsync(".aside_article");
        string message = $"Aside articles count is incorrect. Expected {ExpectedAsideArticleCount}, got {asideArticles.Count}";
        Assert.That(asideArticles, Has.Count.EqualTo(ExpectedAsideArticleCount), message);

        for (int i = 0; i < asideArticles.Count; i++)
        {
            string asideTitle = await asideArticles[i].InnerTextAsync();
            message = $"Aside article title is incorrect. Expected {ExpectedAsideTitles[i]}, got: {asideTitle}";
            Assert.That(asideTitle, Does.Contain(ExpectedAsideTitles[i]), message);
        }
    }
}
