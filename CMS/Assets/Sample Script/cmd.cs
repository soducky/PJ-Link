using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System.Windows;
using System;

public class cmd : MonoBehaviour
{
    public void OffBtn()
    {
        OffComputer();
    }
    void OffComputer()
    {
        ProcessStartInfo proInfo = new ProcessStartInfo();
        Process pro = new Process();
       

        proInfo.FileName = @"cmd"; // ������ ���ϸ� �Է�

        proInfo.CreateNoWindow= false; // cmd â ���� true(������ʱ�), false(����)
        proInfo.UseShellExecute = false; 
        proInfo.RedirectStandardOutput = true; // cmd �����͹ޱ�
        proInfo.RedirectStandardInput= true; // cmd ������ ������
        proInfo.RedirectStandardError= true; // �������� �ޱ�
     

        pro.StartInfo = proInfo; 
        pro.Start();
        pro.StandardInput.Write(@"net use \\192.168.10.23 /user: PC" + Environment.NewLine);
        pro.StandardInput.Write(@"shutdown /s /m \\192.168.10.23" + Environment.NewLine);
        pro.StandardInput.Close();


        pro.WaitForExit();
        pro.Close();

    }
}
