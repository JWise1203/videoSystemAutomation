using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;
using Nito.AsyncEx;


namespace khVSAutomation
{
    class ProjectorLift
    {
        public string LiftName { get; set; }
        public string LiftCOMPort { get; set; }
        public int LiftMoveTime { get; set; }

        private const string m_cstrCancel = ">0500037D\r";
        private const string m_cstrExtend = ">0500107B\r";
        private const string m_cstrRetract = ">0500127D\r";

        private AutomationsEntities myDB;
        private Logger m_objLogger;

        public ProjectorLift(string p_strName, string p_strCOM, int p_strMoveTime, string p_strCurrentSession, logLevel p_objLogLevel = logLevel.ErrorOnly)
        {
            LiftName = p_strName;
            LiftCOMPort = p_strCOM;
            LiftMoveTime = p_strMoveTime;
            myDB = new AutomationsEntities();
            m_objLogger = new Logger(ref myDB, p_strCurrentSession, p_objLogLevel, "ProjectorLift()");
        }

        private void sendSerialData(string p_strCOMPort, int p_intBaudRate, System.IO.Ports.Parity p_objParitySetting, int p_intDataBits, System.IO.Ports.StopBits p_objStopBits, string p_strDataToSend)
        {
            System.IO.Ports.SerialPort l_objLiftSerialPort = new System.IO.Ports.SerialPort(p_strCOMPort, p_intBaudRate, p_objParitySetting, p_intDataBits, p_objStopBits);

            try
            {
                l_objLiftSerialPort.Open();
                l_objLiftSerialPort.WriteLine(p_strDataToSend);
            }
            catch (Exception e) { throw e; }
            finally { l_objLiftSerialPort.Close(); }
        }

        public List<string> getCommandNames()
        {
            var l_strFunctionName = "getCommandNames()";
            List<string> l_lstrCommands = new List<string>();
            //TODO: Add Safer Logic so that a list is not provided if the projector is not available.
            l_lstrCommands.Add("Open");
            l_lstrCommands.Add("Cancel");
            l_lstrCommands.Add("Close");
            return l_lstrCommands;
        }

        public actionStatus SendCommandByName(string p_strCommandName)
        {
            var l_objStatus = actionStatus.None;
            var l_strFunctionName = "SendCommandByName()";
            var l_strCallingFunctionName = "";
            LiftAction l_objLiftAction;
            try
            {
                //Determine if this is aspecial functionality command
                switch (p_strCommandName)
                {
                    case "Open":
                        l_strCallingFunctionName = "doLiftAction(LiftAction.Extend)";
                        l_objLiftAction = LiftAction.Extend;
                        break;
                    case "Close":
                        l_strCallingFunctionName = "doLiftAction(LiftAction.Retract)";
                        l_objLiftAction = LiftAction.Retract;
                        break;
                    case "Cancel":
                        l_strCallingFunctionName = "doLiftAction(LiftAction.Cancel)";
                        l_objLiftAction = LiftAction.Cancel;
                        break;
                    default:
                        throw new Exception(string.Format("{0}: {1}: {2} Command NOT Found.", l_strFunctionName, LiftName, p_strCommandName));
                }
                m_objLogger.logToMemory(string.Format("{0}: {1}: Attempting to call {2} based on receiving the following command name: {3}.", l_strFunctionName, LiftName, l_strCallingFunctionName, p_strCommandName));
                AsyncContext.Run(() => doLiftAction(l_objLiftAction));
                l_objStatus = actionStatus.Success;
                m_objLogger.logToMemory(string.Format("{0}: {1}: {2} completed.", l_strFunctionName, LiftName, l_strCallingFunctionName), l_objStatus);
            }
            catch (Exception e)
            {
                l_objStatus = actionStatus.Error;
                m_objLogger.logToMemory(string.Format("{0}: {1}: Error Sending Command By Name: {2}", l_strFunctionName, LiftName, e.ToString()), l_objStatus);
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
                m_objLogger.logToMemory(string.Format("{0}: {1}: Attempting to Send Message through Serial Port: {2}", l_strFunctionName, LiftName, p_strCommand), l_objStatus);
                sendSerialData(LiftCOMPort, 9600, System.IO.Ports.Parity.None, 8, System.IO.Ports.StopBits.One, p_strCommand);
                l_objStatus = actionStatus.Success;
                m_objLogger.logToMemory(string.Format("{0}: {1}: Lift Action Message Sent: {2}", l_strFunctionName, LiftName, p_strCommand), l_objStatus);
            }
            catch (Exception e)
            {
                l_objStatus = actionStatus.Error;
                m_objLogger.logToMemory(string.Format("{0}: {1}: Error Sending Command to the Projector Lift: {2}.", l_strFunctionName, LiftName, e.ToString()), l_objStatus);
            }
            finally { m_objLogger.writePendingToDB(l_objStatus, p_strFunctionName: l_strFunctionName); }

            return l_objStatus;
        }

        public async Task<actionStatus> doLiftAction(LiftAction p_objLiftAction)
        {
            var l_objStatus = actionStatus.None;
            var l_strFunctionName = "doLiftAction()";
            var l_blnPerformAwait = false;

            try
            {
                m_objLogger.logToMemory(string.Format("{0}: {1}: Attempting to performs action: {2}", l_strFunctionName, LiftName, p_objLiftAction.ToString()), l_objStatus);

                var l_strCurrentAction = "";
                switch (p_objLiftAction)
                {
                    case LiftAction.Cancel: l_strCurrentAction = m_cstrCancel; break;
                    case LiftAction.Extend: l_strCurrentAction = m_cstrExtend; l_blnPerformAwait = true; break;
                    case LiftAction.Retract: l_strCurrentAction = m_cstrRetract; l_blnPerformAwait = true; break;
                    default:
                        throw new Exception(string.Format("Lift Action: {0} Not Found!", p_objLiftAction.ToString()));
                }

                m_objLogger.logToMemory(string.Format("{0}: {1}: Lift Action Found. Attempting to Send Message through Serial Port: {2})",l_strFunctionName, LiftName, LiftCOMPort), l_objStatus);

                sendSerialData(LiftCOMPort, 9600, System.IO.Ports.Parity.None, 8, System.IO.Ports.StopBits.One, l_strCurrentAction);
                if (l_blnPerformAwait) await Task.Delay(LiftMoveTime);

                l_objStatus = actionStatus.Success;
                m_objLogger.logToMemory(string.Format("{0}: {1}: Lift Action Message Sent: {2}", l_strFunctionName, LiftName, l_strCurrentAction), l_objStatus);
            }
            catch (Exception e)
            {
                l_objStatus = actionStatus.Error;
                m_objLogger.logToMemory(string.Format("{0}: {1}: Error Performing Lift Action: {2}", l_strFunctionName, LiftName, e.ToString()), l_objStatus);
            }
            finally { m_objLogger.writePendingToDB(l_objStatus, p_strFunctionName: l_strFunctionName); }
            return l_objStatus;
        }

        public string GetDeviceInfo(bool l_blnBasic = false)
        {
            StringBuilder l_objDeviceInfo = new StringBuilder();
            l_objDeviceInfo.AppendLine("  Name:      " + LiftName);

            if (!l_blnBasic)
            {
                l_objDeviceInfo.AppendLine("  COM Port:  " + LiftCOMPort);
                l_objDeviceInfo.AppendLine("  Move Time: " + LiftMoveTime);
            }

            return l_objDeviceInfo.ToString();
        }

        public bool isDeviceConfigured()
        {
            return ((LiftCOMPort.Trim().Length > 0) && (LiftMoveTime > 0));
        }

    }
}
