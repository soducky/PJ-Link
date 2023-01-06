using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace rv
{
    public class ProductNameCommand : Command
    {
        // Start is called before the first frame update
        private string _name = "";

        public ProductNameCommand()
        {

        }

        internal override string getCommandString()
        {
            return "%1INF2 ?";
        }

        internal override bool processAnswerString(string a)
        {
            if (!base.processAnswerString(a))
            {
                return false;
            }

            _name = a.Replace("%1INF2=", "");

            return true;
        }


        public override string dumpToString()
        {
            string toRet = "ProductName: " + _name;
            return toRet;
        }

        public string ProductName
        {
            get { return _name; }
        }
    }
}
