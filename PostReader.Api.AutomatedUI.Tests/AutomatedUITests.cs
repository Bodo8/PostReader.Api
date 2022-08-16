using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using Xunit;

namespace PostReader.Api.AutomatedUI.Tests
{
    public class AutomatedUITests : IDisposable
    {
        private readonly IWebDriver _driver;
        private readonly DefaultWait<IWebDriver> _fluentWait;
        private const string URL = "https://localhost:7133/WebsitesReader";

        public AutomatedUITests()
        {
            _driver = new ChromeDriver();
            _fluentWait = new DefaultWait<IWebDriver>(_driver);
            _fluentWait.Timeout = TimeSpan.FromSeconds(8);
            _fluentWait.PollingInterval = TimeSpan.FromMilliseconds(250);
            _fluentWait.IgnoreExceptionTypes(typeof(NoSuchElementException));
            _fluentWait.Message = "Element to be searched not found";
            _driver.Navigate().GoToUrl(URL);
        }

        public void Dispose()
        {
            _driver.Quit();
            _driver.Dispose();
        }

        [Fact]
        public void Index_WhenExecuted_ReturnsSearchView()
        {
            Assert.Equal("Search Publications - PostReader.Api", _driver.Title);
        }

        [Fact]
        public void SearchPublications_WhenExecuted_ReturnsTotalCountPublication()
        {
            _driver.FindElement(By.Id("Sentence"))
                .SendKeys("home car kitchen office mapping");
            _driver.FindElement(By.Id("Search")).Click();
            var rowsElement = _fluentWait.Until(x => x.FindElements(By.Id("ResultRow")));
            int rows = rowsElement.Count;
            IWebElement totalPagesElem = _driver.FindElement(By.Id("totalPages"));
            string text = totalPagesElem.GetAttribute("text");
            int.TryParse(text, out int totalPages);
            IWebElement elementCount = _driver.FindElement(By.Id("totalCount"));
            string textCount = elementCount.GetAttribute("textContent");
            int totalCount = ExtractTotalRecords(textCount);

            for (int i = 1; i < totalPages; i++)
            {
                IWebElement link = _driver.FindElement(By.LinkText("Next"));
                string url = link.GetAttribute("href");
                _driver.Navigate().GoToUrl(url);
                var rowElement = _fluentWait.Until(x => x.FindElements(By.Id("ResultRow")));
                rows += rowElement.Count();
            }

            Assert.Equal(188, rows);
        }

        [Fact]
        public void SearchPublications_WhenExecuted_ReturnsMessageAboutLackOfPublication()
        {
            _driver.FindElement(By.Id("Sentence"))
                .SendKeys("gdfghghfffff");
            _driver.FindElement(By.Id("Search")).Click();

            IWebElement messageElement = _fluentWait.Until(x => x.FindElement(By.Id("NoRecords")));
            string noElements = messageElement.GetAttribute("textContent");

            Assert.Equal("Brak pasuj¹cych publikacji", noElements);
        }

        private static int ExtractTotalRecords(string textCount)
        {
            Regex regex = new Regex("(.)(?<values>\\d[0-9]*)");
            var matched = regex.Match(textCount);
            string? result = matched.Groups["values"]?.Value?.Trim();
            int.TryParse(result, out int totalCount);
            return totalCount;
        }
    }
}