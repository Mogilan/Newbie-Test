using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Threading;

namespace csharp_example
{
    [TestFixture]
    public class UnitTest1
    {
        private IWebDriver driver;
        private WebDriverWait wait;

        [SetUp]
        public void Start()
        {
            ChromeOptions option = new ChromeOptions();
            option.AddArgument("--start-maximized");
            driver = new ChromeDriver(option);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
           



        }

        [Test]
        public void TestMethod1()
        {
            driver.Url = "http://qa.fundist.org";
            driver.FindElement(By.Name("Login")).SendKeys("ui_admin");
            driver.FindElement(By.Name("Password")).SendKeys("Test123!");
            driver.FindElement(By.CssSelector("[type='submit']")).Click();
            //Thread.Sleep(10000);
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id("MaintenanceButton")));
            driver.FindElement(By.Id("MaintenanceButton"));
            driver.FindElement(By.Id("SideMenu-Payments")).Click();
            driver.FindElement(By.Id("SideMenu-Payments-Config")).Click();
            driver.FindElement(By.CssSelector("[id^= 'select2-IDApi']")).Click();
            var selectElement = new SelectElement(driver.FindElement(By.Name("IDApi")));
            selectElement.SelectByValue("80");
            driver.FindElement(By.Id("ButtonFilter")).Click();
            Thread.Sleep(5000);
            var elc = driver.FindElement(By.Id("Payments_Config125")).GetAttribute("class");
            if (elc.Trim() == "active")
            {
                driver.FindElement(By.XPath("//table//tr[@id='Payments_Config125']//a[@name='Action-status']")).Click();
                var modalEl = driver.FindElement(By.Id("DisableReasonModal"));

                wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id("DisableReasonModal")));
                var el = driver.FindElement(By.Id("DisableReasonInputText"));
                el.Clear();
                el.SendKeys("disable test");
                driver.FindElement(By.Id("DisableReasonOkButton")).Click();
                wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.StalenessOf(modalEl));

                /*Thread.Sleep(500);
                var modalEls = driver.FindElements(By.Id("DisableReasonModal"));
                for (int t = 0; t < 10; t++)
                {
                    if (modalEls.Count > 0)
                    {
                        Thread.Sleep(500);
                        modalEls = driver.FindElements(By.Id("DisableReasonModal"));
                    }
                    else
                    {
                        continue;
                    }
                    if (t == 9 && modalEls.Count>0)
                    {
                        throw new Exception("pizdec3");
                    }
                }*/

                elc = driver.FindElement(By.Id("Payments_Config125")).GetAttribute("class");
                for (int i = 0; i < 10; i++)
                {
                    if (elc.Trim() == "inactive")
                    {
                        continue;
                    }
                    else
                    {
                        Thread.Sleep(300);
                        elc = driver.FindElement(By.Id("Payments_Config125")).GetAttribute("class");
                    }
                    if (i == 9 && elc.Trim() == "active")
                    {
                        throw new Exception("pizdec2");
                    }
                }

            }

            else if (elc.Trim() == "inactive")
            {
                
                driver.FindElement(By.XPath("//table//tr[@id='Payments_Config125']//a[@name='Action-status']")).Click();
                var modalEl1 = driver.FindElement(By.Id("ConfirmModal"));
                wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id("ConfirmModal")));
                var el = driver.FindElement(By.Id("ConfirmInputText"));
                el.Clear();
                el.SendKeys("enable test");
                driver.FindElement(By.Id("ConfirmOkButton")).Click();
                wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.StalenessOf(modalEl1));
            }
            else
            {
                throw new Exception("pizdec");
                //Assert.Fail();
                //Assert.Warn();
            }
            Thread.Sleep(10000);
            




        }

        [TearDown]
        public void stop()
        {
            driver.Quit();
            driver = null;
        }
    }
}
