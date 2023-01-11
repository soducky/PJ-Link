using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System;
using System.Text;
using Unity.VisualScripting;
using System.Diagnostics;

public class SampleCode : MonoBehaviour
{
    private Boolean g_Connected;
    private Socket m_ClientSocket = null;
    private AsyncCallback m_fnReceiveHandler;
    private AsyncCallback m_fnSendHandler;

    private bool isCoroutine = false;

    private string str = "c";
    private IEnumerator coroutine;

    public String hostName = "DESKTOP-PML0E22";
    public UInt16 hostPort = 3040;

    public class AsyncObject
    {
        public Byte[] Buffer;
        public Socket WorkingSocket;
        public AsyncObject(Int32 bufferSize)
        {
            this.Buffer = new Byte[bufferSize];
        }
    }

    private void Start()
    {
        ConnectToServer(hostName, hostPort);
    }

    private void Update()
    {
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
        SendMessage(str);
        isCoroutine = false;
       
    }

    public SampleCode()
    {
        // �񵿱� �۾��� ���� �븮�ڸ� �ʱ�ȭ�մϴ�.
        m_fnReceiveHandler = new AsyncCallback(handleDataReceive);
        m_fnSendHandler = new AsyncCallback(handleDataSend);

        UnityEngine.Debug.Log("�����ڿϷ�");
    }

    public Boolean Connected
    {
        get
        {
            return g_Connected;
        }
    }

    public void ConnectToServer(String hostName, UInt16 hostPort)
    {
        // TCP ����� ���� ������ �����մϴ�.
      
        m_ClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);

        Boolean isConnected = false;
        try
        {
            // ���� �õ�
            m_ClientSocket.Connect(hostName, hostPort);

            // ���� ����
            isConnected = true;

            UnityEngine.Debug.Log("ù����");
        }
        catch
        {
            // ���� ���� (���� ���� ������ �߻���)
            isConnected = false;
        }
        g_Connected = isConnected;

