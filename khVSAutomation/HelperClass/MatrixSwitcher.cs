using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;


namespace khVSAutomation
{
    class MatrixSwitcher
    {
        public string SwitcherCOMPort { get; set; }
        public string SwitcherName { get; set; }

        public string SwitcherIPAddress { get; set; }

        private static List<tblMatrixSwitcherCommand> m_lstAvailableCommands = null;
        private bool m_blnViaHTTP;
        //private static bool m_blnLogProgress;

        //Left Commands
        private static readonly byte[] Output1Left = { 0x09, 0xBA, 0xBA, 0xBF, 0xC6, 0x04, 0x00, 0x01, 0x00, 0x64 };
        private static readonly byte[] Output2Left = { 0x09, 0xBA, 0xBA, 0xBF, 0xC6, 0x04, 0x00, 0x02, 0x00, 0x31 };
        private static readonly byte[] Output3Left = { 0x09, 0xBA, 0xBA, 0xBF, 0xC6, 0x04, 0x00, 0x03, 0x00, 0xF5 };
        private static readonly byte[] Output4Left = { 0x09, 0xBA, 0XBA, 0xBF, 0xC6, 0x04, 0X00, 0X04, 0x00, 0x9B };
        //Right Commands
        private static readonly byte[] Output1Right = { 0x09, 0xBA, 0xBA, 0xBF, 0xC6, 0x05, 0x00, 0x01, 0x00, 0xEB };
        private static readonly byte[] Output2Right = { 0x09, 0xBA, 0xBA, 0xBF, 0xC6, 0x05, 0x00, 0x02, 0x00, 0xBE };
        private static readonly byte[] Output3Right = { 0x09, 0xBA, 0xBA, 0xBF, 0xC6, 0x05, 0x00, 0x03, 0x00, 0x7A };
        private static readonly byte[] Output4Right = { 0x09, 0xBA, 0xBA, 0xBF, 0xC6, 0x05, 0x00, 0x04, 0x00, 0x14 };
        //Source 1
        private static readonly byte[] Output1Source1 = { 0x09, 0xBA, 0xBA, 0xBF, 0xC6, 0x03, 0x00, 0x01, 0x01, 0xBC };
        private static readonly byte[] Output2Source1 = { 0x09, 0xBA, 0xBA, 0xBF, 0xC6, 0x03, 0x00, 0x02, 0x01, 0xE9 };
        private static readonly byte[] Output3Source1 = { 0x09, 0xBA, 0xBA, 0xBF, 0xC6, 0x03, 0x00, 0x03, 0x01, 0x2D };
        private static readonly byte[] Output4Source1 = { 0x09, 0xBA, 0xBA, 0xBF, 0xC6, 0x03, 0x00, 0x04, 0x01, 0x43 };
        //Source 2
        private static readonly byte[] Output1Source2 = { 0x09, 0xBA, 0xBA, 0xBF, 0xC6, 0x03, 0x00, 0x01, 0x02, 0x5E };
        private static readonly byte[] Output2Source2 = { 0x09, 0xBA, 0xBA, 0xBF, 0xC6, 0x03, 0x00, 0x02, 0x02, 0x0B };
        private static readonly byte[] Output3Source2 = { 0x09, 0xBA, 0xBA, 0xBF, 0xC6, 0x03, 0x00, 0x03, 0x02, 0xCF };
        private static readonly byte[] Output4Source2 = { 0x09, 0xBA, 0xBA, 0xBF, 0xC6, 0x03, 0x00, 0x04, 0x02, 0xA1 };
        //Source 3
        private static readonly byte[] Output1Source3 = { 0x09, 0xBA, 0xBA, 0xBF, 0xC6, 0x03, 0x00, 0x01, 0x03, 0x00 };
        private static readonly byte[] Output2Source3 = { 0x09, 0xBA, 0xBA, 0xBF, 0xC6, 0x03, 0x00, 0x02, 0x03, 0x55 };
        private static readonly byte[] Output3Source3 = { 0x09, 0xBA, 0xBA, 0xBF, 0xC6, 0x03, 0x00, 0x03, 0x03, 0x91 };
        private static readonly byte[] Output4Source3 = { 0x09, 0xBA, 0xBA, 0xBF, 0xC6, 0x03, 0x00, 0x04, 0x03, 0xFF };
        //Source 4
        private static readonly byte[] Output1Source4 = { 0x09, 0xBA, 0xBA, 0xBF, 0xC6, 0x03, 0x00, 0x01, 0x04, 0x83 };
        private static readonly byte[] Output2Source4 = { 0x09, 0xBA, 0xBA, 0xBF, 0xC6, 0x03, 0x00, 0x02, 0x04, 0xD6 };
        private static readonly byte[] Output3Source4 = { 0x09, 0xBA, 0xBA, 0xBF, 0xC6, 0x03, 0x00, 0x03, 0x04, 0x12 };
        private static readonly byte[] Output4Source4 = { 0x09, 0xBA, 0xBA, 0xBF, 0xC6, 0x03, 0x00, 0x04, 0x04, 0x7C };

