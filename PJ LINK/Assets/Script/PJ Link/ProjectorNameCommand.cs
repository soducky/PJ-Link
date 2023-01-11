using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace rv
{
    public class ProjectorNameCommand : Command
    {
        private string _name = "";

        public ProjectorNameCommand()
        {

        }

        internal override string getCommandString()
        {
            return "%1NAME ?";
        }

        internal override bool processAnswerString(string a)
        {
            if (!base.processAnswerString(a))
            {
                return false;
            }

            a = a.Replace("%1NAME=", "");

            byte[] dec = Encoding.ASCII.GetBytes(a);
            _name = Encoding.UTF8.GetString(dec);

            return true;
        }


        public override string dumpToString()
        {
            return "Name: " + _name;
        }

        public string Name
        {
            get { return _name; }
        }
    }
}
