using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Linq;
using System.Threading;
using Xunit;

namespace PostReader.Api.AutomatedUI.Tests
{
    public class AutomatedUITests : IDisposable
    {
        private readonly IWebDriver _driver;
        public AutomatedUITests() => _driver = new ChromeDriver();

        public void Dispose()
        {
            _driver.Quit();
            _driver.Dispose();
        }

        [Fact]
        public void Index_WhenExecuted_ReturnsSearchView()
        {
            _driver.Navigate()
                .GoToUrl("https://localhost:7133/WebsitesReader");

            Assert.Equal("Search Publications - PostReader.Api", _driver.Title);
        }

        [Fact]
        public void SearchPublications_WhenExecuted_ReturnsPublication()
        {
            _driver.Navigate()
                .GoToUrl("https://localhost:7133/WebsitesReader");

            _driver.FindElement(By.Id("Sentence"))
        .SendKeys("Test");

            _driver.FindElement(By.Id("Search"))
       .Click();

            int rows = _driver.FindElements(By.Id("ResultRow")).Count;
            var totalPagesElem = _driver.FindElement(By.Id("totalPages"));
            string text = totalPagesElem.GetAttribute("text");
            int.TryParse(text, out int totalPages);

            for (int i = 1; i < totalPages; i++)
            {
                var link = _driver.FindElement(By.LinkText("Next"));
                var url = link.GetAttribute("href");
                _driver.Navigate().GoToUrl(url);
                WebDriverWait webDriverWait = new WebDriverWait(_driver, TimeSpan.FromSeconds(2));
                rows += _driver.FindElements(By.Id("ResultRow")).Count;
            }

            Assert.Equal(62, rows);
            Assert.Equal("Publications - PostReader.Api", _driver.Title);
        }
    }
}