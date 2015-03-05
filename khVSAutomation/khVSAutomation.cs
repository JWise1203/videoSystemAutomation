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

using rv;

namespace khVSAutomation
{
    static class Settings
    {
        public static string tv1IPAddress { get { return khVSAutomation.Properties.Settings.Default.TV1IP; } }
        public static string tv1MACAddress { get { return khVSAutomation.Properties.Settings.Default.TV1MAC; } }
        public static string tv2IPAddress { get { return khVSAutomation.Properties.Settings.Default.TV2IP; } }
        public static string tv2MACAddress { get { return khVSAutomation.Properties.Settings.Default.TV2MAC; } }
        public static string projectorIPAddress { get { return khVSAutomation.Properties.Settings.Default.PROJIP; } }
        public static string liftCOMPort { get { return khVSAutomation.Properties.Settings.Default.LIFTCOM; } }
        public static int liftMovementTime { get { return khVSAutomation.Properties.Settings.Default.LIFTMOVETIME; } }
    }

    public class Automation
    {
        //static DateTime StartTime;

        private static PJLinkConnection c = null;

        public static string displaySettings()
        {
            StringBuilder myOutput = new StringBuilder();

            myOutput.AppendLine("==============CURRENT APPLICATION SETTINGS==============");
            myOutput.AppendLine("tv1IPAddress:       " + Settings.tv1IPAddress);
            myOutput.AppendLine("tv1MACAddress:      " + Settings.tv1MACAddress);
            myOutput.AppendLine("tv2IPAddress:       " + Settings.tv2IPAddress);
            myOutput.AppendLine("tv2MACAddress:      " + Settings.tv2MACAddress);
            myOutput.AppendLine("projectorIPAddress: " + Settings.projectorIPAddress);
            myOutput.AppendLine("liftCOMPort:        " + Settings.liftCOMPort);
            myOutput.AppendLine("liftMovementTime:   " + Settings.liftMovementTime);
            myOutput.AppendLine("==============END OF APPLICATION SETTINGS===============");

            return myOutput.ToString();
        }

