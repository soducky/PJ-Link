using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace rv
{
    public class InputCommand : Command
    {
        private InputType _type = InputType.UNKNOWN;
        private int _port = -1;

        public enum InputType
        {
            UNKNOWN,
            RGB,
            VIDEO,
            DIGITAL,
            STORAGE,
            NETWORK
        }

        ///ports start with 1 !!!
        public InputCommand(InputType type, int port)
        {
            _type = type;
            _port = port;
        }

        ///Query
        public InputCommand()
        {

        }

        internal override string getCommandString()
        {
            string cmdString = "%1INPT ";

            if (_port > 0)
            {
                cmdString += ((int)_type).ToString() + _port.ToString();
            }
            else
            {
                cmdString += "?";
            }

            return cmdString;
        }

        internal override bool processAnswerString(string a)
        {
            if (!base.processAnswerString(a))
                return false;

            if (_port < 0)
            {
                a = a.Replace("%1INPT=", "");
                int retVal = int.Parse(a);

                _type = (InputType)(retVal / 10);
                _port = retVal % 10;
            }

            return true;
        }


        public InputType Input
        {
            get { return _type; }
        }

        public int Port
        {
            get { return _port; }
        }

        public override string dumpToString()
        {
            string toRet = "Input: " + _type.ToString() + _port.ToString();
            return toRet;
        }
    }
}
