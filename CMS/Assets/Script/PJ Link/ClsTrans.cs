using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using rv;
using cms;
using System.Net.Sockets;

public class ClsTrans : MonoBehaviour
{
    public void onBtn()
    {
        PJLinkConnection c = new PJLinkConnection("192.168.10.150",4352);
        //   c.turnOff();
        c.turnOn();
    }
}