        private AutomationsEntities myDB;
        private Logger m_objLogger;
        public MatrixSwitcher(string p_strName, string p_strCOM, string p_strIPAddress,string p_strCurrentSession, logLevel p_objLogLevel = logLevel.ErrorOnly)
        {
            SwitcherName = p_strName;
            SwitcherCOMPort = p_strCOM;
            SwitcherIPAddress = p_strIPAddress;

            myDB = new AutomationsEntities();
            m_objLogger = new Logger(ref myDB, p_strCurrentSession, p_objLogLevel, "MatrixSwitcher()");
            m_blnViaHTTP = true; //Serial Commands were not tested as of yet. Due to limitations, get requests will be used to control through the switchers admin UI
            m_lstAvailableCommands = new List<tblMatrixSwitcherCommand>();
            if (m_blnViaHTTP)
            {
                //fill the list
                try
                {
                    m_objLogger.logToMemory(string.Format("{0}: {1}: Generating the list of available commands via.", "New MatrixSwitcher()", SwitcherName));
                    m_lstAvailableCommands = (from commands in myDB.tblMatrixSwitcherCommands
                                              orderby commands.CommandID
                                              select commands).ToList();
                    m_objLogger.logToMemory(string.Format("{0}: Matrix Switcher Command Count: {1}", "New MatrixSwitcher()", m_lstAvailableCommands.Count()));
                }
                catch { }
                finally { m_objLogger.writePendingToDB(p_strFunctionName: "New MatrixSwitcher()"); }
            }
        }

        private void sendSerialData(string p_strCOMPort, int p_intBaudRate, System.IO.Ports.Parity p_objParitySetting, int p_intDataBits, System.IO.Ports.StopBits p_objStopBits, byte[] p_bteDataToSend)
        {
            System.IO.Ports.SerialPort l_objSwitcherSerialPort = new System.IO.Ports.SerialPort(p_strCOMPort, p_intBaudRate, p_objParitySetting, p_intDataBits, p_objStopBits);

            try
            {
                //Since the manual had a hex message, I tried to follow an example found here: https://web.archive.org/web/20130709121945/http://msmvps.com/blogs/coad/archive/2005/03/23/SerialPort-_2800_RS_2D00_232-Serial-COM-Port_2900_-in-C_2300_-.NET.aspx
                //If this does not work, try sending the text values instead (like the projector lift)
                l_objSwitcherSerialPort.Open();
                l_objSwitcherSerialPort.Write(p_bteDataToSend, 0, 10);

            }
            catch (Exception e) { throw e; }
            finally { l_objSwitcherSerialPort.Close(); }
        }

