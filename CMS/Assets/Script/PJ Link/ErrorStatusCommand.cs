using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace rv
{

public class ErrorStatusCommand : Command
{
        public enum Status
        {
            OK,
            WARNING,
            ERROR,
            UNKNOWN
        }

        public enum SysComponent
        {
            FAN,
            LAMP,
            TEMP,
            COVER,
            FILTER,
            OTHER
        }

        private List<Status> _status = new List<Status>(6);

        public ErrorStatusCommand()
        {
            for (int i = 0; i < 6; ++i)
                _status.Add(Status.UNKNOWN);
        }

        internal override string getCommandString()
        {
            return "%1ERST ?";
        }

        internal override bool processAnswerString(string a)
        {
            if (!base.processAnswerString(a))
            {
                return false;
            }

            a = a.Replace("%1ERST=", "");
            for (int i = 0; i < a.Length; ++i)
            {
                _status[i] = (Status)int.Parse(a[i].ToString());
            }

            return true;
        }

        public List<Status> StatusList
        {
            get { return _status; }
        }

        public Status FanStatus
        {
            get { return _status[(int)SysComponent.FAN]; }
        }

        public Status LampStatus
        {
            get { return _status[(int)SysComponent.LAMP]; }
        }

        public Status TempStatus
        {
            get { return _status[(int)SysComponent.TEMP]; }
        }

        public Status CoverStatus
        {
            get { return _status[(int)SysComponent.COVER]; }
        }

        public Status FilterStatus
        {
            get { return _status[(int)SysComponent.FILTER]; }
        }

        public Status OtherStatus
        {
            get { return _status[(int)SysComponent.OTHER]; }
        }

        public override string dumpToString()
        {
            string toRet = "";

            toRet += "Fan:    " + FanStatus.ToString();
            toRet += "\nLamp:   " + LampStatus.ToString();
            toRet += "\nTemp:   " + TempStatus.ToString();
            toRet += "\nCover:  " + CoverStatus.ToString();
            toRet += "\nFilter: " + FilterStatus.ToString();
            toRet += "\nOther:  " + OtherStatus.ToString();
            return toRet;
        }
    }

}
