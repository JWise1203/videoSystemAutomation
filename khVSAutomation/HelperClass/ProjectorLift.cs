using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace khVSAutomation
{
    class ProjectorLift
    {
        public ProjectorLift(string p_strName, string p_strCOM, int p_strMoveTime)
        {
            LiftName = p_strName;
            LiftCOMPort = p_strCOM;
            LiftMoveTime = p_strMoveTime;
        }
        
        public string LiftName { get; set; }
        public string LiftCOMPort { get; set; }
        public int LiftMoveTime { get; set; }
    }
}
