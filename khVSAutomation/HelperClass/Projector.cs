using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace khVSAutomation
{
    class Projector
    {
        public Projector(string p_strName, string p_strIP, string p_strLiftAssociation)
        {
            projectorName = p_strName;
            projectorIP = p_strIP;
            projectorLiftAssociation = p_strLiftAssociation;
        }
        
        public string projectorName { get; set; }
        public string projectorIP { get; set; }
        public string projectorLiftAssociation { get; set; }
    }
}
