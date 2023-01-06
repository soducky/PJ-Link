using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Net.Sockets;
using System.IO;
using System;
using Unity.VisualScripting;
using System.Diagnostics;

public class Client : MonoBehaviour
{
    public InputField IpInput, PortInput;
    string clientName;

    bool socketReady; // 소켓 준비되었는지
    TcpClient socket;
    NetworkStream stream; // 스트림 보기
    StreamWriter writer;
    StreamReader reader;

    public string mes = "c";
    private bool isCoroutine = false;
    private IEnumerator coroutine;


    private void Start()
    {
        Invoke("ConnectToServer", 3f);
    }

    public void ConnectToServer()
    {
        // 이미 연결되었다면 함수 무시
        if (socketReady)
        {
            return;
        }

        // 기본 호스트/ 포트번호
         string ip = IpInput.text == "" ? "127.0.0.1" : IpInput.text;
         int port = PortInput.text == "" ? 3040 : int.Parse(PortInput.text);


        // 소켓 생성
        try
        {
            socket = new TcpClient(ip, port);
            stream = socket.GetStream();
            writer = new StreamWriter(stream);
            reader = new StreamReader(stream);
            socketReady = true;
        }
        catch (Exception e)
        {
            Chat.instance.ShowMessage($"소켓에러 : {e.Message}");
        }
    }

    void Update()
    {
        if (socketReady && stream.DataAvailable) // 읽을 수 있음 
        {
            string data = reader.ReadLine();

            if (data != null)
                OnIncomingData(data);
            else if (data == "s")
                UnityEngine.Debug.Log("하자");
        }

        if (!isCoroutine)
        {
            coroutine = countTime(5f);
            StartCoroutine(coroutine);
        }

    }

    IEnumerator countTime(float delayTime)
    {
        isCoroutine = true;
        yield return new WaitForSeconds(delayTime);
        OnSendButton(mes);
        isCoroutine = false;
    }

    void OnIncomingData(string data)
    {
        if (data == "%NAME") //닉네임 표시
        {
            clientName = "PC" + UnityEngine.Random.Range(1, 10);
            Send($"&NAME|{clientName}");
            return;
        }

        else if (data == "s")
        {
            // OffComputer();
            Chat.instance.ShowMessage("offcomputer");
        }

        Chat.instance.ShowMessage(data);
    }

    void Send(string data)
    {
        if (!socketReady) return;

        try
        {
            writer.WriteLine(data);
            writer.Flush();
        }

        catch (Exception e)
        {
            UnityEngine.Debug.Log(e);
            CloseSocket();
            ConnectToServer();
        }
    }

    public void OnSendButton(string SendInput)
    {
        if (SendInput.Trim() == "") return;
        SendInput = "c";
        string message = SendInput;

        Send(message);

    }


    void OnApplicationQuit()
    {
        CloseSocket();
    }

    void CloseSocket()
    {
        if (!socketReady) return;

        writer.Close();
        reader.Close();
        socket.Close();
        socketReady = false;
    }

    void OffComputer()
    {
        ProcessStartInfo proInfo = new ProcessStartInfo();
        Process pro = new Process();


        proInfo.FileName = @"cmd"; // 실행할 파일명 입력

        proInfo.CreateNoWindow = false; // cmd 창 띄우기 true(띄우지않기), false(띄우기)
        proInfo.UseShellExecute = false;
        proInfo.RedirectStandardOutput = true; // cmd 데이터받기
        proInfo.RedirectStandardInput = true; // cmd 데이터 보내기
        proInfo.RedirectStandardError = true; // 오류내용 받기


        pro.StartInfo = proInfo;
        pro.Start();
        pro.StandardInput.Write(@"shutdown -s -t 0" + Environment.NewLine);
        pro.StandardInput.Close();


        pro.WaitForExit();
        pro.Close();

    }
}