        if (isConnected)
        {

            // 4096 ����Ʈ�� ũ�⸦ ���� ����Ʈ �迭�� ���� AsyncObject Ŭ���� ����
            AsyncObject ao = new AsyncObject(4096);

            // �۾� ���� ������ �����ϱ� ���� sockClient �Ҵ�
            ao.WorkingSocket = m_ClientSocket;

            // �񵿱������� ������ �ڷḦ �����ϱ� ���� BeginReceive �޼��� ���!
            m_ClientSocket.BeginReceive(ao.Buffer, 0, ao.Buffer.Length, SocketFlags.None, m_fnReceiveHandler, ao);

            UnityEngine.Debug.Log("����Ϸ�");


        }
        else
        {

            UnityEngine.Debug.Log("�������");

        }
    }

    public void StopClient()
    {
        // �������� Ŭ���̾�Ʈ ������ �ݽ��ϴ�.
        m_ClientSocket.Close();
    }

    private void OnApplicationQuit()
    {
        StopClient();
    }

    public new void  SendMessage(String message)
    {
        // �߰� ������ �ѱ�� ���� ���� ����
        // ũ�⸦ �����ϴ°� �ǹ̰� �����ϴ�.
        // �ֳ��ϸ� �ٷ� ���� �ڵ忡�� ���ڿ��� �����ڵ� ������ ��ȯ�� ����Ʈ �迭�� ��ȯ�ϱ� ������
        // �ּ����� ũ�⸣ �迭�� �ʱ�ȭ�մϴ�.
        AsyncObject ao = new AsyncObject(1);

        // ���ڿ��� ����Ʈ �迭���� ��ȯ
        ao.Buffer = Encoding.Unicode.GetBytes(message);

        ao.WorkingSocket = m_ClientSocket;

        // ���� ����!
        try
        {
            m_ClientSocket.BeginSend(ao.Buffer, 0, ao.Buffer.Length, SocketFlags.None, m_fnSendHandler, ao);

            UnityEngine.Debug.Log("c����");

        }
        catch (Exception ex)
        {
            UnityEngine.Debug.Log("���� �� ���� �߻�!\n�޼���: {0}"+ex.Message);
        }
    }

    private void handleDataReceive(IAsyncResult ar)
    {

        // �Ѱ��� �߰� ������ �����ɴϴ�.
        // AsyncState �Ӽ��� �ڷ����� Object �����̱� ������ �� ��ȯ�� �ʿ��մϴ�~!
        AsyncObject ao = (AsyncObject)ar.AsyncState;

        // ���� ����Ʈ �� ������ ���� ����
        Int32 recvBytes;

        try
        {
            // �ڷḦ �����ϰ�, ���Ź��� ����Ʈ�� �����ɴϴ�.
            recvBytes = ao.WorkingSocket.EndReceive(ar);
        }
        catch
        {
            // ���ܰ� �߻��ϸ� �Լ� ����!
            return;
        }

        // ���Ź��� �ڷ��� ũ�Ⱑ 1 �̻��� ������ �ڷ� ó��
        if (recvBytes > 0)
        {
            // ���� ���ڵ��� ���� �߻��� �� �����Ƿ�, ���� ����Ʈ �� ��ŭ �迭�� �����ϰ� �����Ѵ�.
            Byte[] msgByte = new Byte[recvBytes];
            Array.Copy(ao.Buffer, msgByte, recvBytes);

            // ���� �޼����� ���
            if(Encoding.Unicode.GetString(msgByte)=="s")
            {
                UnityEngine.Debug.Log("����");
                OffComputer();
            }
        }

        try
        {
            // �ڷ� ó���� ��������~
            // ���� �ٽ� �����͸� ���Źޱ� ���ؼ� ���� ��⸦ �ؾ� �մϴ�.
            // Begin~~ �޼��带 �̿��� �񵿱������� �۾��� ����ߴٸ�
            // �ݵ�� �븮�� �Լ����� End~~ �޼��带 �̿��� �񵿱� �۾��� �����ٰ� �˷���� �մϴ�!
            ao.WorkingSocket.BeginReceive(ao.Buffer, 0, ao.Buffer.Length, SocketFlags.None, m_fnReceiveHandler, ao);
        }
        catch (Exception ex)
        {
            // ���ܰ� �߻��ϸ� ���� ���� ��� �� �Լ��� �����Ѵ�.
            Console.WriteLine("�ڷ� ���� ��� ���� ���� �߻�! �޼���: {0}", ex.Message);
            return;
        }
    }
    private void handleDataSend(IAsyncResult ar)
    {

        // �Ѱ��� �߰� ������ �����ɴϴ�.
        AsyncObject ao = (AsyncObject)ar.AsyncState;

        // ���� ����Ʈ ���� ������ ���� ����
        Int32 sentBytes;

        try
        {
            // �ڷḦ �����ϰ�, ������ ����Ʈ�� �����ɴϴ�.
            sentBytes = ao.WorkingSocket.EndSend(ar);
        }
        catch (Exception ex)
        {
            // ���ܰ� �߻��ϸ� ���� ���� ��� �� �Լ��� �����Ѵ�.
            Console.WriteLine("�ڷ� �۽� ���� ���� �߻�! �޼���: {0}", ex.Message);
            return;
        }

        if (sentBytes > 0)
        {
            // ���⵵ ���������� ���� ����Ʈ �� ��ŭ �迭 ���� �� �����Ѵ�.
            Byte[] msgByte = new Byte[sentBytes];
            Array.Copy(ao.Buffer, msgByte, sentBytes);

            UnityEngine.Debug.Log("�޼��� ����: {0}"+Encoding.Unicode.GetString(msgByte));
        }
    }

    void OffComputer()
    {
        ProcessStartInfo proInfo = new ProcessStartInfo();
        Process pro = new Process();


        proInfo.FileName = @"cmd"; // ������ ���ϸ� �Է�

        proInfo.CreateNoWindow = false; // cmd â ���� true(������ʱ�), false(����)
        proInfo.UseShellExecute = false;
        proInfo.RedirectStandardOutput = true; // cmd �����͹ޱ�
        proInfo.RedirectStandardInput = true; // cmd ������ ������
        proInfo.RedirectStandardError = true; // �������� �ޱ�


        pro.StartInfo = proInfo;
        pro.Start();
        pro.StandardInput.Write(@"shutdown -s -t 0" + Environment.NewLine);
        pro.StandardInput.Close();


        pro.WaitForExit();
        pro.Close();

    }
}
