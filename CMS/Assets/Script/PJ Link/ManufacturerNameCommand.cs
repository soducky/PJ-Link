using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace rv
{
    public class ManufacturerNameCommand : Command
    {
        private string _name = "";

        public ManufacturerNameCommand()
        {

        }

        internal override string getCommandString()
        {
            return "%1INF1 ?";
        }

        internal override bool processAnswerString(string a)
        {
            if (!base.processAnswerString(a))
            {
                return false;
            }

            _name = a.Replace("%1INF1=", "");

            return true;
        }


        public override string dumpToString()
        {
            string toRet = "Manufacturer: " + _name;
            return toRet;
        }

        public string Manufacturer
        {
            get { return _name; }
        }
    }
}