        /// <summary>
        /// For Sending Direct Commands from the UI
        /// </summary>
        /// <param name="p_strCOMPort"></param>
        /// <param name="p_intBaudRate"></param>
        /// <param name="p_objParitySetting"></param>
        /// <param name="p_intDataBits"></param>
        /// <param name="p_objStopBits"></param>
        /// <param name="p_bteDataToSend"></param>
        private void sendSerialData(string p_strCOMPort, int p_intBaudRate, System.IO.Ports.Parity p_objParitySetting, int p_intDataBits, System.IO.Ports.StopBits p_objStopBits, string p_bteDataToSend)
        {
            System.IO.Ports.SerialPort l_objSwitcherSerialPort = new System.IO.Ports.SerialPort(p_strCOMPort, p_intBaudRate, p_objParitySetting, p_intDataBits, p_objStopBits);

            try
            {
                l_objSwitcherSerialPort.Open();
                l_objSwitcherSerialPort.Write(p_bteDataToSend);
            }
            catch (Exception e) { throw e; }
            finally { l_objSwitcherSerialPort.Close(); }
        }

        public List<string> getCommandNames()
        {
            List<string> l_lstrCommands = m_blnViaHTTP ? getCommandNamesHTTP() : getCommandNamesSerial();
            return l_lstrCommands;
        }
        private List<string> getCommandNamesHTTP()
        {
            List<string> l_lstrCommands = new List<string>();
            var l_strFunctionName = "getCommandNamesSerial()";
            try
            {
                m_objLogger.logToMemory(string.Format("{0}: {1}: Generating the list of available commands via.", l_strFunctionName, SwitcherName));
                //Dynamically retrieve the available commands for this TV by Name so that they can be dynamically loaded in a UI
                if (m_lstAvailableCommands != null && m_lstAvailableCommands.Count > 0)
                {
                    m_objLogger.logToMemory(string.Format("{0}: {1}: There were {2} commands available. Generating a list of available Commands.", l_strFunctionName, SwitcherName, m_lstAvailableCommands.Count));
                    foreach (var l_objCommand in m_lstAvailableCommands) { l_lstrCommands.Add(l_objCommand.DisplayValue); }
                }
                else
                    m_objLogger.logToMemory(string.Format("{0}: {1}: There were no commands available. Returning The Empty List.", l_strFunctionName, SwitcherName));
                m_objLogger.writePendingToDB(p_strFunctionName: l_strFunctionName);
            }
            catch { }
            finally { m_objLogger.writePendingToDB(p_strFunctionName: l_strFunctionName); }
            return l_lstrCommands;
        }
        private List<string> getCommandNamesSerial()
        {
            List<string> l_lstrCommands = new List<string>();
            var l_strFunctionName = "getCommandNamesSerial()";

            try
            {
                m_objLogger.logToMemory(string.Format("{0}: {1}: Generating the list of available commands via.", l_strFunctionName, SwitcherName));

                l_lstrCommands.Add("Output1Source1");
                l_lstrCommands.Add("Output2Source1");
                l_lstrCommands.Add("Output3Source1");
                l_lstrCommands.Add("Output4Source1");
                l_lstrCommands.Add("Output1Source2");
                l_lstrCommands.Add("Output2Source2");
                l_lstrCommands.Add("Output3Source2");
                l_lstrCommands.Add("Output4Source2");
                l_lstrCommands.Add("Output1Source3");
                l_lstrCommands.Add("Output2Source3");
                l_lstrCommands.Add("Output3Source3");
                l_lstrCommands.Add("Output4Source3");
                l_lstrCommands.Add("Output1Source4");
                l_lstrCommands.Add("Output2Source4");
                l_lstrCommands.Add("Output3Source4");
                l_lstrCommands.Add("Output4Source4");
                l_lstrCommands.Add("Output1Left");
                l_lstrCommands.Add("Output2Left");
                l_lstrCommands.Add("Output3Left");
                l_lstrCommands.Add("Output4Left");
                l_lstrCommands.Add("Output1Right");
                l_lstrCommands.Add("Output2Right");
                l_lstrCommands.Add("Output3Right");
                l_lstrCommands.Add("Output4Right");
            }
            catch { }
            finally { m_objLogger.writePendingToDB(p_strFunctionName: l_strFunctionName); }

            return l_lstrCommands;
        }

