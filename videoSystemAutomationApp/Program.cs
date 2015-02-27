using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections;
using System.Collections.Specialized;

using System.Net;
using System.Net.Sockets;
using System.IO;
using Newtonsoft.Json;

using rv;

namespace videoSystemAutomationApp
{
    class Program
    {
        private static readonly string tv1IPAddress = "192.168.1.6";
        private static readonly string tv2IPAddress = "192.168.1.7";
        private static readonly string projectorIPAddress = "192.168.1.5";
        private static readonly string tv1MACAddress = "FC:F1:52:A0:F0:56";
        private static readonly string liftCOMPort = "COM1";
        private static readonly int liftMovementTime = 15000;

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
            OperationStringList.Add("1", "Dev Option");
            OperationStringList.Add("Z", "Exit application");

            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("====================================================================================");
            Console.WriteLine("Welcome to the Wilson Road Kingdom Hall A/V Applicaiton");
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

        private static int DisplayAndSelectDeploymentvAppStates()
        {
            string operation = "";
            StringDictionary OperationStringList = new StringDictionary();

            Console.WriteLine("");
            Console.WriteLine("Please select the vApp state after deployment:");

            OperationStringList.Add("1", "Power On");
            OperationStringList.Add("2", "Powered Off/Shutdown (Depends on vApp Template power settings)");

            foreach (DictionaryEntry pair in OperationStringList)
            {
                Console.WriteLine("     {0}:  {1}", pair.Key, pair.Value);
            }

            Console.WriteLine("");

            while (true)
            {
                Console.Write("Please select the operation you wish to excute: ");
                operation = Console.ReadLine();

                if (OperationStringList.ContainsKey(operation))
                {
                    break;
                }

                Console.Write("Error! You must enter a valid operation integer.");
                Console.WriteLine("");
                Console.WriteLine("");
            }

            return Convert.ToInt32(operation);
        }

