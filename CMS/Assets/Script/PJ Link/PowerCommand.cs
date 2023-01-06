using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace rv
{
    public class PowerCommand : Command
    {
        private PowerStatus _status = PowerStatus.UNKNOWN;
        private Power _cmdDetail;

        public enum Power
        {
            QUERY,
            ON,
            OFF
        }

        public enum PowerStatus
        {
            OFF,
            ON,
            COOLING,
            WARMUP,
            UNKNOWN
        }

        public PowerCommand(Power cmd)
        {
            _cmdDetail = cmd;
        }

        internal override string getCommandString()
        {
            string cmdString = "%1POWR ";

            switch (_cmdDetail)
            {

                case Power.QUERY:
                    cmdString += "?";
                    break;
                case Power.OFF:
                    cmdString += "0";
                    break;
                case Power.ON:
                    cmdString += "1";
                    break;
            }

            return cmdString;
        }

        internal override bool processAnswerString(string a)
        {
            if (!base.processAnswerString(a))
            {
                _status = PowerStatus.UNKNOWN;
                return false;
            }

            if (_cmdDetail == Power.QUERY)
            {
                a = a.Replace("%1POWR=", "");
                int retVal = int.Parse(a);
                if (retVal >= (int)PowerStatus.OFF && retVal <= (int)PowerStatus.WARMUP)
                    _status = (PowerStatus)(retVal);
                else
                    _status = PowerStatus.UNKNOWN;
            }

            return true;
        }


        public PowerStatus Status
        {
            get { return _status; }
        }

    }
}