        /// <summary>
        /// Finds the value of the Command Name passed in and then sends the command value to the Projector to execute.
        /// </summary>
        /// <param name="p_strCommandName"></param>
        /// <returns></returns>
        public actionStatus SendCommandByName(string p_strCommandName)
        {
            var l_objStatus = m_blnViaHTTP ? SendCommandByNameHTTP(p_strCommandName) : SendCommandByNameSerial(p_strCommandName);
            return l_objStatus;
        }
        private actionStatus SendCommandByNameHTTP(string p_strCommandName)
        {
            var l_objStatus = actionStatus.None;
            var l_strFunctionName = "SendCommandByNameHTTP()";
            var l_blnCommandLocated = false;
            try
            {
                if (m_lstAvailableCommands.Count > 0)
                {
                    m_objLogger.logToMemory(string.Format("{0}: {1}: Attempting to locate {2} in the available list of Matrix commands.", l_strFunctionName, SwitcherName, p_strCommandName));
                    foreach (var l_objCommand in m_lstAvailableCommands)
                    {
                        if (l_objCommand.DisplayValue == p_strCommandName)
                        {
                            l_blnCommandLocated = true;
                            m_objLogger.logToMemory(string.Format("{0}: {1}: {2} Located. Attempting to Send {3} to the Matrix.", l_strFunctionName, SwitcherName, p_strCommandName, l_objCommand.Name));

                            string[] l_aryParameters = l_objCommand.CommandParameters.Split(';');
                            l_objStatus = SendCommandByValueHTTP(l_aryParameters[0], l_aryParameters[1], l_aryParameters[2]);

                            if (l_objStatus == actionStatus.Error)
                                throw new Exception(string.Format("Attempt to send {0} to the Matrix Failed.", l_objCommand.Name));
                            break;
                        }
                    }
                }
                else
                    throw new Exception("There were no commands available in the Matrix Command List. Operation Cancelled.");

                if (!l_blnCommandLocated)
                    throw new Exception(string.Format("{0} Could not be located. Operation Cancelled.", p_strCommandName));
            }
            catch (Exception e)
            {
                l_objStatus = actionStatus.Error;
                m_objLogger.logToMemory(string.Format("{0}: {1}: Error Sending Command By Name: {2}", l_strFunctionName, SwitcherName, e.ToString()), l_objStatus);
            }
            finally { m_objLogger.writePendingToDB(l_objStatus, p_strFunctionName: l_strFunctionName); }

            return l_objStatus;
        }
        private actionStatus SendCommandByNameSerial(string p_strCommandName)
        {
            var l_objStatus = actionStatus.None;
            var l_strFunctionName = "SendCommandByNameSerial()";
            try
            {
                //Determine if this is a special functionality command
                switch (p_strCommandName)
                {
                    case "Output1Source1":
                        l_objStatus = doSwitcherAction(SwitcherOutput.Output1, SwitcherAction.Source1);
                        break;
                    case "Output2Source1":
                        l_objStatus = doSwitcherAction(SwitcherOutput.Output2, SwitcherAction.Source1);
                        break;
                    case "Output3Source1":
                        l_objStatus = doSwitcherAction(SwitcherOutput.Output3, SwitcherAction.Source1);
                        break;
                    case "Output4Source1":
                        l_objStatus = doSwitcherAction(SwitcherOutput.Output4, SwitcherAction.Source1);
                        break;
                    case "Output1Source2":
                        l_objStatus = doSwitcherAction(SwitcherOutput.Output1, SwitcherAction.Source2);
                        break;
                    case "Output2Source2":
                        l_objStatus = doSwitcherAction(SwitcherOutput.Output2, SwitcherAction.Source2);
                        break;
                    case "Output3Source2":
                        l_objStatus = doSwitcherAction(SwitcherOutput.Output3, SwitcherAction.Source2);
                        break;
                    case "Output4Source2":
                        l_objStatus = doSwitcherAction(SwitcherOutput.Output4, SwitcherAction.Source2);
                        break;
                    case "Output1Source3":
                        l_objStatus = doSwitcherAction(SwitcherOutput.Output1, SwitcherAction.Source3);
                        break;
                    case "Output2Source3":
                        l_objStatus = doSwitcherAction(SwitcherOutput.Output2, SwitcherAction.Source3);
                        break;
                    case "Output3Source3":
                        l_objStatus = doSwitcherAction(SwitcherOutput.Output3, SwitcherAction.Source3);
                        break;
                    case "Output4Source3":
                        l_objStatus = doSwitcherAction(SwitcherOutput.Output4, SwitcherAction.Source3);
                        break;
                    case "Output1Source4":
                        l_objStatus = doSwitcherAction(SwitcherOutput.Output1, SwitcherAction.Source4);
                        break;
                    case "Output2Source4":
                        l_objStatus = doSwitcherAction(SwitcherOutput.Output2, SwitcherAction.Source4);
                        break;
                    case "Output3Source4":
                        l_objStatus = doSwitcherAction(SwitcherOutput.Output3, SwitcherAction.Source4);
                        break;
                    case "Output4Source4":
                        l_objStatus = doSwitcherAction(SwitcherOutput.Output4, SwitcherAction.Source4);
                        break;
                    case "Output1Left":
                        l_objStatus = doSwitcherAction(SwitcherOutput.Output1, SwitcherAction.Left);
                        break;
                    case "Output2Left":
                        l_objStatus = doSwitcherAction(SwitcherOutput.Output2, SwitcherAction.Left);
                        break;
                    case "Output3Left":
                        l_objStatus = doSwitcherAction(SwitcherOutput.Output3, SwitcherAction.Left);
                        break;
                    case "Output4Left":
                        l_objStatus = doSwitcherAction(SwitcherOutput.Output4, SwitcherAction.Left);
                        break;
                    case "Output1Right":
                        l_objStatus = doSwitcherAction(SwitcherOutput.Output1, SwitcherAction.Right);
                        break;
                    case "Output2Right":
                        l_objStatus = doSwitcherAction(SwitcherOutput.Output2, SwitcherAction.Right);
                        break;
                    case "Output3Right":
                        l_objStatus = doSwitcherAction(SwitcherOutput.Output3, SwitcherAction.Right);
                        break;
                    case "Output4Right":
                        l_objStatus = doSwitcherAction(SwitcherOutput.Output4, SwitcherAction.Right);
                        break;
                    default:
                        throw new Exception(string.Format("{0}: {1}: {2} Command NOT Found.", l_strFunctionName, SwitcherName, p_strCommandName));
                }
            }
            catch (Exception e)
            {
                l_objStatus = actionStatus.Error;
                m_objLogger.logToMemory(string.Format("{0}: {1}: Error Sending Command By Name: {2}", l_strFunctionName, SwitcherName, e.ToString()), l_objStatus);
            }
            finally { m_objLogger.writePendingToDB(l_objStatus, p_strFunctionName: l_strFunctionName); }

            return l_objStatus;
        }

