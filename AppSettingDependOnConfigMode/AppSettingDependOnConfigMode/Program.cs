﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppSettingDependOnConfigMode
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine(ConfigurationManager.AppSettings["testProp"]);

            DevModeSetting devMode =new DevModeSetting();


            //Get Mode from Arg
            //devMode = DevModeSettingHandler.GetDevModeSetting(args[0]);
            //Get Mode from Code
            devMode = DevModeSettingHandler.GetDevModeSetting("debug");

            //Read From Section
            Console.WriteLine(devMode.TestProp);
            Console.ReadLine();

            //Read From AppSetting
            Console.WriteLine(ConfigurationManager.AppSettings["testProp"]);
            Console.ReadLine();

            //Update AppSetting From Section
            ConfigurationManager.AppSettings["testProp"] = devMode.TestProp;
            Console.WriteLine(ConfigurationManager.AppSettings["testProp"]);

            devMode.TestProp = ConfigurationManager.AppSettings["testProp"];
            Console.WriteLine(devMode.TestProp);
            Console.ReadLine();
        }
    }
}
