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

        private static AutomationsEntities myDB;
        private static Logger m_objLogger;
        public MatrixSwitcher(string p_strName, string p_strCOM, string p_strCurrentSession, logLevel p_objLogLevel = logLevel.ErrorOnly)
        {
            SwitcherName = p_strName;
            SwitcherCOMPort = p_strCOM;

            myDB = new AutomationsEntities();
            m_objLogger = new Logger(ref myDB, p_strCurrentSession, p_objLogLevel, "MatrixSwitcher()");
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
