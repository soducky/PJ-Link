using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace rv
{
    public class Command : MonoBehaviour
    {
        public delegate void CommandResultHandler(Command sender, Response response);

        protected Response _cmdResponse;

        public enum Response
        {
            SUCCESS,
            UNDEFINED_CMD,
            OUT_OF_PARAMETER,
            UNVAILABLE_TIME,
            PROJECTOR_FAILURE,
            AUTH_FAILURE,
            COMMUNICATION_ERROR
        }

        internal virtual string getCommandString()
        {
            //NOOP
            return "";
        }

        internal virtual bool processAnswerString(string a)
        {

            if (a.IndexOf("=ERR1") >= 0)
                _cmdResponse = Response.UNDEFINED_CMD;
            else if (a.IndexOf("=ERR2") >= 0)
                _cmdResponse = Response.UNDEFINED_CMD;
            else if (a.IndexOf("=ERR3") >= 0)
                _cmdResponse = Response.UNVAILABLE_TIME;
            else if (a.IndexOf("=ERR4") >= 0)
                _cmdResponse = Response.PROJECTOR_FAILURE;
            else if (a.IndexOf(" ERRA") >= 0)
                _cmdResponse = Response.AUTH_FAILURE;
            else //OK or query answer...
                _cmdResponse = Response.SUCCESS;

            return _cmdResponse == Response.SUCCESS;
        }

        public Response CmdResponse
        {
            get { return _cmdResponse; }
        }

        public virtual string dumpToString()
        {
            return "";
        }
    }
}

