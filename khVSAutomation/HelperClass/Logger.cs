using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nito.AsyncEx;

namespace khVSAutomation
{

    class Logger
    {
        private static StringBuilder m_objMemoryLog;
        private static logLevel m_objLogLevel;
        private static AutomationsEntities myDB = null; 
        private static string m_strSessionID;
        private static string m_strClassName;

        public Logger(ref AutomationsEntities p_objMyDB, string p_strSessionID, logLevel p_objLogLevel = logLevel.ErrorOnly, string p_strClassName = "")
        {
            m_objLogLevel = p_objLogLevel;
            m_objMemoryLog = new StringBuilder();
            myDB = p_objMyDB;
            m_strSessionID = p_strSessionID;
            m_strClassName = p_strClassName;
        }

        public bool logToMemory(string p_strDescription,actionStatus p_objStatus = actionStatus.None, bool p_blnNewLine = true, bool p_blnLogAndWrite = false, string p_strFunctionName = "")
        {
            bool l_blnAddToLog = false;
            try
            {
                if ((m_objMemoryLog != null) && canLog(p_objStatus))
                {
                    l_blnAddToLog = true;
                    if (p_blnNewLine) 
                        m_objMemoryLog.AppendLine(p_strDescription);
                    else
                        m_objMemoryLog.Append(p_strDescription);
                }  
                if (p_blnLogAndWrite) writePendingToDB(p_objStatus, p_strFunctionName: p_strFunctionName);
            }
            catch { /* DO NOTHING */ }
            return l_blnAddToLog;
        }

        //public async Task<string> writePendingToDBAsync(actionStatus p_objStatus = actionStatus.None, bool p_blnReturnOutput = false, string p_strUpdatedBy = "GeneralUser")
        public string writePendingToDB(actionStatus p_objStatus = actionStatus.None, bool p_blnReturnOutput = false, string p_strUpdatedBy = "GeneralUser", string p_strFunctionName = "")
        {
            string l_strOutput = "";

            if (myDB != null && m_objMemoryLog != null && m_objMemoryLog.Length > 0 ) 
            {
                l_strOutput = string.Format("{0} {1} {2}", m_strClassName, p_strFunctionName, m_objMemoryLog.ToString());
                
                myDB.tblOperationStatus.Add(new tblOperationStatu {SessionID = m_strSessionID,
                                                                        StatusID = (int)p_objStatus, 
                                                                        Description = l_strOutput, 
                                                                        UpdatedBy = p_strUpdatedBy, 
                                                                        UpdatedDateTime = DateTime.Now.ToUniversalTime()});
                //Task<int> saveResult = myDB.SaveChangesAsync();
                //For Now we will run these syncronously
                AsyncContext.Run(() => myDB.SaveChangesAsync());
                m_objMemoryLog.Clear(); //Clear to ensure we can log to another record
                if (p_blnReturnOutput == false) l_strOutput = ""; //Return nothing or everything that was just written
                //await saveResult;
            }
            return l_strOutput;
        }

        /// <summary>
        /// Write Out to the DB Immediately
        /// 
        /// e.g.: Write for Errors or other important messages that may not be able to wait.
        /// </summary>
        /// <param name="p_strDescription"></param>
        /// <param name="p_objStatus"></param>
        /// <param name="p_strUpdatedBy"></param>
        //public async void logToDBAsync(string p_strDescription, actionStatus p_objStatus = actionStatus.None, string p_strUpdatedBy = "GeneralUser")
        public string logToDB(string p_strDescription, actionStatus p_objStatus = actionStatus.None, bool p_blnReturnOutput = false, string p_strUpdatedBy = "GeneralUser", bool p_blnError_WriteMemoryToDB = false, string p_strFunctionName = "")
        {
            //Let's do the work here so as not to repeat ourselves throughout the project
            string l_strOutput = "";
            bool l_blnAddToLog = canLog(p_objStatus);
            if (!(myDB == null) && l_blnAddToLog)
            {
                if (p_blnError_WriteMemoryToDB) writePendingToDB(p_objStatus, p_strFunctionName: p_strFunctionName);
                l_strOutput = string.Format("{0} {1} {2}", m_strClassName, p_strFunctionName, p_strDescription);
                myDB.tblOperationStatus.Add(new tblOperationStatu {SessionID = m_strSessionID, 
                                                                    StatusID = (int)p_objStatus,
                                                                    Description = l_strOutput, 
                                                                    UpdatedBy = p_strUpdatedBy, 
                                                                    UpdatedDateTime = DateTime.Now.ToUniversalTime()});
                //int x = await myDB.SaveChangesAsync();
                //For Now we will run these syncronously
                AsyncContext.Run(() => myDB.SaveChangesAsync());
            }
            return l_strOutput;
        }

        private bool canLog(actionStatus p_objStatus)
        {
            bool l_blnCanLog = false;

            if ((m_objLogLevel == logLevel.All) ||
                (m_objLogLevel == logLevel.ErrorOnly &&
                (p_objStatus == actionStatus.PartialError ||
                p_objStatus == actionStatus.Error))) l_blnCanLog = true;

            return l_blnCanLog;
        }

    }
}
