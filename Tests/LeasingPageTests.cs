using Microsoft.Playwright;
using NUnit.Framework;
using System.Threading.Tasks;

namespace QuickGarageManagerTesting.Tests;

public class LeasingPageTests : BaseTest
{
    const string LeasingUrl = "https://celkiboi.github.io/QGMBlog/pages/leasing.html";
    const string ExpectedTitle = "Leasing - QGM Blog";
    const int ExpectedNavLinkCount = 5;
    readonly string[] ExpectedLinks = ["Homepage", "News", "Leasing calculator", "App", "About"];
    const string ExpectedFooterText = "Quick garage manager blog. All rights reserved 2023.";

    [SetUp]
    public async override Task SetUp()
    {
        await base.SetUp();
        await page.GotoAsync(LeasingUrl, new PageGotoOptions { Timeout = 10000 });
    }

    [Test]
    public async Task PageTitleShouldBeCorrect()
    {
        string title = await page.TitleAsync();
        string message = $"The page title is incorrect. Expected {ExpectedTitle}, got: {title}";
        Assert.That(title, Is.EqualTo(ExpectedTitle), message);
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
    public async Task FooterShouldBePresent()
    {
        IElementHandle? footer = await page.QuerySelectorAsync(".alt_footer");
        string message = "Footer is missing";
        Assert.That(footer, Is.Not.Null, message);

        IElementHandle? logoFooter = await page.QuerySelectorAsync("img[src='../images/qgm_blog_logo.png']");
        message = "Footer logo is missing";
        Assert.That(logoFooter, Is.Not.Null, message);

        string footerText = await footer.InnerTextAsync();
        message = $"Footer text is incorrect. Expected {ExpectedFooterText}, got {footerText}";
        Assert.That(footerText, Is.EqualTo(ExpectedFooterText), message);
    }

    [Test]
    public async Task LeasingCalculatorShouldBePresent()
    {
        IElementHandle? calculator = await page.QuerySelectorAsync(".calculator_container");
        string message = "Leasing calculator section is missing";
        Assert.That(calculator, Is.Not.Null, message);

        IElementHandle? heading = await calculator.QuerySelectorAsync("h2");
        message = "Leasing calculator heading is missing";
        Assert.That(heading, Is.Not.Null, message);
    }

    [TestCase("USD", "$")]
    [TestCase("EUR", "€")]
    [TestCase("GBP", "£")]
    public async Task CurrencySymbolShouldUpdateCorrectly(string currency, string expectedSymbol)
    {
        await page.SelectOptionAsync("#currency", new SelectOptionValue { Label = currency });

        string currencySymbolCarPrice = await page.InnerTextAsync("#currencySymbolCarPrice");
        string currencySymbolDownPayment = await page.InnerTextAsync("#currencySymbolDownPayment");
        string selectedCurrencyResult = await page.InnerTextAsync("#selectedCurrencyResult");

        string message = $"Currency symbol should be {expectedSymbol}, but got: {currencySymbolCarPrice}";
        Assert.That(currencySymbolCarPrice, Is.EqualTo(expectedSymbol), message);

        message = $"Currency symbol should be {expectedSymbol}, but got: {currencySymbolDownPayment}";
        Assert.That(currencySymbolDownPayment, Is.EqualTo(expectedSymbol), message);

        message = $"Currency symbol should be {expectedSymbol}, but got: {selectedCurrencyResult}";
        Assert.That(selectedCurrencyResult, Is.EqualTo(expectedSymbol), message);
    }

    [TestCase(20000, 5000, 36, 5, 449.56)]
    [TestCase(35000, 7000, 60, 3, 503.12)]
    [TestCase(50000, 15000, 48, 4.5, 798.12)]
    [TestCase(10000, 2000, 24, 7, 358.17)]
    [TestCase(30000, 10000, 72, 6, 331.46)]
    [TestCase(15000, 2000, 36, 8, 407.37)]
    [TestCase(45000, 10000, 60, 4, 644.58)]
    public async Task MonthlyPaymentShouldBeCalculatedCorrectly(decimal carPrice, decimal downPayment, int loanTerm, decimal interestRate, decimal expectedMonthlyPayment)
    {
        await page.FillAsync("#carPrice", carPrice.ToString());
        await page.FillAsync("#downPayment", downPayment.ToString());
        await page.FillAsync("#loanTerm", loanTerm.ToString());
        await page.FillAsync("#interestRate", interestRate.ToString());

        await page.ClickAsync("#calculate_button");

        string monthlyPayment = await page.InnerTextAsync("#monthlyPayment");
        string message = $"The calculated monthly payment is incorrect. Expected {expectedMonthlyPayment}, got: {monthlyPayment}";

        bool isValid = decimal.TryParse(monthlyPayment, out decimal actualPayment);

        Assert.Multiple(() =>
        {
            Assert.That(isValid, Is.True, message);
            Assert.That(actualPayment, Is.EqualTo(expectedMonthlyPayment).Within(0.01), message);
        });
    }
}

