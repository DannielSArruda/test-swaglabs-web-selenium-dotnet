using OpenQA.Selenium;

namespace SauceDemoTests.Pages
{
    public class LoginPage
    {
        private IWebDriver driver;
        private By usernameField = By.Id("user-name");
        private By passwordField = By.Id("password");
        private By loginButton = By.Id("login-button");
        private By errorAlert = By.XPath("//h3[@data-test='error']");
        private By cartButton = By.Id("shopping_cart_container");
        private By cartTitle = By.Id("//button[text()='Your Cart']");
        private By menuButton = By.XPath("//button[text()='Open Menu']");
        private By logoutButton = By.Id("logout_sidebar_link");
        private By cartBadge = By.XPath("//span[@class='fa-layers-counter shopping_cart_badge']");
        private By removeButton = By.XPath("//button[text()='REMOVE']");
        private By checkoutButton = By.XPath("//a[text()='CHECKOUT']");
        private By firstNameField = By.Id("first-name");
        private By lastNameField = By.Id("last-name");
        private By zipCodeField = By.Id("postal-code");
        private By contineuCheckoutButton = By.XPath("//input[@value='CONTINUE']");
        private By finishCheckoutButton = By.XPath("//a[text()='FINISH']");
        private By thanksCheckoutTitle = By.XPath("//h2[text()='THANK YOU FOR YOUR ORDER']");
        private By cancelCheckoutButton = By.XPath("//a[text()='CANCEL']");

        public LoginPage(IWebDriver driver)
        {
            this.driver = driver;
        }

        public void Login(string username, string password)
        {
            driver.FindElement(usernameField).SendKeys(username);
            driver.FindElement(passwordField).SendKeys(password);
            driver.FindElement(loginButton).Click();
        }

        public bool IsLoginSuccessful()
        {
            try
            {
                return driver.Url.Contains("inventory.html") && driver.FindElement(cartButton).Displayed;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        public bool IsLoginFailed(string expectedMessage)
        {
            try
            {
                IWebElement errorElement = driver.FindElement(errorAlert);
                return errorElement.Displayed && errorElement.Text == expectedMessage;

            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        public void Logout()
        {
            driver.FindElement(menuButton).Click();
            Thread.Sleep(1000);
            driver.FindElement(logoutButton).Click();
        }

        public void AddProductToCart(string productName)
        {
            var productElement = driver.FindElement(By.XPath($"//div[@class='inventory_item' and .//div[@class='inventory_item_name' and text()='{productName}']]"));

            var addToCartButton = productElement.FindElement(By.XPath(".//button[contains(text(),'ADD TO CART')]"));

            addToCartButton.Click();
        }

        public bool IsCartItemCountCorrect(int expectedCount)
        {
            try
            {
                var cartBadgeString = driver.FindElement(cartBadge);
                int cartBadgeNumber = int.Parse(cartBadgeString.Text);
                return cartBadgeNumber == expectedCount;
            }
            catch (NoSuchElementException)
            {
                return expectedCount == 0;
            }
        }

        public void ClickRemoveProduct()
        {
            driver.FindElement(removeButton).Click();
        }

        public void OpenCart()
        {
            driver.FindElement(cartButton).Click();

            try
            {
                IWebElement cartTitleElement = driver.FindElement(cartTitle);
                bool isDisplayed = cartTitleElement.Displayed;
                Console.WriteLine($"Cart ttitle is visible? {isDisplayed}");
            }
            catch (NoSuchElementException)
            {
                Console.WriteLine("Cart title was not found");
            }
        }

        public void ClickCheckout()
        {
            driver.FindElement(checkoutButton).Click();
        }

        public void FillCheckoutInfo(string firstName, string lastName, string zipCode)
        {
            driver.FindElement(firstNameField).SendKeys(firstName);
            driver.FindElement(lastNameField).SendKeys(lastName);
            driver.FindElement(zipCodeField).SendKeys(zipCode);
            driver.FindElement(contineuCheckoutButton).Click();
        }

        public void FinishCheckout()
        {
            driver.FindElement(finishCheckoutButton).Click();
        }

        public bool CheckoutConfirmed()
        {

            try
            {
                IWebElement thanksCheckoutTitleElement = driver.FindElement(thanksCheckoutTitle);
                return thanksCheckoutTitleElement.Displayed;

            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        public void CancelCheckout()
        {
            driver.FindElement(cancelCheckoutButton).Click();
        }

        public bool CheckoutCancelled()
        {

            try
            {
                bool itemStillInCart = IsCartItemCountCorrect(1);
                bool isInProductList = IsLoginSuccessful();
                return itemStillInCart && isInProductList;

            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        public string GetCurrentUrl()
        {
            return driver.Url;
        }
    }
}
