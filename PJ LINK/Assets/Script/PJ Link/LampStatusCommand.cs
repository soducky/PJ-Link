using rv;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace rv
{
    public class LampStatusCommand : Command
    {
        public enum Status
        {
            OFF,
            ON,
            UNKNOWN
        }

        private List<Status> _status = new List<Status>();
        private List<int> _hours = new List<int>();

        public LampStatusCommand()
        {

        }

        internal override string getCommandString()
        {
            return "%1LAMP ?";
        }

        internal override bool processAnswerString(string a)
        {
            if (!base.processAnswerString(a))
            {
                return false;
            }

            a = a.Replace("%1LAMP=", "");

            string[] arr = a.Split(' ');
            for (int i = 0; i < arr.Length; i += 2)
            {
                _hours.Add(int.Parse(arr[i]));
                _status.Add((Status)(int.Parse(arr[i + 1])));
            }

            return true;
        }

        public List<Status> StatusList
        {
            get { return _status; }
        }

        public List<int> HoursList
        {
            get { return _hours; }
        }

        public int NumOfLamps
        {
            get { return _hours.Count; }
        }

        public Status getStatusOfLamp(int l)
        {
            if (l < NumOfLamps)
                return _status[l];
            return Status.UNKNOWN;
        }

        public int getHoursOfLamp(int l)
        {
            if (l < NumOfLamps)
                return _hours[l];
            return -1;
        }

        public override string dumpToString()
        {
            string toRet = "";

            toRet += "Num of Lamps: " + NumOfLamps.ToString();
            for (int i = 0; i < NumOfLamps; ++i)
            {
                toRet += "\n  Lamp " + i.ToString() + ": " + _status[i].ToString() + "(" + _hours[i].ToString() + "h)";
            }

            return toRet;
        }
    }
}