        private static int DisplayAndSelectvAppStates()
        {
            string operation = "";
            StringDictionary OperationStringList = new StringDictionary();

            Console.WriteLine("");
            Console.WriteLine("Please select the vApp state to apply:");

            OperationStringList.Add("1", "Power On");
            OperationStringList.Add("2", "Power Off");
            OperationStringList.Add("3", "Shutdown Operating System");
            OperationStringList.Add("4", "Suspend");
            OperationStringList.Add("5", "Reboot");
            OperationStringList.Add("6", "Do not execute a configuration state command");

            foreach (DictionaryEntry pair in OperationStringList)
            {
                Console.WriteLine("     {0}:  {1}", pair.Key, pair.Value);
            }

            Console.WriteLine("");

            while (true)
            {
                Console.Write("Please select the operation you wish to excute: ");
                operation = Console.ReadLine();

                if (OperationStringList.ContainsKey(operation))
                {
                    break;
                }

                Console.Write("Error! You must enter a valid operation integer.");
                Console.WriteLine("");
                Console.WriteLine("");
            }

            return Convert.ToInt32(operation);
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

        internal static bool ConfirmOperation(int Function)
        {
            string line;

            while (true)
            {
                Console.WriteLine("");

                switch (Function)
                {
                    case 1:
                        Console.Write("Are you sure you wish to execute this command? (Y/N): ");
                        break;
                    case 2:
                        Console.Write("Change network for each vApp in deployment collection? (Y/N): ");
                        break;
                    case 3:
                        Console.Write("Save configuration(s) state? (Y/N): ");
                        break;
                    case 4:
                        Console.Write("Deploy instructor and test student vApps? (Y/N): ");
                        break;
                    case 5:
                        Console.Write("Change power state for instructor vApp? (Y/N): ");
                        break;
                    case 6:
                        Console.Write("Deploy vApps to existing vDC? (Y/N): ");
                        break;
                    case 7:
                        Console.Write("Snapshot vApp after deployment? (Y/N): ");
                        break;
                    case 8:
                        Console.Write("Would you like to create the instructor account? (Y/N): ");
                        break;
                    case 9:
                        Console.Write("Would you like to copy the deployment report to the clipboard? (Y/N): ");
                        break;
                }

                line = Console.ReadLine();

                if (line == "Y" | line == "y" | line == "N" | line == "n")
                    break;
            }

            if (line == "Y" | line == "y")
                return true;
            else
                return false;
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
                        printProjectorStatus();

                        RunAnotherOperation();
                        break;
                    case "D":
                        WakeupTV(tv1MACAddress);

                        RunAnotherOperation();
                        break;
                    case "E":
                        PowerOffTV();

                        RunAnotherOperation();
                        break;
                    case "F":
                        RegisterTV();

                        RunAnotherOperation();
                        break;
                    case "1":
                        testSerial();

                        RunAnotherOperation();
                        break;
                    default:
                        Console.WriteLine("Error! Unhandled operation selected. Please contact Jared Wise");

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

        private static void testSerial()
        {
            sendSerialData("COM3", 9600, System.IO.Ports.Parity.None, 8, System.IO.Ports.StopBits.One, ">0500127D[]");
        }

        private static void sendSerialData(string COMPort, int baudRate, System.IO.Ports.Parity paritySetting, int dataBits, System.IO.Ports.StopBits stopBits, string dataToSend)
        {
            Console.WriteLine("Sending string:  " + dataToSend);
            System.IO.Ports.SerialPort liftSerialPort = new System.IO.Ports.SerialPort(COMPort, baudRate, paritySetting, dataBits, stopBits);
            
            try
            {
                liftSerialPort.Open();
                liftSerialPort.WriteLine(dataToSend);
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                liftSerialPort.Close();
            }
        }



        private static void cancelProjectorLiftMove()
        {
            try
            {
                sendSerialData(liftCOMPort, 9600, System.IO.Ports.Parity.None, 8, System.IO.Ports.StopBits.One, ">0500037D");
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private async static void extendProjectorLift()
        {
            try
            {
                sendSerialData(liftCOMPort, 9600, System.IO.Ports.Parity.None, 8, System.IO.Ports.StopBits.One, ">0500107B");
                await Task.Delay(liftMovementTime);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private async static void retractProjectorLift()
        {
            try
            {
                sendSerialData("COM3", 9600, System.IO.Ports.Parity.None, 8, System.IO.Ports.StopBits.One, ">0500127D");
                await Task.Delay(liftMovementTime);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private static void turnOnSystem()
        {
            switch (checkProjectorPowerStatus())
            {
                case "OFF":
                    Console.WriteLine("Lowering projector lift...");
                    extendProjectorLift();

                    Console.WriteLine("Powering on projector...");
                    turnOnProjector();

                    break;

                case "ON":
                    break;

                case "UNKNOWN":
                    goto case "OFF";
            }

            Console.WriteLine("Waiting for projector to power on and warmup...");

            //Wait until projector is fully powered on
            while (checkProjectorPowerStatus() != "ON")
            {
                System.Threading.Thread.Sleep(750); // pause for 3/4 second;
                Console.WriteLine("Current Projector Status: " + getProjectorStatus());
            }

            Console.WriteLine("Pausing for 5 seconds...");
            System.Threading.Thread.Sleep(5000);
            Console.WriteLine("Changing projector source to HDMI...");
            changeProjectorToHDMI();

            Console.WriteLine("Powering on TV...");
            WakeupTV(tv1MACAddress);
        }

        private static void turnOffSystem()
        {
            switch (checkProjectorPowerStatus())
            {
                case "OFF":
                    break;

                case "ON":
                    Console.WriteLine("Powering off projector...");
                    turnOffProjector();
                    break;

                case "UNKNOWN":
                    goto case "ON";
            }

            Console.WriteLine("Waiting for projector to power off and cooldown...");

            //Wait until projector is fully powered on
            while (checkProjectorPowerStatus() != "UNKNOWN")
            {
                System.Threading.Thread.Sleep(750); // pause for 1/4 second;
                Console.WriteLine("Current Projector Status: " + getProjectorStatus());
            }


            Console.WriteLine("Powering off TV...");
            //Power off TV Here!
        }

        private static void RegisterTV()
        {
            CookieContainer allcookies = new CookieContainer();

            string hostname = System.Environment.MachineName;
            string jsontosend = "{\"id\":13,\"method\":\"actRegister\",\"version\":\"1.0\",\"params\":[{\"clientid\":\"" + hostname + ":34c43339-af3d-40e7-b1b2-743331375368c\",\"nickname\":\"" + hostname + " (Mendel's APP)\"},[{\"clientid\":\"" + hostname + ":34c43339-af3d-40e7-b1b2-743331375368c\",\"value\":\"yes\",\"nickname\":\"" + hostname + " (Mendel's APP)\",\"function\":\"WOL\"}]]}";

            try
            {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(" http://" + tv1IPAddress + "/sony/accessControl");
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";
                httpWebRequest.AllowAutoRedirect = true;
                httpWebRequest.Timeout = 500;

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(jsontosend);
                }

                try
                {
                    httpWebRequest.GetResponse();
                }

                catch { }
            }

            catch { Console.WriteLine("device not reachable!"); }

            Console.Write("Please enter PIN that is displayed on TV: \n");
            string pincode = Console.ReadLine();

            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("Continuing...");
            Console.WriteLine("");
            Console.WriteLine("");

            try
            {
                var httpWebRequest2 = (HttpWebRequest)WebRequest.Create(" http://" + tv1IPAddress + "/sony/accessControl");
                httpWebRequest2.ContentType = "application/json";
                httpWebRequest2.Method = "POST";
                httpWebRequest2.AllowAutoRedirect = true;
                httpWebRequest2.CookieContainer = allcookies;
                httpWebRequest2.Timeout = 500;

                using (var streamWriter = new StreamWriter(httpWebRequest2.GetRequestStream()))
                {
                    streamWriter.Write(jsontosend);
                }

                string authInfo = "" + ":" + pincode;
                authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));
                httpWebRequest2.Headers["Authorization"] = "Basic " + authInfo;

                var httpResponse = (HttpWebResponse)httpWebRequest2.GetResponse();


                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var responseText = streamReader.ReadToEnd();
                    Console.WriteLine("Response Text: " + responseText);
                }

                //write register cookie to file!
                string answerCookie = JsonConvert.SerializeObject(httpWebRequest2.CookieContainer.GetCookies(new Uri("http://" + tv1IPAddress + "/sony/appControl")));

                // Write the string to a file.
                System.IO.StreamWriter file = new System.IO.StreamWriter("cookie.json");
                file.WriteLine(answerCookie);
                file.Close();

                Console.WriteLine("Cookie: " + answerCookie);

            }

            catch { Console.WriteLine("timeout!"); }
        }

        private static void PowerOffTV()
        {
            string hostname = System.Environment.MachineName;

            Console.WriteLine("Powering off TV...");

            HttpWebRequest request = (HttpWebRequest)
            HttpWebRequest.Create("http://" + tv1IPAddress + "/sony/IRCC");

            string xmlString = "<?xml version=\"1.0\"?>";
            xmlString += "<s:Envelope xmlns:s=\"http://schemas.xmlsoap.org/soap/envelope/\" s:encodingStyle=\"http://schemas.xmlsoap.org/soap/encoding/\">";
            xmlString += "<s:Body>";
            xmlString += "<u:X_SendIRCC xmlns:u=\"urn:schemas-sony-com:service:IRCC:1\">";
            xmlString += "<IRCCCode>AAAAAQAAAAEAAAAvAw==</IRCCCode>";
            xmlString += "</u:X_SendIRCC>";
            xmlString += "</s:Body>";
            xmlString += "</s:Envelope>";

            ASCIIEncoding encoding = new ASCIIEncoding();

            byte[] bytesToWrite = encoding.GetBytes(xmlString);

            request.Method = "POST";
            request.ContentLength = bytesToWrite.Length;
            request.Headers.Add("SOAPAction: \"urn:schemas-sony-com:service:IRCC:1#X_SendIRCC\"");
            request.ContentType = "text/xml; charset=utf-8";
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";

            Stream newStream = request.GetRequestStream();
            newStream.Write(bytesToWrite, 0, bytesToWrite.Length);
            newStream.Close();

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);

            string responseFromServer = reader.ReadToEnd();

            Console.WriteLine(responseFromServer);


            ////string jsontosend = "{\"id\":20,\"result\":[{\"bundled\":true,\"type\":\"RM-J1100\"},[{\"name\":\"PowerOff\",\"value\":\"AAAAAQAAAAEAAAAvAw==\"}";

            ////string jsontosend = "{\"id\":20,\"method\":\"getRemoteControllerInfo\",\"version\":\"1.0\",\"params\":[]}";
            ////string jsontosend = "{\"name\":\"PowerOff\",\"value\":\"AAAAAQAAAAEAAAAvAw==\"}";

            //string content = "<?xml version=\"1.0\"?>";
            //content += "<s:Envelope xmlns:s=\"http://schemas.xmlsoap.org/soap/envelope/\" s:encodingStyle=\"http://schemas.xmlsoap.org/soap/encoding/\">";
            //content += "<s:Body>";
            //content += "<u:X_SendIRCC xmlns:u=\"urn:schemas-sony-com:service:IRCC:1\">";
            //content += "<IRCCCode>AAAAAQAAAAEAAAAvAw==</IRCCCode>";
            //content += "</u:X_SendIRCC>";
            //content += "</s:Body>";
            //content += "</s:Envelope>";

            //try
            //{
            //    var httpWebRequest = (HttpWebRequest)WebRequest.Create(" http://" + tv1IPAddress + "/sony/IRCC");
            //    //httpWebRequest.ContentType = "application/json";
            //    httpWebRequest.ContentType = "text/xml; charset=\"utf-8\"";
            //    httpWebRequest.Method = "POST";
            //    httpWebRequest.AllowAutoRedirect = true;
            //    httpWebRequest.Timeout = 500;

            //    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            //    {
            //        streamWriter.Write(content);
            //    }

            //    try
            //    {
            //        httpWebRequest.GetResponse();

            //        var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

            //        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            //        {
            //            var responseText = streamReader.ReadToEnd();
            //            Console.WriteLine("Response Text: " + responseText);
            //        }
            //    }

            //    catch (Exception e)
            //    {
            //        Console.WriteLine(e.Message);
            //    }
            //}

            //catch { Console.WriteLine("timeout!"); }
        }

        private static string WakeupTV(string macAddress)
        {
            Byte[] datagram = new byte[102];

            for (int i = 0; i <= 5; i++)
            {
                datagram[i] = 0xff;
            }

            string[] macDigits = null;
            if (macAddress.Contains("-"))
            {
                macDigits = macAddress.Split('-');
            }
            else
            {
                macDigits = macAddress.Split(':');
            }

            if (macDigits.Length != 6)
            {
                throw new ArgumentException("Incorrect MAC address supplied!");
            }

            int start = 6;
            for (int i = 0; i < 16; i++)
            {
                for (int x = 0; x < 6; x++)
                {
                    datagram[start + i * 6 + x] = (byte)Convert.ToInt32(macDigits[x], 16);
                }
            }

            UdpClient client = new UdpClient();
            client.Send(datagram, datagram.Length, "255.255.255.255", 3);

            return "Command Sent!";
        }

        private static void connectToProjector()
        {
            System.Net.IPAddress address = null;

            if (System.Net.IPAddress.TryParse(projectorIPAddress, out address))
            {
                c = new PJLinkConnection(projectorIPAddress, "JBMIAProjectorLink");
            }

            else
            {
                Console.WriteLine("Invalid IP Address Entered");
            }
        }

        private static string getProjectorStatus()
        {
            return c.powerQuery().ToString();
        }

        private static void printProjectorStatus()
        {
            LampStatusCommand l = new LampStatusCommand();
            //int hours = l.getHoursOfLamp(1);
            string status = l.getStatusOfLamp(1).ToString();

            //Connect to projector, if not currently connected
            if(c == null)
            {
                connectToProjector();
            }

            string power = c.powerQuery().ToString();
            ProjectorInfo pi = new ProjectorInfo();

            Console.WriteLine("Connection Status: Connected");
            Console.WriteLine("");
            Console.WriteLine("Power Status: " + power);
            Console.WriteLine("Fan Status: " + pi.FanStatus);
            Console.WriteLine("Lamp Status: " + pi.LampStatus);
            Console.WriteLine("Current Source Input: " + pi.Input);
            Console.WriteLine("Cover Status: " + pi.CoverStatus);
            Console.WriteLine("Filter Status: " + pi.FilterStatus);
            Console.WriteLine("");
        }

        private static void turnOnProjector()
        {
            if (c == null)
            {
                connectToProjector();
            }

            Console.WriteLine("Turning Projector On");
            c.turnOn();
            Console.WriteLine("Projector is now:" + c.powerQuery().ToString());
        }

        private static void turnOffProjector()
        {
            if (c == null)
            {
                connectToProjector();
            }

            Console.WriteLine("Turning Projector Off");
            c.turnOff();
            Console.WriteLine("Projector is now:" + c.powerQuery().ToString());
        }

        private static void change_Input(rv.InputCommand.InputType x, int i)
        {
            InputCommand ic2 = new InputCommand(x, i);

            if (c.sendCommand(ic2) == Command.Response.SUCCESS)
                Console.WriteLine(ic2.dumpToString());
            else
                Console.WriteLine("Communication Error");

        }

        private static void changeProjectorToHDMI()
        {
            change_Input(InputCommand.InputType.DIGITAL, 1);
        }

        private static void changeProjectorToVGA()
        {
            change_Input(InputCommand.InputType.RGB, 1);
        }

        private static string checkProjectorPowerStatus()
        {
            if (c == null)
            {
                connectToProjector();
            }

            return c.powerQuery().ToString();
        }
    }
}
