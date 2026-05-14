using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using FluentAssertions;
using Xunit;

namespace com.split.backend.Tests.SystemTests
{
    public class LoginSystemTests : IDisposable
    {
        private readonly IWebDriver _driver;
        private readonly string _baseUrl = "https://budgetly-exp-app.web.app";

        public LoginSystemTests()
        {
            var options = new ChromeOptions();
            options.AddArgument("--disable-blink-features=AutomationControlled");
            _driver = new ChromeDriver(options);
            _driver.Manage().Window.Maximize();
        }

        [Fact]
        public void WebApp_Production_UserCanLoginSuccessfully()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/login");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(20));

            var emailInput = wait.Until(d => d.FindElement(By.CssSelector("input[type='email']")));
            emailInput.Clear();
            emailInput.SendKeys("usuarionuevo@yopmail.com");

            var passwordInput = wait.Until(d => d.FindElement(By.CssSelector("input[type='password']")));
            passwordInput.Clear();
            passwordInput.SendKeys("12345678");


            var loginButton = wait.Until(d => d.FindElement(By.XPath("//button[contains(., 'Sign In')]")));

            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", loginButton);

            loginButton.Click();

            wait.Until(d => d.Url.Contains("/dashboard") || d.Url.Contains("/home"));

            _driver.Url.Should().ContainAny("/dashboard", "/home");

            Thread.Sleep(10000);
        }

        public void Dispose()
        {
            _driver.Quit();
            _driver.Dispose();
        }
    }
}