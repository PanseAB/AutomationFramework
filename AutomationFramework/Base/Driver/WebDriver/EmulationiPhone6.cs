﻿using OpenQA.Selenium.Chrome;
using AutomationFrameWork.Helper;
using OpenQA.Selenium;

namespace AutomationFrameWork.Driver.Core
{
    class EmulationiPhone6 : Drivers
    {
        private static readonly EmulationiPhone6 _instance = new EmulationiPhone6();
        static EmulationiPhone6 ()
        {
        }
        private EmulationiPhone6 ()
        {
        }

        public static EmulationiPhone6 Instance
        {
            get
            {
                return _instance;
            }
        }
        protected override object StartDriver (int pageLoadTimeout = 60, int scriptTimeout = 60, bool isMaximize = false)
        {
            //Drivers.DriverStorage = new ChromeDriver(DriverHelper.Instance.DriverPath, (ChromeOptions)EmulationiPhone6.Instance.DriverOption);
            IWebDriver driver = new ChromeDriver(DriverHelper.Instance.DriverPath, (ChromeOptions)EmulationiPhone6.Instance.DriverOption);
            driver.Manage().Timeouts().SetPageLoadTimeout(System.TimeSpan.FromSeconds(pageLoadTimeout));
            driver.Manage().Timeouts().SetScriptTimeout(System.TimeSpan.FromSeconds(scriptTimeout));
            if (isMaximize)
                driver.Manage().Window.Maximize();
            return driver;
        }

        protected override object DriverOption
        {
            get
            {
                ChromeOptions op = (ChromeOptions)Drivers.DriverOptions;
                if (op == null)
                    op = new ChromeOptions();
                op.EnableMobileEmulation("Apple iPhone 6");
                return op;
            }
        }
    }
}