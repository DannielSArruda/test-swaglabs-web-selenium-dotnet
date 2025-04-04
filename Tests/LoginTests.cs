using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using SauceDemoTests.Pages;
using SauceDemoTests.Utils;

namespace SauceDemoTests.Tests
{
    [TestClass]
    public class LoginTests
    {
        private IWebDriver driver;
        private LoginPage loginPage;

        [TestInitialize]
        public void Setup()
        {
            driver = WebDriverManager.GetDriver();

            if (driver == null)
            {
                throw new InvalidOperationException("WebDriver was not initialized correctly.");
            }

            driver.Navigate().GoToUrl("https://www.saucedemo.com/v1/index.html");
            loginPage = new LoginPage(driver);
        }

        [TestMethod]
        public void ValidLoginTest()
        {
            loginPage.Login("standard_user", "secret_sauce");
            Assert.IsTrue(loginPage.IsLoginSuccessful(), "Validate that the login was successful.");
        }

        [TestMethod]
        public void InvalidLoginWithInvalidCredentialsTest()
        {
            loginPage.Login("invalid_user", "invalid_password");

            string expectedErrorMessage = "Epic sadface: Username and password do not match any user in this service";
            Assert.IsTrue(loginPage.IsLoginFailed(expectedErrorMessage), "Validate the error message return");

            string expectedLoginUrl = "https://www.saucedemo.com/v1/index.html";
            Assert.AreEqual(expectedLoginUrl, loginPage.GetCurrentUrl(), "Validate that you remain on the login screen");
        }

        [TestMethod]
        public void ValidLoginAndLogoutTest()
        {
            loginPage.Login("standard_user", "secret_sauce");
            Assert.IsTrue(loginPage.IsLoginSuccessful(), "Validate that the login was successful.");
            loginPage.Logout();
            string expectedLoginUrl = "https://www.saucedemo.com/v1/index.html";
            Assert.AreEqual(expectedLoginUrl, loginPage.GetCurrentUrl());
        }

        [TestMethod]
        public void AddProductToCart()
        {
            loginPage.Login("standard_user", "secret_sauce");
            Assert.IsTrue(loginPage.IsLoginSuccessful(), "Validate that the login was successful.");
            loginPage.AddProductToCart("Sauce Labs Bolt T-Shirt");
            Thread.Sleep(1000);
            Assert.IsTrue(loginPage.IsCartItemCountCorrect(1), "Validate that you have the number of products in the cart.");

        }

        [TestMethod]
        public void RemoveProductFromCart()
        {
            loginPage.Login("standard_user", "secret_sauce");
            Assert.IsTrue(loginPage.IsLoginSuccessful(), "Validate that the login was successful.");

            loginPage.AddProductToCart("Sauce Labs Bike Light");
            Thread.Sleep(1000);
            Assert.IsTrue(loginPage.IsCartItemCountCorrect(1), "Validate that you have the number of products in the cart.");

            loginPage.ClickRemoveProduct();
            Thread.Sleep(1000);
            Assert.IsTrue(loginPage.IsCartItemCountCorrect(0), "Validate that the cart is empty");

            loginPage.AddProductToCart("Sauce Labs Bolt T-Shirt");
            Thread.Sleep(1000);
            Assert.IsTrue(loginPage.IsCartItemCountCorrect(1), "Validate that you have the number of products in the cart.");

            loginPage.OpenCart();
            Thread.Sleep(1000);

            loginPage.ClickRemoveProduct();
            Thread.Sleep(1000);
            Assert.IsTrue(loginPage.IsCartItemCountCorrect(0), "Validate that the cart is empty");

        }

        [TestMethod]
        public void CheckoutProduct()
        {
            loginPage.Login("standard_user", "secret_sauce");
            Assert.IsTrue(loginPage.IsLoginSuccessful(), "Validate that the login was successful.");

            loginPage.AddProductToCart("Sauce Labs Bike Light");
            Thread.Sleep(1000);
            Assert.IsTrue(loginPage.IsCartItemCountCorrect(1), "Validate that you have the number of products in the cart.");

            loginPage.OpenCart();
            Thread.Sleep(1000);

            loginPage.ClickCheckout();
            Thread.Sleep(1000);
            loginPage.FillCheckoutInfo("Daniel", "Silva", "12345678");
            Thread.Sleep(1000);
            loginPage.FinishCheckout();
            Thread.Sleep(1000);
            Assert.IsTrue(loginPage.CheckoutConfirmed(), "Validate that the order has been confirmed");

        }

        [TestMethod]
        public void CancelCheckoutProduct()
        {
            loginPage.Login("standard_user", "secret_sauce");
            Assert.IsTrue(loginPage.IsLoginSuccessful(), "Validate that the login was successful.");

            loginPage.AddProductToCart("Sauce Labs Bike Light");
            Thread.Sleep(1000);
            Assert.IsTrue(loginPage.IsCartItemCountCorrect(1), "Validate that you have the number of products in the cart.");

            loginPage.OpenCart();
            Thread.Sleep(1000);

            loginPage.ClickCheckout();
            Thread.Sleep(1000);
            loginPage.FillCheckoutInfo("Daniel", "Silva", "12345678");
            Thread.Sleep(1000);
            loginPage.CancelCheckout();
            Thread.Sleep(1000);
            Assert.IsTrue(loginPage.CheckoutCancelled(), "Validate that the order has been cancelled and returned to the product list.");

        }

        [TestCleanup]
        public void TearDown()
        {
            driver.Quit();
        }
    }
}
