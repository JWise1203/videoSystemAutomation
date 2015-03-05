using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

using System.Collections;
using System.Collections.Specialized;

using System.Net;
using System.Net.Sockets;
using System.IO;
using Newtonsoft.Json;
using khVSAutomation;

using rv;

using hallAutomations = khVSAutomation.Automation;

namespace videoSystemAutomationApp
{
    class Program
    {
        static DateTime StartTime;

        private static PJLinkConnection c = null;

        static void Main(string[] args)
        {
            StartTime = DateTime.Now;

            Console.SetWindowSize(120, 50);
            Console.Clear();

            DisplayMainUserInterface();
                  
        }

        private static void DisplayMainUserInterface()
        {
            string operation = "";
            var version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;

            StringDictionary OperationStringList = new StringDictionary();

            OperationStringList.Add("A", "Turn on system");
            OperationStringList.Add("B", "Turn off system");
            OperationStringList.Add("C", "Check projector status");
            OperationStringList.Add("D", "Power On TV");
            OperationStringList.Add("E", "In Progress - Power Off TV");
            OperationStringList.Add("F", "Register App with TV");
            OperationStringList.Add("Y", "Check Current Settings");
            OperationStringList.Add("Z", "Exit application");

            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("====================================================================================");
            Console.WriteLine("Welcome to the Video Automation System Applicaiton");
            Console.WriteLine("Version: " + string.Format("v{0}.{1}.{2} ({3})", version.Major, version.Minor, version.Build, version.Revision));
            Console.WriteLine("====================================================================================");
            Console.WriteLine("");

            string key;
            string values;
            SortedList sortList = new SortedList();

            foreach (DictionaryEntry pair in OperationStringList)
            {
                key = pair.Key.ToString();
                values = pair.Value.ToString();
                sortList.Add(key.ToString(), values.ToString());
            }

            foreach (DictionaryEntry pair in sortList)
            {
                Console.WriteLine("     {0}:  {1}", pair.Key.ToString().ToUpper(), pair.Value);
            }

            Console.WriteLine("");

            while (true)
            {
                Console.Write("Please select the operation you wish to excute: ");
                operation = Console.ReadLine();
                Console.WriteLine("");

                if (OperationStringList.ContainsKey(operation))
                {
                    break;
                }

                Console.Write("Error! You must enter a valid index character.");
                Console.WriteLine("");
            }

            OperationLogicProcessor(operation.ToUpper());
        }
        
        private static void RunAnotherOperation()
        {
            string line;

            while (true)
            {
                Console.WriteLine("");
                Console.Write("Would you like to execute another command? (Y/N): ");
                line = Console.ReadLine();

                if (line == "Y" | line == "y" | line == "N" | line == "n")
                    break;
            }

            if (line == "Y" | line == "y")
                DisplayMainUserInterface();
            else
                Console.WriteLine("");
            Console.WriteLine("Exiting application...");
            System.Environment.Exit(1);
        }

