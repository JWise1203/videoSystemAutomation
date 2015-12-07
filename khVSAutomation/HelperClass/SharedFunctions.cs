using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace khVSAutomation
{
    static class SharedFunctions
    {
        #region Logging
        /// <summary>
        /// This is Confusing for now... but necessary. This allows us to Log to the DB and to a separate String Builder for console app display
        /// This will be cleaned up as soon as the UI is completed.
        /// </summary>
        /// <param name="p_objDBLogger"></param>
        /// <param name="p_objProgress"></param>
        /// <param name="p_strMessage"></param>
        /// <param name="p_blnWriteProgressToStringBuilder"></param>
        /// <param name="p_blnNewLine"></param>
        public static void writeProgress(string p_strMessage,
                                        ref Logger p_objDBLogger, 
                                        StringBuilder p_objProgress,  
                                        actionStatus p_objActionStatus = actionStatus.None, 
                                        bool p_blnWriteToStringBuilder = true, 
                                        bool p_blnWriteToLogger = true,
                                        bool p_blnNewLine = true)
        {

            if (p_blnWriteToLogger)  p_objDBLogger.logToMemory(p_strMessage, p_objActionStatus, p_blnNewLine);

            //Writing to the DB is handled separately.
            if (p_blnWriteToStringBuilder)
            {
                if (p_objProgress == null) p_objProgress = new StringBuilder();

                if (p_blnNewLine) p_objProgress.AppendLine(p_strMessage);
                else p_objProgress.Append(p_strMessage);
            }
        }
        #endregion
    }
}
