using Microsoft.Playwright;
using NUnit.Framework;
using System.Threading.Tasks;

namespace QuickGarageManagerTesting.Tests.ArticleTests;

public class BugattiArticleTests : ArticleTests
{
    const string ArticleUrl = "https://celkiboi.github.io/QGMBlog/articles/Article2/article.html";
    const int ExpectedAsideArticleCount = 3;
    readonly string[] ExpectedAsideTitles = [
        "Rimac Nevera tested - the fastest car in the world?",
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
