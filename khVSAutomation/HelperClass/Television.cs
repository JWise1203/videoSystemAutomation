using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Windows;
using Newtonsoft.Json;
using SonyAPILib;

namespace khVSAutomation
{

    class Television
    {
        enum televisionDataType
        {
            Cookie = 1,
            Command = 2
        }
        public string TelevisionName { get; set; }
        public string TelevisionIPAddress { get; set; }
        public string TelevisionMACAddress { get; set; }
        public bool TelevisionRegistered { get; set; }
        public List<SonyCommands> TelevisionCommands { get; set; }


        private static AutomationsEntities myDB;
        //private static tblTelevision m_objtv;
        private static Logger m_objLogger;

        private static string m_strHostName = "";

        private static string m_strJSONToSend;
        private static string m_strCookieData = "";
        private static string m_strCommandList = "";
        //private static CookieContainer m_objCookieContainer;
        private static CookieContainer allcookies = new CookieContainer();
        private SonyCommandList dataSet = new SonyCommandList();

        //private static bool isSony;
        //private static SonyAPI_Lib m_SonyAPI = null;
        //private static SonyAPI_Lib.SonyDevice mySonyDevice = null;

        
        //public Television(string p_strName, string p_strIP, string p_strMAC, string p_strCurrentSession, string p_strCookieData, string p_strCommandList, logLevel p_objLogLevel = logLevel.ErrorOnly, bool isSony = false)
        public Television(tblTelevision p_objtv, string p_strCurrentSession, logLevel p_objLogLevel = logLevel.ErrorOnly, bool isSony = false)
        {
            //m_objtv = p_objtv;
            TelevisionRegistered = false;
            TelevisionName = p_objtv.Name;
            TelevisionIPAddress = p_objtv.IPAddress;
            TelevisionMACAddress = p_objtv.MACAddress;
            m_strCookieData = string.IsNullOrWhiteSpace(p_objtv.CookieData) ? "" : p_objtv.CookieData;
            m_strCommandList = string.IsNullOrWhiteSpace(p_objtv.CommandList) ? "" : p_objtv.CommandList;

            ////TODO: Complete - We need to modify the original devs code to find the device via IP/MAC
            ////Create a new instance, but do not use it until we need it.
            //if (isSony)
            //{
            //    m_SonyAPI = new SonyAPI_Lib();
            //    m_SonyAPI.LOG.enableLogging = true;
            //    m_SonyAPI.LOG.enableLogginglev = (p_objLogLevel == logLevel.ErrorOnly || p_objLogLevel == logLevel.None) ? "Basic" : "All"; 
            //    // Default is set to C:\ProgramData\Sony - TODO: Need to Make this to the DB
            //    m_SonyAPI.LOG.loggingPath = null;
            //    m_SonyAPI.LOG.loggingName = "SonyAPILib_LOG_" + p_strName + "_" + p_strCurrentSession + ".txt";
            //    m_SonyAPI.LOG.clearLog(null); //clears the old one if it is the same exact name - This should not be the case for us since we use a new session id each time.
            //    //mySonyDevice = new SonyAPI_Lib.SonyDevice();

            //    //                mySonyDevice.buildFromDocument()
            //}
            m_strHostName = System.Environment.MachineName; //System.Windows.Forms.SystemInformation.ComputerName + "(SonyAPILib)";
            allcookies = null;
            m_strJSONToSend = "";

            myDB = new AutomationsEntities();
            m_objLogger = new Logger(ref myDB, p_strCurrentSession,  p_objLogLevel, "Television()");

            //Check this televisions Registration
            checkReg();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tvID">integer representation of the television to register (1,2,3,4,5.....)</param>
        /// <param name="p_strJSONToSend">Send Empty String, this will be assigned during this function. The Value assigned to this variable is needed for part2</param>
        /// <param name="p_objCookieContainer">Send empty object. Will be assigned during this function. The Value assigned to this variable is needed for part2</param>
        /// <param name="p_blnContinuePart2">flag to indicate whether part 2 can be started</param>
        /// <returns></returns>
        public actionStatus registerTV_Step1(ref bool p_blnPinRequired)
        {
            var l_objStatus = actionStatus.None;
            var l_strFunctionName = "registerTV_Step1()";
            p_blnPinRequired = true;
            try
            {
                m_objLogger.logToMemory("Attempting to Register the TV", l_objStatus);
                allcookies = new CookieContainer();

                m_strJSONToSend = "{\"id\":13,\"method\":\"actRegister\",\"version\":\"1.0\",\"params\":[{\"clientid\":\"" + m_strHostName + ":34c43339-af3d-40e7-b1b2-743331375368c\",\"nickname\":\"" + m_strHostName + " (Mendel's APP)\"},[{\"clientid\":\"" + m_strHostName + ":34c43339-af3d-40e7-b1b2-743331375368c\",\"value\":\"yes\",\"nickname\":\"" + m_strHostName + " (Mendel's APP)\",\"function\":\"WOL\"}]]}";

                var l_objHttpWebRequest = (HttpWebRequest)WebRequest.Create(" http://" + TelevisionIPAddress + "/sony/accessControl");
                l_objHttpWebRequest.ContentType = "application/json";
                l_objHttpWebRequest.Method = "POST";
                l_objHttpWebRequest.AllowAutoRedirect = true;
                l_objHttpWebRequest.Timeout = 500;

                using (var l_objStreamWriter = new StreamWriter(l_objHttpWebRequest.GetRequestStream()))
                {
                    l_objStreamWriter.Write(m_strJSONToSend);
                }

                try 
                {
                    var l_objHttpResponse = (HttpWebResponse)l_objHttpWebRequest.GetResponse();
                    using (var streamReader = new StreamReader(l_objHttpResponse.GetResponseStream()))
                    {
                        var responseText = streamReader.ReadToEnd();
                        m_objLogger.logToMemory("Registration response: " + responseText);
                    }
                    m_strCookieData = JsonConvert.SerializeObject(l_objHttpWebRequest.CookieContainer.GetCookies(new Uri("http://" + TelevisionIPAddress + "/sony/appControl")));
                    updateTelevisionData(televisionDataType.Cookie, m_strCookieData);
                    TelevisionRegistered = true;
                    p_blnPinRequired = false;

                    //Since we do not need step 2 let's refresh the command list now
                    get_remote_command_list(true);
                }
                catch 
                {
                    m_objLogger.logToMemory("Gen3 Pin Code Required: Must input PIN");
                }
                l_objStatus = actionStatus.Success;
                //m_objLogger.logToMemory("Please enter PIN that is displayed on TV: ", l_objStatus);
            }
            catch (Exception e)
            {
                l_objStatus = actionStatus.Error;
                m_objLogger.logToDB("Error Registering the TV: device not reachable! :" + e.ToString(), l_objStatus, true, p_strFunctionName: l_strFunctionName);
            }
            finally { m_objLogger.writePendingToDB(l_objStatus, p_strFunctionName: l_strFunctionName); }

            return l_objStatus;
        }

        public actionStatus registerTV_Step2(string p_strPIN)
        {
            var l_objStatus = actionStatus.None;
            var l_strFunctionName = "registerTV_Step2()";
            try
            {
                m_objLogger.logToMemory("Attempting to Register the TV - Part 2", l_objStatus);
                m_objLogger.logToMemory("Pin Received: " + p_strPIN + "\n\n", l_objStatus);

                var httpWebRequest2 = (HttpWebRequest)WebRequest.Create(" http://" + TelevisionIPAddress + "/sony/accessControl");
                httpWebRequest2.ContentType = "application/json";
                httpWebRequest2.Method = "POST";
                httpWebRequest2.AllowAutoRedirect = true;
                httpWebRequest2.CookieContainer = allcookies;
                httpWebRequest2.Timeout = 500;

                m_strJSONToSend = "{\"id\":13,\"method\":\"actRegister\",\"version\":\"1.0\",\"params\":[{\"clientid\":\"" + m_strHostName + ":34c43339-af3d-40e7-b1b2-743331375368c\",\"nickname\":\"" + m_strHostName + " (Mendel's APP)\"},[{\"clientid\":\"" + m_strHostName + ":34c43339-af3d-40e7-b1b2-743331375368c\",\"value\":\"yes\",\"nickname\":\"" + m_strHostName + " (Mendel's APP)\",\"function\":\"WOL\"}]]}";
                using (var streamWriter = new StreamWriter(httpWebRequest2.GetRequestStream()))
                {
                    streamWriter.Write(m_strJSONToSend);
                }

                string authInfo = "" + ":" + p_strPIN;
                authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));
                httpWebRequest2.Headers["Authorization"] = "Basic " + authInfo;
                var httpResponse = (HttpWebResponse)httpWebRequest2.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var responseText = streamReader.ReadToEnd();
                    m_objLogger.logToMemory("Response Text: " + responseText, l_objStatus);
                }

