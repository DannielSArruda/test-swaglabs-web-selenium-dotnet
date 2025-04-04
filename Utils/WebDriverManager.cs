using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;

namespace SauceDemoTests.Utils
{
    public class WebDriverManager
    {
        private static IWebDriver driver;

        public static IWebDriver GetDriver()
        {
            if (driver == null)
            {
                try
                {
                    ChromeOptions options = new ChromeOptions();
                    options.AddArgument("--start-maximized");
                    driver = new ChromeDriver(options);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao iniciar o WebDriver: {ex.Message}");
                    throw;
                }
            }
            return driver;
        }

        public static void QuitDriver()
        {
            driver?.Quit();
            driver = null;
        }
    }
}