        private static void OperationLogicProcessor(String operation)
        {
            try
            {
                switch (operation)
                {
                    case "A":
                        turnOnSystem();
                        
                        RunAnotherOperation();
                        break;
                    case "B":
                        turnOffSystem();

                        RunAnotherOperation();
                        break;
                    case "C":
                        Console.Write(hallAutomations.printProjectorStatus());

                        RunAnotherOperation();
                        break;
                    case "D":
                        Console.Write(hallAutomations.WakeupTV());

                        RunAnotherOperation();
                        break;
                    case "E":
                        Console.Write(hallAutomations.PowerOffTV());

                        RunAnotherOperation();
                        break;
                    case "F":
                        //TODO: might have to split this function up to get it working in the khAutomations dll
                        //RegisterTV();

                        //RunAnotherOperation();
                        break;
                    case "Y" :
                        Console.Write(hallAutomations.displaySettings());

                        RunAnotherOperation();
                        break;
                    default:
                        Console.WriteLine("Error! Unhandled operation selected.");

                        System.Environment.Exit(1);

                        break;
                }

                TimeSpan ts = DateTime.Now - StartTime;
                Console.WriteLine("Operation completed in: " + ts.Hours + " Hours " + ts.Minutes + " Minutes " + ts.Seconds + " Seconds");
            }

            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private static void turnOnSystem()
        {
            try 
            {
                //Console.WriteLine("Powering on TV...");
                Console.Write(hallAutomations.WakeupTV());

                switch (hallAutomations.checkProjectorPowerStatus())
                {
                    case "OFF":
                        Console.WriteLine("Lowering projector lift...");
                        hallAutomations.extendProjectorLift();
                        Console.WriteLine("Powering on projector...");

                        Console.Write(hallAutomations.turnOnProjector());

                        break;

                    case "ON":
                        break;

                    case "UNKNOWN":
                        goto case "OFF";
                }

                Console.WriteLine("Waiting for projector to power on and warmup...");

                //Wait until projector is fully powered on
                while (hallAutomations.checkProjectorPowerStatus() != "ON")
                {
                    System.Threading.Thread.Sleep(750); // pause for 3/4 second;
                    Console.WriteLine("Current Projector Status: " + hallAutomations.getProjectorStatus());
                }

                Console.WriteLine("Pausing for 5 seconds...");
                System.Threading.Thread.Sleep(5000);
                Console.WriteLine(hallAutomations.changeProjectorToHDMI());
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        private static void turnOffSystem()
        {
            switch (hallAutomations.checkProjectorPowerStatus())
            {
                case "OFF":
                    break;

                case "ON":
                    Console.WriteLine(hallAutomations.turnOffProjector());
                    break;

                case "UNKNOWN":
                    goto case "ON";
            }

            Console.WriteLine("Waiting for projector to power off and cooldown...");

            //Wait until projector is fully powered off
            while (hallAutomations.checkProjectorPowerStatus() != "UNKNOWN")
            {
                System.Threading.Thread.Sleep(750); // pause for 1/4 second;
                Console.WriteLine("Current Projector Status: " + hallAutomations.getProjectorStatus());
            }

            //Retract projector lift
            Console.WriteLine("Retracting projector lift...");
            hallAutomations.retractProjectorLift();
            Console.WriteLine("Completed - Retracting projector lift...");

            //TODO Figure out how to power off TVs
            Console.WriteLine("Powering off TV...");
            //Power off TV Here!
        }

        //TODO: Place this function in the dll
        //private static void RegisterTV()
        //{
        //    CookieContainer allcookies = new CookieContainer();

        //    string hostname = System.Environment.MachineName;
        //    string jsontosend = "{\"id\":13,\"method\":\"actRegister\",\"version\":\"1.0\",\"params\":[{\"clientid\":\"" + hostname + ":34c43339-af3d-40e7-b1b2-743331375368c\",\"nickname\":\"" + hostname + " (Mendel's APP)\"},[{\"clientid\":\"" + hostname + ":34c43339-af3d-40e7-b1b2-743331375368c\",\"value\":\"yes\",\"nickname\":\"" + hostname + " (Mendel's APP)\",\"function\":\"WOL\"}]]}";

        //    try
        //    {
        //        var httpWebRequest = (HttpWebRequest)WebRequest.Create(" http://" + Settings.tv1IPAddress + "/sony/accessControl");
        //        httpWebRequest.ContentType = "application/json";
        //        httpWebRequest.Method = "POST";
        //        httpWebRequest.AllowAutoRedirect = true;
        //        httpWebRequest.Timeout = 500;

        //        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
        //        {
        //            streamWriter.Write(jsontosend);
        //        }

        //        try
        //        {
        //            httpWebRequest.GetResponse();
        //        }

        //        catch { }
        //    }

        //    catch { Console.WriteLine("device not reachable!"); }

        //    Console.Write("Please enter PIN that is displayed on TV: \n");
        //    string pincode = Console.ReadLine();

        //    Console.WriteLine("");
        //    Console.WriteLine("");
        //    Console.WriteLine("Continuing...");
        //    Console.WriteLine("");
        //    Console.WriteLine("");

        //    try
        //    {
        //        var httpWebRequest2 = (HttpWebRequest)WebRequest.Create(" http://" + Settings.tv1IPAddress + "/sony/accessControl");
        //        httpWebRequest2.ContentType = "application/json";
        //        httpWebRequest2.Method = "POST";
        //        httpWebRequest2.AllowAutoRedirect = true;
        //        httpWebRequest2.CookieContainer = allcookies;
        //        httpWebRequest2.Timeout = 500;

        //        using (var streamWriter = new StreamWriter(httpWebRequest2.GetRequestStream()))
        //        {
        //            streamWriter.Write(jsontosend);
        //        }

        //        string authInfo = "" + ":" + pincode;
        //        authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));
        //        httpWebRequest2.Headers["Authorization"] = "Basic " + authInfo;

        //        var httpResponse = (HttpWebResponse)httpWebRequest2.GetResponse();


        //        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
        //        {
        //            var responseText = streamReader.ReadToEnd();
        //            Console.WriteLine("Response Text: " + responseText);
        //        }

        //        //write register cookie to file!
        //        string answerCookie = JsonConvert.SerializeObject(httpWebRequest2.CookieContainer.GetCookies(new Uri("http://" + Settings.tv1IPAddress + "/sony/appControl")));

        //        // Write the string to a file.
        //        System.IO.StreamWriter file = new System.IO.StreamWriter("cookie.json");
        //        file.WriteLine(answerCookie);
        //        file.Close();

        //        Console.WriteLine("Cookie: " + answerCookie);

        //    }

        //    catch { Console.WriteLine("timeout!"); }
        //}
    }
}
