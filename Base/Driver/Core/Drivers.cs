﻿using OpenQA.Selenium;
using System.Threading;
using OpenQA.Selenium.Remote;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Reflection;

namespace AutomationFrameWork.Driver.Core
{
    abstract class Drivers
    {
        protected static ThreadLocal<object> driverStored = new ThreadLocal<object>(true);
        protected static ThreadLocal<DesiredCapabilities> desiredCapabilities = new ThreadLocal<DesiredCapabilities>();
        protected static ThreadLocal<object> optionStorage = new ThreadLocal<object>();
        protected static ThreadLocal<Dictionary<string, string>> remoteInfo = new ThreadLocal<Dictionary<string, string>>();
        private static readonly object _syncRoot = new Object();

        /// <summary>
        /// This method is use for
        /// scan all class driver with correct name via DriverType 
        /// and invoke method StartDriver
        /// </summary>
        /// <param name="driverType"></param>
        protected static void StartDrivers (DriverType driverType)
        {
            lock (_syncRoot)
            {
                List<Type> listClass = System.Reflection.Assembly.GetExecutingAssembly().GetTypes()
                          .Where(item => item.Namespace == "AutomationFrameWork.Driver.Core")
                          .ToList();
                foreach (Type className in listClass)
                {
                    if (className.Name.ToString().ToLower().Equals(driverType.ToString().ToLower()))
                    {
                        MethodInfo startDriver = className.GetMethod("StartDriver", BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.NonPublic);
                        FieldInfo instance = className.GetField("_instance",
                            BindingFlags.Static | BindingFlags.NonPublic);
                        object instanceDriver = instance.GetValue(null);
                        startDriver.Invoke(instanceDriver, Type.EmptyTypes);
                        break;
                    }
                }
            }
        }
        /// <summary>
        /// This method use for close driver 
        /// </summary>
        protected static void CloseDrivers ()
        {
            IWebDriver _driver = (IWebDriver)driverStored.Value;
            _driver.Quit();
            if (optionStorage.Value != null)
                optionStorage.Value = null;
            if (desiredCapabilities.Value != null)
                desiredCapabilities.Value = null;
            if (remoteInfo.Value != null)
                remoteInfo.Value.Clear();
            if (driverStored.Value != null)
                driverStored.Value = null;
        }
        /// <summary>
        /// This method is use 
        /// for return object with can be MobileDriver or WebDriver
        /// </summary>
        /// <returns></returns>
        protected static object DriverStorage
        {
            get
            {
                return driverStored.Value;
            }
            set
            {
                driverStored.Value = value;
            }
        }
        /// <summary>
        /// This method is use 
        /// for setting DesiredCapabilities for Remote Driver, Firefox Driver, PhantomJs Driver
        /// </summary>
        protected static DesiredCapabilities DesiredCapabilities
        {
            get
            {
                if (Drivers.desiredCapabilities.Value == null)
                    Drivers.desiredCapabilities.Value = new DesiredCapabilities();
                return Drivers.desiredCapabilities.Value;
            }
            set
            {
                Drivers.desiredCapabilities.Value = value;
            }
        }
        /// <summary>
        /// This method is use
        /// for return DriverOption like ChromeOption, InternetExplorerOption
        /// </summary>
        protected static object DriverOptions
        {
            get
            {
                return optionStorage.Value;
            }
            set
            {
                optionStorage.Value = value;
            }
        }
        /// <summary>
        /// This method is use
        /// for return RemoteInfo like address and port in appium , RemoteDriver
        /// </summary>
        protected static Dictionary<string, string> RemoteInfo
        {
            get
            {
                return remoteInfo.Value;
            }
            set
            {
                remoteInfo.Value = value;
            }
        }
        /// <summary>
        /// This method is use for
        /// Start driver for any class extend Drivers
        /// </summary>    
        virtual protected void StartDriver ()
        {
        }
        /// <summary>
        /// This method is use for
        /// Get DriverOption for any class extend Drivers
        /// </summary>
        virtual protected object DriverOption
        {
            get;
        }
    }
}