        public actionStatus SendCommandByValue(string p_strCommand)
        {
            var l_objStatus = actionStatus.None;
            var l_strFunctionName = "SendCommandByValue()";

            try
            {
                m_objLogger.logToMemory(string.Format("{0}: {1}: Attempting to Send Message through Serial Port: {2}", l_strFunctionName, SwitcherName, p_strCommand), l_objStatus);
                sendSerialData(SwitcherCOMPort, 9600, System.IO.Ports.Parity.None, 8, System.IO.Ports.StopBits.One, p_strCommand);
                l_objStatus = actionStatus.Success;
                m_objLogger.logToMemory(string.Format("{0}: {1}: Matrix Action Message Sent: {2}", l_strFunctionName, SwitcherName, p_strCommand), l_objStatus);
            }
            catch (Exception e)
            {
                l_objStatus = actionStatus.Error;
                m_objLogger.logToMemory(string.Format("{0}: {1}: Error Sending Command to the Matrix Switcher: {2}.", l_strFunctionName, SwitcherName, e.ToString()), l_objStatus);
            }
            finally { m_objLogger.writePendingToDB(l_objStatus, p_strFunctionName: l_strFunctionName); }

            return l_objStatus;
        }

        public actionStatus SendCommandByValueHTTP(string p_strCommandType, string p_strDevice, string p_strSource)
        {
            var l_objStatus = actionStatus.None;
            var l_strFunctionName = "SendCommandByValueHTTP()";

            var l_intCheckSum = Int32.Parse(p_strCommandType) + Int32.Parse(p_strDevice) + Int32.Parse(p_strSource);
            m_objLogger.logToMemory(string.Format("{0}: {1}: Attempting to Send the command to the switcher.", l_strFunctionName, SwitcherName));
            // Create a request for the URL. 
            var l_strTestResult = "";
            try
            {

                WebRequest request = WebRequest.Create(string.Format(" http://{0}/switch.cgi?command={1}&data0={2}&data1={3}&checksum={4}", SwitcherIPAddress, p_strCommandType, p_strDevice, p_strSource, l_intCheckSum.ToString()));
                // If required by the server, set the credentials.
                request.Credentials = CredentialCache.DefaultCredentials;
                // Get the response.
                WebResponse response = request.GetResponse();
                // Display the status.
                //TODO: Add response determination conditions - Responses returned should be 'OK'
                l_strTestResult = ((HttpWebResponse)response).StatusDescription;
                response.Close();
                l_objStatus = actionStatus.Success;
                m_objLogger.logToMemory(string.Format("{0}: {1}: Complete.", l_strFunctionName, SwitcherName), l_objStatus);
            }
            catch (Exception e)
            {
                l_objStatus = actionStatus.Error;
                m_objLogger.logToMemory(string.Format("{0}: {1}: Error Sending Command to the TV: {2}.", l_strFunctionName, SwitcherName, e.ToString()), l_objStatus);
            }
            finally { m_objLogger.writePendingToDB(l_objStatus, p_strFunctionName: l_strFunctionName); }

            return l_objStatus;
        }

