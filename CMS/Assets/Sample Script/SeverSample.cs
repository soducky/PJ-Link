using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.Net;
using System;
using System.Text;
using UnityEngine.UI;

public class SeverSample : MonoBehaviour
{
    private Socket m_ServerSocket = null;
    private AsyncCallback m_fnReceiveHandler;
    private AsyncCallback m_fnSendHandler;
    private AsyncCallback m_fnAcceptHandler;

    public Image pcoffimage;
    public Sprite pconimage;
    public Sprite rechagePcOffImage;

    public InputField input_port;
    private UInt16 m_Port = 3040;

    private string str = "s";

    public class AsyncObject
    {
        public Byte[] Buffer;
        public Socket WorkingSocket;
        public AsyncObject(Int32 bufferSize)
        {
            this.Buffer = new Byte[bufferSize];
        }
    }

    public void Start()
    {
        StartServer(m_Port);
    }

    public void StartServer(UInt16 ChangePort)
    {
        // �񵿱� �۾��� ���� �븮�� �ʱ�ȭ
        m_fnReceiveHandler = new AsyncCallback(handleDataReceive);
        m_fnSendHandler = new AsyncCallback(handleDataSend);
        m_fnAcceptHandler = new AsyncCallback(handleClientConnectionRequest);

        // ���� ����
        m_ServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);

        // ����� ��Ʈ: 3040
        if (ChangePort == 3040)
        {
            m_ServerSocket.Bind(new IPEndPoint(IPAddress.Any, ChangePort));
            Debug.Log("3040 ����");
        }

        else if (ChangePort != 3040)
        {
            UnityEngine.Debug.Log("��Ʈ�� �ٲټ��� 3040");
        }
  
        // ���� ��û ����
        m_ServerSocket.Listen(5);

        // BeginAccept �޼��带 �̿��� ������ ���� ��û�� �񵿱������� ó���մϴ�.
        // ���� ��û�� ó���ϴ� �Լ��� handleClientConnectionRequest �Դϴ�.
        m_ServerSocket.BeginAccept(m_fnAcceptHandler, null);

        Debug.Log("�������� �Ϸ�" + ChangePort);
    }


    public void StopServer()
    {
   
        m_ServerSocket.Close();
    }

    public new void SendMessage(String message)
    {
        // �߰� ������ �ѱ�� ���� ���� ����
        // ũ�⸦ �����ϴ°� �ǹ̰� �����ϴ�.
        // �ֳ��ϸ� �ٷ� ���� �ڵ忡�� ���ڿ��� �����ڵ� ������ ��ȯ�� ����Ʈ �迭�� ��ȯ�ϱ� ������
        // �ּ����� ũ�⸣ �迭�� �ʱ�ȭ�մϴ�.
        AsyncObject ao = new AsyncObject(1);

        // ���ڿ��� ����Ʈ �迭���� ��ȯ
        ao.Buffer = Encoding.Unicode.GetBytes(message);

        // ���� ������ ����
        ao.WorkingSocket = m_ServerSocket;

        // ���� ����!
        m_ServerSocket.BeginSend(ao.Buffer, 0, ao.Buffer.Length, SocketFlags.None, m_fnSendHandler, ao);
    }



    private void handleClientConnectionRequest(IAsyncResult ar)
    {
        // Ŭ���̾�Ʈ�� ���� ��û�� �����մϴ�.
        Socket sockClient = m_ServerSocket.EndAccept(ar);

        // 4096 ����Ʈ�� ũ�⸦ ���� ����Ʈ �迭�� ���� AsyncObject Ŭ���� ����
        AsyncObject ao = new AsyncObject(4096);

        // �۾� ���� ������ �����ϱ� ���� sockClient �Ҵ�
        ao.WorkingSocket = sockClient;

        // �񵿱������� ������ �ڷḦ �����ϱ� ���� BeginReceive �޼��� ���!
        sockClient.BeginReceive(ao.Buffer, 0, ao.Buffer.Length, SocketFlags.None, m_fnReceiveHandler, ao);
    }
    private void handleDataReceive(IAsyncResult ar)
    {

        // �Ѱ��� �߰� ������ �����ɴϴ�.
        // AsyncState �Ӽ��� �ڷ����� Object �����̱� ������ �� ��ȯ�� �ʿ��մϴ�~!
        AsyncObject ao = (AsyncObject)ar.AsyncState;

        // �ڷḦ �����ϰ�, ���Ź��� ����Ʈ�� �����ɴϴ�.
        Int32 recvBytes = ao.WorkingSocket.EndReceive(ar);

        // ���Ź��� �ڷ��� ũ�Ⱑ 1 �̻��� ������ �ڷ� ó��
        if (recvBytes > 0)
        {
            Debug.Log("�޼��� ����: {0}"+Encoding.Unicode.GetString(ao.Buffer));

            // �ڷ� ó���� ��������~
            // ���� �ٽ� �����͸� ���Źޱ� ���ؼ� ���� ��⸦ �ؾ� �մϴ�.
            // Begin~~ �޼��带 �̿��� �񵿱������� �۾��� ����ߴٸ�
            // �ݵ�� �븮�� �Լ����� End~~ �޼��带 �̿��� �񵿱� �۾��� �����ٰ� �˷���� �մϴ�!
            ao.WorkingSocket.BeginReceive(ao.Buffer, 0, ao.Buffer.Length, SocketFlags.None, m_fnReceiveHandler, ao);
            
            if(Encoding.Unicode.GetString(ao.Buffer) == "c")
            {
                pcoffimage.sprite = pconimage;
            }

            else if(Encoding.Unicode.GetString(ao.Buffer) != "c")
            {
                pcoffimage.sprite = rechagePcOffImage;
            }

            else
            {
                pcoffimage.sprite = rechagePcOffImage;
            }
        }
    }
    private void handleDataSend(IAsyncResult ar)
    {

        // �Ѱ��� �߰� ������ �����ɴϴ�.
        AsyncObject ao = (AsyncObject)ar.AsyncState;

        // �ڷḦ �����ϰ�, ������ ����Ʈ�� �����ɴϴ�.
        Int32 sentBytes = ao.WorkingSocket.EndSend(ar);

        if (sentBytes > 0)
            Debug.Log("�޼��� ����: {0}"+Encoding.Unicode.GetString(ao.Buffer));
    }

    public void BtnClik()
    {
        //UInt16.TryParse(input_port.text, out m_Port);
       /* if(m_Port >= 1)
        {
            StartServer(m_Port);
        }*/
        SendMessage(str);
        Debug.Log("���ۿϷ� : " + str);
    }

    private void OnApplicationQuit()
    {
        StopServer();
    }
}