                //write register cookie to file!
                m_strCookieData = JsonConvert.SerializeObject(httpWebRequest2.CookieContainer.GetCookies(new Uri("http://" + TelevisionIPAddress + "/sony/appControl")));
                updateTelevisionData(televisionDataType.Cookie, m_strCookieData);
                TelevisionRegistered = true;
                m_objLogger.logToMemory("Cookie: " + m_strCookieData, l_objStatus);

                //Since we are all ready to go - let's update the command list now
                get_remote_command_list(true);
            }
            catch (Exception e)
            {
                TelevisionRegistered = false;
                l_objStatus = actionStatus.Error;
                m_objLogger.logToDB("Error Registering the TV: Timeout!: " + e.ToString(), l_objStatus, true, p_strFunctionName: l_strFunctionName);
            }
            finally { m_objLogger.writePendingToDB(l_objStatus, p_strFunctionName: l_strFunctionName); }

            return l_objStatus;
        }

        public actionStatus RegisterTV1Step()
        {
            var l_objStatus = actionStatus.None;
            string jsontosend = "{\"id\":13,\"method\":\"actRegister\",\"version\":\"1.0\",\"params\":[{\"clientid\":\"" + m_strHostName + ":34c43339-af3d-40e7-b1b2-743331375368c\",\"nickname\":\"" + m_strHostName + "\"},[{\"clientid\":\"" + m_strHostName + ":34c43339-af3d-40e7-b1b2-743331375368c\",\"value\":\"yes\",\"nickname\":\"" + m_strHostName + "\",\"function\":\"WOL\"}]]}";
            try
            {
                var httpWebRequest2 = (HttpWebRequest)WebRequest.Create(@"http://" + TelevisionIPAddress + @"/sony/accessControl");
                httpWebRequest2.ContentType = "application/json";
                httpWebRequest2.Method = "POST";
                httpWebRequest2.AllowAutoRedirect = true;
                CookieContainer allcookies = new CookieContainer();
                httpWebRequest2.CookieContainer = allcookies;
                httpWebRequest2.Timeout = 10000;
                using (var streamWriter = new StreamWriter(httpWebRequest2.GetRequestStream()))
                {
                    streamWriter.Write(jsontosend);
                }
                string pincode = "0904";
                string authInfo = "" + ":" + pincode;
                authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));
                httpWebRequest2.Headers["Authorization"] = "Basic " + authInfo;
                var httpResponse = (HttpWebResponse)httpWebRequest2.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var responseText = streamReader.ReadToEnd();
                    m_objLogger.logToMemory("Sent Request to register and received the following response " + responseText);
                }
                //write register cookie to file!
                m_strCookieData = JsonConvert.SerializeObject(httpWebRequest2.CookieContainer.GetCookies(new Uri("http://" + TelevisionIPAddress + "/sony/appControl")));
                updateTelevisionData(televisionDataType.Cookie, m_strCookieData);
                TelevisionRegistered = true;
                m_objLogger.logToMemory("Cookie: " + m_strCookieData, l_objStatus);

                //Since we are all ready to go - let's update the command list now
                get_remote_command_list(true);
                l_objStatus = actionStatus.Success;
            }
            catch (Exception e)
            {
                l_objStatus = actionStatus.Error;
                m_objLogger.logToMemory("Registration Error " + e.ToString(), actionStatus.Error);
            }

            return l_objStatus;
        }

        public actionStatus PowerOffTV()
        {
            var l_objStatus = actionStatus.None;
            var l_strFunctionName = "PowerOffTV";

            try
            {
                //Power = AAAAAQAAAAEAAAAVAw==
                //PowerOn = AAAAAQAAAAEAAAAuAw==
                //PowerOff = AAAAAQAAAAEAAAAvAw==
                m_objLogger.logToMemory("Attempting to Power Off the TV", l_objStatus);

                string m_strHostName = System.Environment.MachineName;

                m_objLogger.logToMemory("Configuring the HTTP Request", l_objStatus);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + TelevisionIPAddress + "/sony/IRCC");

                string xmlString = "<?xml version=\"1.0\"?>";
                xmlString += "<s:Envelope xmlns:s=\"http://schemas.xmlsoap.org/soap/envelope/\" s:encodingStyle=\"http://schemas.xmlsoap.org/soap/encoding/\">";
                xmlString += "<s:Body>";
                xmlString += "<u:X_SendIRCC xmlns:u=\"urn:schemas-sony-com:service:IRCC:1\">";
                xmlString += "<IRCCCode>AAAAAQAAAAEAAAAvAw==</IRCCCode>";
                xmlString += "</u:X_SendIRCC>";
                xmlString += "</s:Body>";
                xmlString += "</s:Envelope>";

                ASCIIEncoding l_objEncoding = new ASCIIEncoding();

                byte[] l_abteBytesToWrite = l_objEncoding.GetBytes(xmlString);
                request.KeepAlive = true;
                request.Method = "POST";
                request.ContentLength = l_abteBytesToWrite.Length;
                request.ContentType = "text/xml; charset=utf-8";

                request.Host = TelevisionIPAddress;
                request.UserAgent = "Dalvik/1.6.0 (Linux; u; Android 4.0.3; EVO Build/IML74K)";

                request.CookieContainer = new CookieContainer();
                //request.CookieContainer.Add(new Uri(@"http://" + TelevisionIPAddress + "/sony/"), new Cookie("auth", "39c0b727b3ee1607e13662cdbf5ff3d0f63b6deb5229bffcc950fe0a63f94716"));
                //39c0b727b3ee1607e13662cdbf5ff3d0f63b6deb5229bffcc950fe0a63f94716
                request.CookieContainer.Add(new Uri(@"http://" + TelevisionIPAddress + "/sony/"), new Cookie("auth", "8e1f754e72e830234593652bfbf85f756fbd47b929d95cddd8a08160bf7364c6"));

                request.Headers.Add("SOAPAction: \"urn:schemas-sony-com:service:IRCC:1#X_SendIRCC\"");
                request.Headers.Add("Accept-Encoding", "gzip");
                //request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";

                //Changed HERE - TODO: Need to Streamline and put cookie in the DB
                System.IO.Stream os = request.GetRequestStream();
                // Post data and close connection
                os.Write(l_abteBytesToWrite, 0, l_abteBytesToWrite.Length);
                System.Net.HttpWebResponse resp = request.GetResponse() as HttpWebResponse;
                Stream respData = resp.GetResponseStream();
                StreamReader sr = new StreamReader(respData);
                string response = sr.ReadToEnd();
                os.Close();
                sr.Close();
                respData.Close();
                if (response != "")
                {
                    l_objStatus = actionStatus.Success;
                    m_objLogger.logToMemory("Request Sent Successful. Received the Following Response: " + response, l_objStatus);
                }
                else
                {
                    l_objStatus = actionStatus.PartialError;
                    m_objLogger.logToMemory("Request was sent, but no response was received - Marking a parial Error. TV " + TelevisionName + " May not have turned off. Please turn off manually.", l_objStatus);
                }
                //DONE HERE


                //Stream l_objRequestStream = request.GetRequestStream();
                //l_objRequestStream.Write(l_abteBytesToWrite, 0, l_abteBytesToWrite.Length);
                //l_objRequestStream.Close();

                //m_objLogger.logToMemory("Sending the Request", l_objStatus);
                //HttpWebResponse l_objResponse = (HttpWebResponse)request.GetResponse();
                //Stream l_objDataStream = l_objResponse.GetResponseStream();
                //StreamReader l_objStreamReader = new StreamReader(l_objDataStream);

                //m_objLogger.logToMemory("Checking the Response", l_objStatus);
                //string l_strResponse = l_objStreamReader.ReadToEnd();
                //m_objLogger.logToMemory("Response from Server: " + l_strResponse, l_objStatus);
            }
            catch (Exception e)
            {
                l_objStatus = actionStatus.Error;
                m_objLogger.logToDB("Error Powering Off the TV: " + e.ToString(), l_objStatus, true, p_strFunctionName: l_strFunctionName);
            }
            finally { m_objLogger.writePendingToDB(l_objStatus, p_strFunctionName: l_strFunctionName); }

            return l_objStatus;
        }
        public string getCommandNames()
        {
            var l_strCommands = "";
            //TODO: Write Logic to Dynamically retrieve the available commands for this TV by Name
            return l_strCommands;
        }

        public actionStatus SendCommand(string p_strCommandName)
        {
            var l_objStatus = actionStatus.None;
            var l_strFunctionName = "SendCommand";

            try
            {
                var l_strCommand = "";

                //TODO: Write Logic to match up the requested command with the action string.

                if (l_strCommand != "") l_objStatus = TestCommands(l_strCommand);
                else {
                    l_objStatus = actionStatus.Error;
                    m_objLogger.logToMemory("Command Name: " + p_strCommandName + " Not Found.", l_objStatus); 
                }
            }
            catch (Exception e)
            {
                l_objStatus = actionStatus.Error;
                m_objLogger.logToDB("Error Testing Command: " + e.ToString(), l_objStatus, true, p_strFunctionName: l_strFunctionName);
            }
            finally { m_objLogger.writePendingToDB(l_objStatus, p_strFunctionName: l_strFunctionName); }

            return l_objStatus;
        }

        public actionStatus TestCommands(string p_strCommand)
        {
            var l_objStatus = actionStatus.None;
            var l_strFunctionName = "TestCommands";

            try
            {
                m_objLogger.logToMemory("Attempting to " + p_strCommand, l_objStatus);

                m_objLogger.logToMemory("Configuring the HTTP Request", l_objStatus);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + TelevisionIPAddress + "/sony/IRCC");

                string xmlString = "<?xml version=\"1.0\"?>";
                xmlString += "<s:Envelope xmlns:s=\"http://schemas.xmlsoap.org/soap/envelope/\" s:encodingStyle=\"http://schemas.xmlsoap.org/soap/encoding/\">";
                xmlString += "<s:Body>";
                xmlString += "<u:X_SendIRCC xmlns:u=\"urn:schemas-sony-com:service:IRCC:1\">";
                xmlString += "<IRCCCode>" + p_strCommand + "</IRCCCode>";
                xmlString += "</u:X_SendIRCC>";
                xmlString += "</s:Body>";
                xmlString += "</s:Envelope>";

                ASCIIEncoding l_objEncoding = new ASCIIEncoding();

                byte[] l_abteBytesToWrite = l_objEncoding.GetBytes(xmlString);
                request.KeepAlive = true;
                request.Method = "POST";
                request.ContentLength = l_abteBytesToWrite.Length;
                request.ContentType = "text/xml; charset=utf-8";

                request.Host = TelevisionIPAddress;
                request.UserAgent = "Dalvik/1.6.0 (Linux; u; Android 4.0.3; EVO Build/IML74K)";

                request.CookieContainer = new CookieContainer();
                //request.CookieContainer.Add(new Uri(@"http://" + TelevisionIPAddress + "/sony/"), new Cookie("auth", "39c0b727b3ee1607e13662cdbf5ff3d0f63b6deb5229bffcc950fe0a63f94716"));
                request.CookieContainer.Add(new Uri(@"http://" + TelevisionIPAddress + "/sony/"), new Cookie("auth", "8e1f754e72e830234593652bfbf85f756fbd47b929d95cddd8a08160bf7364c6"));
                //

                request.Headers.Add("SOAPAction: \"urn:schemas-sony-com:service:IRCC:1#X_SendIRCC\"");
                request.Headers.Add("Accept-Encoding", "gzip");
                //request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";

                //Changed HERE - TODO: Need to Streamline and put cookie in the DB
                System.IO.Stream os = request.GetRequestStream();
                // Post data and close connection
                os.Write(l_abteBytesToWrite, 0, l_abteBytesToWrite.Length);
                System.Net.HttpWebResponse resp = request.GetResponse() as HttpWebResponse;
                Stream respData = resp.GetResponseStream();
                StreamReader sr = new StreamReader(respData);
                string response = sr.ReadToEnd();
                os.Close();
                sr.Close();
                respData.Close();
                if (response != "")
                {
                    l_objStatus = actionStatus.Success;
                    m_objLogger.logToMemory("Request Sent Successful. Received the Following Response: " + response, l_objStatus);
                }
                else
                {
                    l_objStatus = actionStatus.PartialError;
                    m_objLogger.logToMemory("Request was sent, but no response was received - Marking a parial Error. TV " + TelevisionName + " May not have turned off. Please turn off manually.", l_objStatus);
                }
            }
            catch (Exception e)
            {
                l_objStatus = actionStatus.Error;
                m_objLogger.logToDB("Error Testing Command: " + e.ToString(), l_objStatus, true, p_strFunctionName: l_strFunctionName);
            }
            finally { m_objLogger.writePendingToDB(l_objStatus, p_strFunctionName: l_strFunctionName); }

            return l_objStatus;
        }

        public actionStatus WakeupTV()
        {
            var l_objStatus = actionStatus.None;
            var l_strFunctionName = "WakeupTV";

            try
            {
                m_objLogger.logToMemory("Attempting to Wakeup the TV", l_objStatus);

                Byte[] l_abteDatagram = new byte[102];

                for (int i = 0; i <= 5; i++)
                    l_abteDatagram[i] = 0xff;

                m_objLogger.logToMemory("Checking the MAC Address of the TV", l_objStatus);
                string[] l_astrMacDigits = null;
                if (TelevisionMACAddress.Contains("-"))
                    l_astrMacDigits = TelevisionMACAddress.Split('-');
                else
                    l_astrMacDigits = TelevisionMACAddress.Split(':');

                if (l_astrMacDigits.Length != 6)
                    throw new ArgumentException("Incorrect MAC address supplied!");

                int l_intStart = 6;
                for (int i = 0; i < 16; i++)
                    for (int x = 0; x < 6; x++)
                        l_abteDatagram[l_intStart + i * 6 + x] = (byte)Convert.ToInt32(l_astrMacDigits[x], 16);

                m_objLogger.logToMemory("Sending a Wakeup call to the TV", l_objStatus);
                UdpClient l_objUDPClient = new UdpClient();
                l_objUDPClient.Send(l_abteDatagram, l_abteDatagram.Length, "255.255.255.255", 3);
                m_objLogger.logToMemory("TV Wakeup Sent Successfully", l_objStatus);
                l_objStatus = actionStatus.Success;

                try
                {
                    checkReg();
                }
                catch
                { 
                    //DOING NOTHING FOR NOW - THIS MAY BE TOO EARLY FOR THIS KIND OF OPERATION
                    m_objLogger.logToMemory("There was an error trying to check the Registration after waking the TV - May need to try elsewhere.");
                }
                
            }
            catch (Exception e)
            {
                l_objStatus = actionStatus.Error;
                m_objLogger.logToMemory("WakeupTV: Error Waking the TV: " + e.ToString(), l_objStatus, true, p_strFunctionName: l_strFunctionName);
            }
            finally { m_objLogger.writePendingToDB(l_objStatus, p_strFunctionName: l_strFunctionName); }

            return l_objStatus;
        }

        public string GetDeviceInfo(bool l_blnBasic = false)
        {
            StringBuilder l_objDeviceInfo = new StringBuilder();
            l_objDeviceInfo.AppendLine("  Name:        " + TelevisionName);

            if (!l_blnBasic)
            {
                l_objDeviceInfo.AppendLine("  IP Address:  " + TelevisionIPAddress);
                l_objDeviceInfo.AppendLine("  MAC Address: " + TelevisionMACAddress);
                l_objDeviceInfo.AppendLine("  Registered:" + TelevisionRegistered);
            }

            return l_objDeviceInfo.ToString();
        }

        public bool isDeviceConfigured()
        {
            return ((TelevisionIPAddress.Trim().Length > 0) && (TelevisionMACAddress.Trim().Length > 0));
        }

        private void checkReg()
        {
            var l_strFunctionName = "checkReg";
            var l_objStatus = actionStatus.None;
            TelevisionRegistered = false;

            try
            {
                if (m_strCommandList == "" || m_strCookieData == "")
                {
                    //We Need to Register the TVs the hard way
                }
                else
                {
                    //Let's check the registration status
                        List<SonyCookie> bal = JsonConvert.DeserializeObject<List<SonyCookie>>(m_strCookieData);
                        DateTime CT = DateTime.Now;
                        //_Log.writetolog(this.Name + ": Checking if Cookie has Expired: " + bal[0].Expires, false);
                        //_Log.writetolog(this.Name + ": Cookie Expiration Date: " + bal[0].Expires, false);
                        //_Log.writetolog(this.Name + ": Current Date and Time : " + CT, false);
                        if (CT > Convert.ToDateTime(bal[0].Expires))
                        {
                            //_Log.writetolog(this.Name + ": Cookie is Expired!", true);
                            //_Log.writetolog(this.Name + ": Retriving NEW Cookie!", true);
                            string jsontosend = "{\"id\":13,\"method\":\"actRegister\",\"version\":\"1.0\",\"params\":[{\"clientid\":\"" + m_strHostName + ":34c43339-af3d-40e7-b1b2-743331375368c\",\"nickname\":\"" + m_strHostName + "\"},[{\"clientid\":\"" + m_strHostName + ":34c43339-af3d-40e7-b1b2-743331375368c\",\"value\":\"yes\",\"nickname\":\"" + m_strHostName + "\",\"function\":\"WOL\"}]]}";
                            try
                            {
                                var httpWebRequest2 = (HttpWebRequest)WebRequest.Create(@"http://" + TelevisionIPAddress + @"/sony/accessControl");
                                httpWebRequest2.ContentType = "application/json";
                                httpWebRequest2.Method = "POST";
                                httpWebRequest2.AllowAutoRedirect = true;
                                httpWebRequest2.CookieContainer = allcookies;
                                httpWebRequest2.Timeout = 500;
                                using (var streamWriter = new StreamWriter(httpWebRequest2.GetRequestStream()))
                                {
                                    streamWriter.Write(jsontosend);
                                }
                                var httpResponse = (HttpWebResponse)httpWebRequest2.GetResponse();
                                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                                {
                                    var responseText = streamReader.ReadToEnd();
                                    //_Log.writetolog("Registration response: " + responseText, false);
                                }

                                m_strCookieData = JsonConvert.SerializeObject(httpWebRequest2.CookieContainer.GetCookies(new Uri("http://" + TelevisionIPAddress + "/sony/appControl")));
                                updateTelevisionData(televisionDataType.Cookie, m_strCookieData);
                                bal = JsonConvert.DeserializeObject<List<SonyCookie>>(m_strCookieData);
                                allcookies.Add(new Uri(@"http://" + TelevisionIPAddress + bal[0].Path), new Cookie(bal[0].Name, bal[0].Value));
                                TelevisionRegistered = true;
                                //_Log.writetolog(this.Name + ": New Cookie auth=" + this.Cookie, false);
                            }
                            catch
                            {
                                //_Log.writetolog(this.Name + ": Failed to retrieve new Cookie", false);
                                TelevisionRegistered = false;
                                //results = false;
                            }
                        }
                        else
                        {
                            m_objLogger.logToMemory("The Stored Cookie is still Good for the " + TelevisionName + " Television");
                            //_Log.writetolog(bal[0].Name + ": Adding Cookie to Device: " + bal[0].Value, false);
                            allcookies.Add(new Uri(@"http://" + TelevisionIPAddress + bal[0].Path), new Cookie(bal[0].Name, bal[0].Value));
                            TelevisionRegistered = true;
                            //_Log.writetolog(this.Name + ": Cookie Found: auth=" + this.Cookie, false);
                        }
                }

                //Load the commands that we already have available
                if (TelevisionRegistered) loadRemoteCommandList(m_strCommandList);
            }
            catch
            { 
                TelevisionRegistered = false; 
            }
            finally { m_objLogger.writePendingToDB(l_objStatus, p_strFunctionName: l_strFunctionName); }

        }

        private bool updateTelevisionData(televisionDataType p_objDataType, string p_strNewData = "")
        {
            var l_blnSuccess = false;
            try
            {
                tblTelevision t = myDB.tblTelevisions
                                      .First(i => i.IPAddress == TelevisionIPAddress);

                var l_objNewData = p_strNewData == "" ? m_strCookieData : p_strNewData;
                if(p_objDataType == televisionDataType.Cookie)
                {
                    t.CookieData = l_objNewData;
                }
                else if(p_objDataType == televisionDataType.Command)
                {
                    t.CommandList = l_objNewData;
                }
                else { return false; }
                myDB.SaveChanges(); //Save Syncronously for now
                l_blnSuccess = true;
            }
            catch (Exception e)
            {
                //We want to log it at this point, but let's continue since we will still have what we need in memory
                m_objLogger.logToMemory("Issue Saving  Cookie to DB: " + e.ToString(), actionStatus.Error);
                l_blnSuccess = false;
            }
            return l_blnSuccess;
        }

        #region get remote command list
        /// <summary>
        /// This method will retrieve Gen1 and Gen2 XML IRCC Command List or Gen3 JSON Command List.
        /// </summary>
        /// <returns>Returns a string containing the contents of the returned XML Command List for your Use</returns>
        /// <remarks>This method will also populate the SonyDevice.Commands object list with the retrieved command list</remarks>
        public string get_remote_command_list(bool p_blnSaveCommandListToDB = false)
        {
            //TODO: Logging & Try Catch
            string cmdList = "";
            //_Log.writetolog(this.Name + " is Retrieving Generation:" + this.Actionlist.RegisterMode + " Remote Command List", false);
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(@"http://" + TelevisionIPAddress + @"/sony/system");
            httpWebRequest.ContentType = "text/json";
            httpWebRequest.Method = "POST";
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = "{\"id\":20,\"method\":\"getRemoteControllerInfo\",\"version\":\"1.0\",\"params\":[]}";
                streamWriter.Write(json);
            }
            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var responseText = streamReader.ReadToEnd();
                m_strCommandList = responseText;
                if (cmdList != "")
                {
                    //_Log.writetolog("Response Returned: " + cmdList, true);
                    //_Log.writetolog("Retrieve Command List was Successful", true);
                }
                else
                {
                    //_Log.writetolog("Retrieve Command List was NOT successful", true);
                }
                dataSet = JsonConvert.DeserializeObject<SonyCommandList>(responseText);
            }
            string first = dataSet.result[1].ToString();
            List<SonyCommands> bal = JsonConvert.DeserializeObject<List<SonyCommands>>(first);
            TelevisionCommands = bal;
            //_Log.writetolog(this.Name + " Commands have been Populated: " + this.Commands.Count().ToString(), true);

            if (p_blnSaveCommandListToDB && m_strCommandList.Length > 0) updateTelevisionData(televisionDataType.Command, m_strCommandList);

            return m_strCommandList;
        }

        private actionStatus loadRemoteCommandList(string p_strCommandList)
        {
            var l_objStatus = actionStatus.None;

            try
            {
                if (p_strCommandList.Length > 0)
                {
                    dataSet = JsonConvert.DeserializeObject<SonyCommandList>(p_strCommandList);
                    string first = dataSet.result[1].ToString();
                    List<SonyCommands> bal = JsonConvert.DeserializeObject<List<SonyCommands>>(first);
                    TelevisionCommands = bal;
                }
                else
                { 
                    //log - no commands to load
                    //Try getting the commands now......
                    get_remote_command_list(true);
                }
            }
            catch
            { 
                l_objStatus = actionStatus.Error; 
            }
            finally
            { }

            return l_objStatus;
        }

        #endregion
        /*
        #region Logging
        private static void writeProgress(StringBuilder p_objMyProgress, string p_strMessage, bool p_blnNewLine = true)
        {
            if (m_blnLogProgress)
            {
                if (p_objMyProgress == null) p_objMyProgress = new StringBuilder();

                if (p_blnNewLine) p_objMyProgress.AppendLine(p_strMessage);
                else p_objMyProgress.Append(p_strMessage);
            }
        }
        #endregion
        */

    }


    #region Sony Cookie
    /// <summary>
    /// Gets or Sets the Sony Device Cookie Container Object
    /// </summary>
    [Serializable]
    class SonyCookie
    {
        /// <summary>
        /// Gets or Sets the Cookie Comment
        /// </summary>
        public string Comment { get; set; }
        /// <summary>
        /// Gets or Sets the Cookie Comment URI
        /// </summary>
        public object CommentUri { get; set; }
        /// <summary>
        /// Gets or Sets Cookie for HTTP Only
        /// </summary>
        public bool HttpOnly { get; set; }
        /// <summary>
        /// gets or Sets the Cookie Discard
        /// </summary>
        public bool Discard { get; set; }
        /// <summary>
        /// gets or Sets the Cookie Domain
        /// </summary>
        public string Domain { get; set; }
        /// <summary>
        /// Gets or Sets the Cookie Expired
        /// </summary>
        public bool Expired { get; set; }
        /// <summary>
        /// Gets or Sets the Cookies Expiration
        /// </summary>
        public string Expires { get; set; }
        /// <summary>
        /// Gets or Sets the Cookie Name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Gets or Sets the Cookie Path
        /// </summary>
        public string Path { get; set; }
        /// <summary>
        /// Gets or Sets the Cookie Port
        /// </summary>
        public string Port { get; set; }
        /// <summary>
        /// Gets or Sets the Is Cookie Secure
        /// </summary>
        public bool Secure { get; set; }
        /// <summary>
        /// Gets or Sets the Cookie Time Stamp
        /// </summary>
        public string TimeStamp { get; set; }
        /// <summary>
        /// Gets or Sets the Cookie Value
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// Gets or Sets the Cookie Version
        /// </summary>
        public int Version { get; set; }
    }
    #endregion

    #region Sony Command List
    /// <summary>
    /// Gets or Sets the Sony Command List Object
    /// </summary>
    [Serializable]
    class SonyCommandList
    {
        /// <summary>
        /// Gets or Sets the Devices Command List ID
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// Gets or Sets the Devices Command List Results
        /// </summary>
        public List<object> result { get; set; }
    }
    #endregion

    #region Sony Command
    /// <summary>
    /// Gets or Sets the Sony Device Command
    /// </summary>
    [Serializable]
    public class SonyCommands
    {
        /// <summary>
        /// Gets or Sets the Devices Command Name
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// Gets or Sets the Devices Command Value
        /// </summary>
        public string value { get; set; }
    }
    #endregion

}