        private static void sendSerialData(string COMPort, int baudRate, System.IO.Ports.Parity paritySetting, int dataBits, System.IO.Ports.StopBits stopBits, string dataToSend)
        {
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


        /// <summary>
        /// cancels the Projector Lift Movement
        /// </summary>
        /// <returns></returns>
        public static string cancelProjectorLiftMove()
        {
            StringBuilder myprogress = new StringBuilder();

            try
            {
                myprogress.AppendLine("Attempting to Cancel the Current Life Action");
                sendSerialData(Settings.liftCOMPort, 9600, System.IO.Ports.Parity.None, 8, System.IO.Ports.StopBits.One, ">0500037D\r");
                myprogress.AppendLine("Life Action Cancelled");
            }
            catch (Exception e)
            {
                throw new Exception("cancelProjectorLiftMove: Error Cancelling Lift Action: " + e.ToString());
            }
            return myprogress.ToString();
        }

        public async static void extendProjectorLift()
        {
            try
            {
                sendSerialData(Settings.liftCOMPort, 9600, System.IO.Ports.Parity.None, 8, System.IO.Ports.StopBits.One, ">0500107B\r");
                await Task.Delay(Settings.liftMovementTime);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async static void retractProjectorLift()
        {
            try
            {
                sendSerialData(Settings.liftCOMPort, 9600, System.IO.Ports.Parity.None, 8, System.IO.Ports.StopBits.One, ">0500127D\r");
                await Task.Delay(Settings.liftMovementTime);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        
        //TODO: Figrue out how to register the TV without Extra User Interaction OOORRRR Break the function apart
        //private static string RegisterTV()
        //{
        //    StringBuilder myprogress = new StringBuilder();

        //    try
        //    {
        //        myprogress.AppendLine("Attempting to Register the TV");

        //        CookieContainer allcookies = new CookieContainer();

        //        string hostname = System.Environment.MachineName;
        //        string jsontosend = "{\"id\":13,\"method\":\"actRegister\",\"version\":\"1.0\",\"params\":[{\"clientid\":\"" + hostname + ":34c43339-af3d-40e7-b1b2-743331375368c\",\"nickname\":\"" + hostname + " (Mendel's APP)\"},[{\"clientid\":\"" + hostname + ":34c43339-af3d-40e7-b1b2-743331375368c\",\"value\":\"yes\",\"nickname\":\"" + hostname + " (Mendel's APP)\",\"function\":\"WOL\"}]]}";

        //        try
        //        {
        //            var httpWebRequest = (HttpWebRequest)WebRequest.Create(" http://" + Settings.tv1IPAddress + "/sony/accessControl");
        //            httpWebRequest.ContentType = "application/json";
        //            httpWebRequest.Method = "POST";
        //            httpWebRequest.AllowAutoRedirect = true;
        //            httpWebRequest.Timeout = 500;

        //            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
        //            {
        //                streamWriter.Write(jsontosend);
        //            }

        //            try
        //            {
        //                httpWebRequest.GetResponse();
        //            }

        //            catch { }
        //        }

        //        catch { Console.WriteLine("device not reachable!"); }

        //        Console.Write("Please enter PIN that is displayed on TV: \n");
        //        string pincode = Console.ReadLine();

        //        Console.WriteLine("");
        //        Console.WriteLine("");
        //        Console.WriteLine("Continuing...");
        //        Console.WriteLine("");
        //        Console.WriteLine("");

        //        try
        //        {
        //            var httpWebRequest2 = (HttpWebRequest)WebRequest.Create(" http://" + Settings.tv1IPAddress + "/sony/accessControl");
        //            httpWebRequest2.ContentType = "application/json";
        //            httpWebRequest2.Method = "POST";
        //            httpWebRequest2.AllowAutoRedirect = true;
        //            httpWebRequest2.CookieContainer = allcookies;
        //            httpWebRequest2.Timeout = 500;

        //            using (var streamWriter = new StreamWriter(httpWebRequest2.GetRequestStream()))
        //            {
        //                streamWriter.Write(jsontosend);
        //            }

        //            string authInfo = "" + ":" + pincode;
        //            authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));
        //            httpWebRequest2.Headers["Authorization"] = "Basic " + authInfo;

        //            var httpResponse = (HttpWebResponse)httpWebRequest2.GetResponse();


        //            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
        //            {
        //                var responseText = streamReader.ReadToEnd();
        //                Console.WriteLine("Response Text: " + responseText);
        //            }

        //            //write register cookie to file!
        //            string answerCookie = JsonConvert.SerializeObject(httpWebRequest2.CookieContainer.GetCookies(new Uri("http://" + Settings.tv1IPAddress + "/sony/appControl")));

        //            // Write the string to a file.
        //            System.IO.StreamWriter file = new System.IO.StreamWriter("cookie.json");
        //            file.WriteLine(answerCookie);
        //            file.Close();

        //            Console.WriteLine("Cookie: " + answerCookie);

        //        }

        //        catch { Console.WriteLine("timeout!"); }

        //    }
        //    catch (Exception e)
        //    {
        //        throw new Exception("RegisterTV: Error Registering the TV: " + e.ToString());
        //    }
        //    return myprogress.ToString();
        //}

        public static string PowerOffTV()
        {
            StringBuilder myprogress = new StringBuilder();

            try
            {
                myprogress.AppendLine("Attempting to Power Off the TV");

                string hostname = System.Environment.MachineName;

                myprogress.AppendLine("Configuring the HTTP Request");
                HttpWebRequest request = (HttpWebRequest)
                HttpWebRequest.Create("http://" + Settings.tv1IPAddress + "/sony/IRCC");

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

                myprogress.AppendLine("Sending the Request");
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);

                myprogress.AppendLine("Checking the Response");
                string responseFromServer = reader.ReadToEnd();
                myprogress.AppendLine("Response from Server: " + responseFromServer);
            }
            catch (Exception e)
            {
                throw new Exception("PowerOffTV: Error Powering Off the TV: " + e.ToString());
            }

            return myprogress.ToString();
        }

        public static string WakeupTV()
        {
            StringBuilder myprogress = new StringBuilder();

            try
            {
                myprogress.AppendLine("Attempting to Wakeup the TV");

                Byte[] datagram = new byte[102];

                for (int i = 0; i <= 5; i++)
                {
                    datagram[i] = 0xff;
                }

                myprogress.AppendLine("Checking the MAC Address of the TV");
                string[] macDigits = null;
                if (Settings.tv1MACAddress.Contains("-"))
                {
                    macDigits = Settings.tv1MACAddress.Split('-');
                }
                else
                {
                    macDigits = Settings.tv1MACAddress.Split(':');
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

                myprogress.AppendLine("Sending a Wakeup call to the TV");
                UdpClient client = new UdpClient();
                client.Send(datagram, datagram.Length, "255.255.255.255", 3);
                myprogress.AppendLine("TV Wakeup Sent Successfully");
            }
            catch (Exception e)
            {
                throw new Exception("WakeupTV: Error Waking the TV: " + e.ToString());
            }

            return myprogress.ToString();
        }

        private static string connectToProjector()
        {
            StringBuilder myProgress = new StringBuilder();
            try
            {
                myProgress.Append("Is there a connection to the projector? ");
                if (c == null)
                {
                    myProgress.AppendLine("N");
                    myProgress.AppendLine("Attempting to Connect to the projector");

                    System.Net.IPAddress address = null;

                    myProgress.AppendLine("Checking the IP Address of the Projector");
                    if (System.Net.IPAddress.TryParse(Settings.projectorIPAddress, out address))
                    {
                        myProgress.AppendLine("Attempting to Connect to the projector");
                        c = new PJLinkConnection(Settings.projectorIPAddress, "JBMIAProjectorLink");
                        myProgress.AppendLine("A Connection has been established!");
                    }
                    else
                    {
                        myProgress.AppendLine("Invalid IP Address Entered");
                    }
                }
                else
                {
                    myProgress.AppendLine("Y");
                }
            }
            catch (Exception e)
            {
                throw new Exception("ConnectToProjector: Error Connecting to OR getting the status of the projector: " + e.ToString());
            }
            return myProgress.ToString(); 
        }

        public static string getProjectorStatus()
        {
            if (c != null)
                return c.powerQuery().ToString();
            else
                return "No Status Available";
        }

        public static string printProjectorStatus()
        {
            StringBuilder myProgress = new StringBuilder();

            try
            {
                myProgress.AppendLine("Attempting to Print the Projector Status");

                LampStatusCommand lamp = new LampStatusCommand();
                string status = lamp.getStatusOfLamp(1).ToString();

                myProgress.Append(connectToProjector());

                string power = c.powerQuery().ToString();
                ProjectorInfo pi = new ProjectorInfo();


                myProgress.AppendLine("Connection Status: Connected");
                myProgress.AppendLine("");
                myProgress.AppendLine("Power Status: " + power);
                myProgress.AppendLine("Fan Status: " + pi.FanStatus);
                myProgress.AppendLine("Lamp Status: " + pi.LampStatus);
                myProgress.AppendLine("Current Source Input: " + pi.Input);
                myProgress.AppendLine("Cover Status: " + pi.CoverStatus);
                myProgress.AppendLine("Filter Status: " + pi.FilterStatus);
                myProgress.AppendLine("Current Lamp Status: " + status);
                myProgress.AppendLine("");
            }
            catch (Exception e)
            {
                throw new Exception("printProjectorStatus: Error Printing the Projector Status: " + e.ToString()); 
            }

            return myProgress.ToString();
        }

        public static string turnOnProjector()
        {
            StringBuilder myProgress = new StringBuilder();
            myProgress.AppendLine("Attempting to turn the projector On");

            try
            {
                myProgress.Append(connectToProjector());

                //TODO:create a message bus so that these messages can be placed back in the functions
                myProgress.AppendLine("Turning Projector On");
                c.turnOn();
                myProgress.AppendLine("Projector is now:" + c.powerQuery().ToString());
            }
            catch (Exception e)
            {
                throw new Exception("turnOnProjector: Error Turning the Projector On: " + e.ToString());
            }
            
            return myProgress.ToString();
        }

        public static string turnOffProjector()
        {
            StringBuilder myProgress = new StringBuilder();
            myProgress.AppendLine("Attempting to turn the Projector Off......");
            try
            {
                myProgress.Append(connectToProjector());
                
                //TODO:create a message bus so that these messages can be displayed real time
                myProgress.AppendLine("Turning Projector Off");
                c.turnOff();
                myProgress.AppendLine("Projector is now:" + c.powerQuery().ToString());
            }
            catch (Exception e)
            {
                throw new Exception("turnOffProjector: Error Turning off the Projector: " + e.ToString());
            }

            return myProgress.ToString();
        }

        public static string change_Input(rv.InputCommand.InputType x, int i, string inputName)
        {
            StringBuilder myProgress = new StringBuilder();

            try
            {
                myProgress.AppendLine("Attempting to Change the Projector Input to " + inputName);

                InputCommand ic2 = new InputCommand(x, i);

                if (c.sendCommand(ic2) == Command.Response.SUCCESS)
                    myProgress.AppendLine(ic2.dumpToString());
                else
                    myProgress.AppendLine("Communication Error");
            }
            catch (Exception e)
            {
                throw new Exception("change_Input: Error Changing the Projector Input to " + inputName + ": " + e.ToString());
            }
            return myProgress.ToString();
        }

        public static string changeProjectorToHDMI()
        {
            return change_Input(InputCommand.InputType.DIGITAL, 1, "HDMI");
        }

        public static string changeProjectorToVGA()
        {
            return change_Input(InputCommand.InputType.RGB, 1, "VGA");
        }

        public static string checkProjectorPowerStatus()
        {
            //StringBuilder myProgress = new StringBuilder();
            //try
            //{
            //    myProgress.AppendLine("Attempting to Check the Projector Power Status");
            //    myProgress.Append(connectToProjector());
            //    myProgress.AppendLine("");
            //    myProgress.AppendLine("Current Projector Power Status:");
            //    myProgress.AppendLine(c.powerQuery().ToString());
            //    myProgress.AppendLine("");
            //}
            //catch (Exception e)
            //{
            //    throw new Exception("checkProjectorPowerStatus: Error Checking the projector power status: " + e.ToString());
            //}
            
            //return myProgress.ToString();
            connectToProjector();
            return c.powerQuery().ToString();
        }
    }
}
