using System;
using System.Collections.Generic;
using System.Threading;
using System.Diagnostics;
using System.IO;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

// установи
// Selenium.WebDriver
// Selenium.WebDriver.ChromeDriver
// DotNetSeleniumExtras.WaitHelpers

// для установки средства -> диспетчер покетов NUGET -> управление пакетами NUGET для расширений -> обзор -> вводишь названия и скачиваешь

class Program
{
    static void Main()
    {
        // пути и ссылки
        string userName = "Mark"; // это имя компа
        string automationFolder = @"C:\ChromeAutomation"; //создай папку по этому пути

        List<string> videoUrls = new List<string> // это ссылки на уроки
        {
            "https://kod.mob-edu.ru/mod/event3kl/external_record.php?him=98569", //4.4.2
            "https://kod.mob-edu.ru/mod/event3kl/external_record.php?him=92245", //4.4.3
            "https://kod.mob-edu.ru/mod/event3kl/external_record.php?him=94871", //4.4.4
            "https://kod.mob-edu.ru/mod/event3kl/external_record.php?him=94872", //4.4.5
            "https://kod.mob-edu.ru/mod/event3kl/external_record.php?him=94873", //4.4.6
            "https://kod.mob-edu.ru/mod/event3kl/external_record.php?him=94874", //4.4.7
        };

        //  -хром
        Console.WriteLine("-хром");
        foreach (var process in Process.GetProcessesByName("chrome")) { try { process.Kill(); } catch { } }
        Thread.Sleep(2000);

        if (!Directory.Exists(automationFolder)) Directory.CreateDirectory(automationFolder);

        // перезапуск хрома
        ChromeOptions options = new ChromeOptions();
        options.AddArgument($"--user-data-dir={automationFolder}");
        options.AddArgument("--profile-directory=Default");
        options.AddArgument("--remote-debugging-port=9222");
        options.AddArgument("--no-sandbox");
        options.AddArgument("--start-maximized");
        options.AddArgument("--mute-audio");

        ChromeDriverService service = ChromeDriverService.CreateDefaultService();
        service.HideCommandPromptWindow = true;

        try
        {
            using (IWebDriver driver = new ChromeDriver(service, options, TimeSpan.FromMinutes(2)))
            {
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));

                foreach (var url in videoUrls)
                {
                    try
                    {
                        Console.WriteLine($"\n[{DateTime.Now:HH:mm}] вкл: {url}");
                        driver.Navigate().GoToUrl(url);
                        Thread.Sleep(10000);

                        // загрузка 2035
                        try
                        {
                            var authBtn = wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("a[href*='2035']")));
                            Console.WriteLine("2035");
                            authBtn.Click();
                            Thread.Sleep(15000);
                        }
                        catch { Console.WriteLine("2035 пон"); }

                        // вкл
                        Console.WriteLine("поиск вкл");

                        // вкл
                        var frames = driver.FindElements(By.TagName("iframe"));
                        if (frames.Count > 0) driver.SwitchTo().Frame(0);

                        try
                        {
                            // поиск вкл
                            IWebElement smallBtn = wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector(".vjs-play-control")));


                            string btnTitle = smallBtn.GetAttribute("title");
                            if (btnTitle.ToLower().Contains("play") || !smallBtn.GetAttribute("class").Contains("vjs-playing"))
                            {
                                Console.WriteLine("вкл 2");
                                ((IJavaScriptExecutor)driver).ExecuteScript("arguments.click();", smallBtn);

                                // еще вкл
                                Thread.Sleep(2000);
                                ((IJavaScriptExecutor)driver).ExecuteScript("var v = document.querySelector('video'); if(v && v.paused) v.play();");
                            }
                            else
                            {
                                Console.WriteLine("видео вкл");
                            }
                        }
                        catch (Exception ex)// еще вкл
                        {
                            Console.WriteLine("вкл 3");
                            ((IJavaScriptExecutor)driver).ExecuteScript("var v = document.querySelector('video'); if(v) v.play();");
                        }

                        driver.SwitchTo().DefaultContent();
                        Console.WriteLine("видео работает ");

                        // таймер
                        for (int i = 1; i <= 65; i++)
                        {
                            Thread.Sleep(60000);
                            if (i % 10 == 0) Console.WriteLine($"прошло {i} мин из 65.");
                            if (driver.WindowHandles.Count == 0) break;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"ошибка: {ex.Message}");// ошибки
                        driver.SwitchTo().DefaultContent();
                    }
                }
            }
        }
        // конец просмотра и ошибки
        catch (Exception ex) { Console.WriteLine($"ошибка: {ex.Message}"); }

        Console.WriteLine("вск");
        Console.ReadLine();
    }
}