        public actionStatus doSwitcherAction(SwitcherOutput p_objSwitcherOutput, SwitcherAction p_objSwitcherAction)
        {
            var l_objStatus = actionStatus.None;
            var l_strFunctionName = "doSwitcherAction()"; 

            try
            {
                m_objLogger.logToMemory("Called do Switcher Action with Output set to: " + p_objSwitcherOutput + " and Switcher Action Set to: " + p_objSwitcherAction);

                byte[] l_bteCurrentAction = null;
                switch (p_objSwitcherAction)
                {
                    case SwitcherAction.Source1:
                        switch (p_objSwitcherOutput)
                        {
                            case SwitcherOutput.Output1: l_bteCurrentAction = Output1Source1; break;
                            case SwitcherOutput.Output2: l_bteCurrentAction = Output2Source1; break;
                            case SwitcherOutput.Output3: l_bteCurrentAction = Output3Source1; break;
                            case SwitcherOutput.Output4: l_bteCurrentAction = Output4Source1; break;
                        }
                        break;
                    case SwitcherAction.Source2:
                        switch (p_objSwitcherOutput)
                        {
                            case SwitcherOutput.Output1: l_bteCurrentAction = Output1Source2; break;
                            case SwitcherOutput.Output2: l_bteCurrentAction = Output2Source2; break;
                            case SwitcherOutput.Output3: l_bteCurrentAction = Output3Source2; break;
                            case SwitcherOutput.Output4: l_bteCurrentAction = Output4Source2; break;
                        }
                        break;
                    case SwitcherAction.Source3:
                        switch (p_objSwitcherOutput)
                        {
                            case SwitcherOutput.Output1: l_bteCurrentAction = Output1Source3; break;
                            case SwitcherOutput.Output2: l_bteCurrentAction = Output2Source3; break;
                            case SwitcherOutput.Output3: l_bteCurrentAction = Output3Source3; break;
                            case SwitcherOutput.Output4: l_bteCurrentAction = Output4Source3; break;
                        }
                        break;
                    case SwitcherAction.Source4:
                        switch (p_objSwitcherOutput)
                        {
                            case SwitcherOutput.Output1: l_bteCurrentAction = Output1Source4; break;
                            case SwitcherOutput.Output2: l_bteCurrentAction = Output2Source4; break;
                            case SwitcherOutput.Output3: l_bteCurrentAction = Output3Source4; break;
                            case SwitcherOutput.Output4: l_bteCurrentAction = Output4Source4; break;
                        }
                        break;
                    case SwitcherAction.Left:
                        switch (p_objSwitcherOutput)
                        {
                            case SwitcherOutput.Output1: l_bteCurrentAction = Output1Left; break;
                            case SwitcherOutput.Output2: l_bteCurrentAction = Output2Left; break;
                            case SwitcherOutput.Output3: l_bteCurrentAction = Output3Left; break;
                            case SwitcherOutput.Output4: l_bteCurrentAction = Output4Left; break;
                        }
                        break;
                    case SwitcherAction.Right:
                        switch (p_objSwitcherOutput)
                        {
                            case SwitcherOutput.Output1: l_bteCurrentAction = Output1Right; break;
                            case SwitcherOutput.Output2: l_bteCurrentAction = Output2Right; break;
                            case SwitcherOutput.Output3: l_bteCurrentAction = Output3Right; break;
                            case SwitcherOutput.Output4: l_bteCurrentAction = Output4Right; break;
                        }
                        break;
                    default:
                        throw new Exception("Switcher Action: " + p_objSwitcherAction + "Not Found!");
                }
                if (l_bteCurrentAction != null)
                {
                    m_objLogger.logToMemory("Switcher Action and Output Found: Attempting to Send Message through Serial Port " + SwitcherCOMPort);

                    sendSerialData(SwitcherCOMPort, 9600, System.IO.Ports.Parity.None, 8, System.IO.Ports.StopBits.One, l_bteCurrentAction);

                    m_objLogger.logToMemory("Switcher Action Message Sent: " + l_bteCurrentAction.ToString());

                    l_objStatus = actionStatus.Success;

                }
                else { throw new Exception("Switcher Output: " + p_objSwitcherOutput + "Not Found!"); }
            }
            catch (Exception e)
            {
                l_objStatus = actionStatus.Error;
                m_objLogger.logToDB("Error Performing Switcher Action: " + e.ToString(), l_objStatus, true, p_strFunctionName: l_strFunctionName);
                throw e;
            }
            finally { m_objLogger.writePendingToDB(l_objStatus, p_strFunctionName: l_strFunctionName); }
            return l_objStatus;
        }

        public string GetDeviceInfo(bool l_blnBasic = false)
        {
            StringBuilder l_objDeviceInfo = new StringBuilder();
            l_objDeviceInfo.AppendLine("  Name:      " + SwitcherName);
            if (!l_blnBasic) l_objDeviceInfo.AppendLine("  COM Port:  " + SwitcherCOMPort);
            return l_objDeviceInfo.ToString();
        }

        public bool isDeviceConfigured()
        {
            return (SwitcherCOMPort.Trim().Length > 0);
        }
    }
}
