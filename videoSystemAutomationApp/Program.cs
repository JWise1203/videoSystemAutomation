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

namespace videoSystemAutomationApp
{
    class Program
    {
        static DateTime StartTime;

        private static Automation hallAutomations = null;

        static void Main(string[] args)
        {
            StartTime = DateTime.Now;

            Console.SetWindowSize(120, 50);
            Console.Clear();

            //Set the LogLevel To All For Testing Purposes - We can back it down later
            hallAutomations = new Automation(false, logLevel.All);
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
            OperationStringList.Add("H", "Send Command");
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
                        for (int i = 1; i <= hallAutomations.NumberOfTelevisions; i++ )
                        {
                            if (i != 1)
                            {
                                Console.Write("Attempting to wake up tv " + i);
                                Console.Write(hallAutomations.WakeupTV(i));
                            }
                        }
                            

                        RunAnotherOperation();
                        break;
                    case "E":
                        for (int i = 1; i <= hallAutomations.NumberOfTelevisions; i++)
                        {
                            if (i != 1)
                            {
                                Console.Write("Attempting to power off tv " + i);
                                Console.Write(hallAutomations.PowerOffTV(i));
                            }
                        }
                        RunAnotherOperation();
                        break;
                    case "F":
                        RegisterTV();
                        
                        RunAnotherOperation();
                        break;
                    case "H":
                        List<string> l_strAvailTVs3 = hallAutomations.getAvailableDeviceInfo(DeviceTypes.Television);

                        if (l_strAvailTVs3.Count > 0)
                        {
                            foreach (string l_strTVInfo in l_strAvailTVs3)
                            {
                                string[] l_astrTVInfo = l_strTVInfo.Split('|');
                                Console.WriteLine("Enter {0} for {1}", l_astrTVInfo[0], l_astrTVInfo[1]);
                            }
                            Console.WriteLine("");
                            int iTVID = Int32.Parse(Console.ReadLine());
                            Console.WriteLine("Please Paste Command");
                            string strCommand = Console.ReadLine();
                            StringBuilder l_objProgress = new StringBuilder();

                            var l_objStatus = hallAutomations.SendCommandByValue(strCommand, iTVID,l_objProgress);
                            Console.Write(l_objProgress.ToString());
                        }
                        else Console.WriteLine("Registration Cancelled - There are currently no Televisions setup in the configuration file.");

                        RunAnotherOperation();


                         for (int i = 1; i <= hallAutomations.NumberOfTelevisions; i++)
                        {
                            if (i == 1)
                            {
                                Console.Write("Attempting to power off tv " + i);
                                Console.Write(hallAutomations.PowerOffTV(i));
                            }
                        }
                        RunAnotherOperation();
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

        private static void RegisterTV()
        {
            List<string> l_strAvailTVs = hallAutomations.getAvailableDeviceInfo(DeviceTypes.Television);

            //What Television do we need to register?
            if (l_strAvailTVs.Count > 0)
            {
                foreach (string l_strTVInfo in l_strAvailTVs)
                {
                    string[] l_astrTVInfo = l_strTVInfo.Split('|');
                    Console.WriteLine("Enter {0} for {1}", l_astrTVInfo[0], l_astrTVInfo[1]);
                }
                Console.WriteLine("");
                int iTVID = Int32.Parse(Console.ReadLine());


                StringBuilder l_objProgress = new StringBuilder();

                Console.WriteLine("Performing Registration....");
                Console.WriteLine("Before continuing, you may need to set the device to Registration Mode,");
                Console.WriteLine("Confirm Registration or enter the Registration PIN code.");
                Console.WriteLine("Go to the device and perfrom any step, or be ready to before ehitting enter below!");
                Console.WriteLine("=====================================");
                Console.WriteLine("Hit any key to continue");
                Console.ReadKey();

                var l_blnPinRequired = true;
                var l_objStatus = hallAutomations.registerTV_Part1(ref l_blnPinRequired ,iTVID, l_objProgress);
                Console.Write(l_objProgress.ToString());


                if (l_objStatus == actionStatus.Success && l_blnPinRequired )
                {
                    l_objProgress.Clear();
                    Console.WriteLine("Please Enter the PIN Code Displayed on the TV... OR Enter 'S' to skip this step.");
                    var l_strPIN = Console.ReadLine();
                    // Send PIN code to TV to create Autorization cookie
                    if (!l_strPIN.Equals("S", StringComparison.OrdinalIgnoreCase) && l_strPIN.Trim().Length > 0)
                    {
                        Console.WriteLine("Sending Authentication PIN Code. ");

                        l_objStatus = hallAutomations.registerTV_Part2(l_strPIN, iTVID, l_objProgress);
                        Console.Write(l_objProgress.ToString());
                    }
                    else
                    {
                        Console.Write("Part 2 of registration process skipped.");
                    }
                }
            }
            else Console.WriteLine("Registration Cancelled - There are currently no Televisions setup in the configuration file.");
        }

        private static void turnOnSystem()
        {
            //TODO: NEED TO CREATE A MESSAGEBUS OR SOMETHING TO GIVE THE USER REALTIME UPDATES

            StringBuilder l_objProgress = new StringBuilder();
            actionStatus l_objStatus = actionStatus.None;

            //Currently there are multiple TV's, but only one projector and lift.
            l_objStatus = hallAutomations.turnSystemOn(p_intTVID : 99, p_objProgress : l_objProgress);
            Console.Write(l_objProgress.ToString());

            switch (l_objStatus)
            {
                case actionStatus.Success:
                    Console.WriteLine("The System has been turned on successfully.");
                    break;
                default:
                    Console.WriteLine("Something went wrong. Please check the logs for more details.\nIf this issue persists, please contact the system administrator");
                    break;
            }   
        }


        private static void turnOffSystem()
        {
            StringBuilder l_objProgress = new StringBuilder();
            actionStatus l_objStatus = actionStatus.None;
            l_objStatus = hallAutomations.turnSystemOff(p_intTVID: 99, p_objProgress: l_objProgress);
            Console.Write(l_objProgress.ToString());

            switch (l_objStatus)
            {
                case actionStatus.Success:
                    Console.WriteLine("The System has been successfully turned off.");
                    break;
                default:
                    Console.WriteLine("Something went wrong. Please check the logs for more details.\nIf this issue persists, please contact the system administrator");
                    break;
            }
        }
    }
}
