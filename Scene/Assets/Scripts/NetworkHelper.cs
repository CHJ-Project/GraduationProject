using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using common;
using System.Net;
using System.Net.Sockets;
using System;
using System.Text;
using UnityEngine.UI;

public class NetworkHelper : MonoBehaviour {
    [HideInInspector]
    public string userName = "";
    [HideInInspector]
    public string enemyName = "";
    [HideInInspector]
    public string coin = "-1";
    [HideInInspector]
    public string soul = "-1";
    [HideInInspector]
    public float player_x, player_y, player_z;
    [HideInInspector]
    public float enemy_x, enemy_y, enemy_z;
    [HideInInspector]
    public string enemyType;
    [HideInInspector]
    public Transform enemy;
    [HideInInspector]
    public Animator enemyAnim;
    [HideInInspector]
    public AllClickListener allClickListener;

    private Socket clientSocket = null;
    private Message message = null;
    private string selfCode = "-1";
    private string otherCode = "-1";
    private string gameCode = "-1";
    private string sceneName = "";
    [HideInInspector]
    public Vector3 enemyPos = new Vector3(-1, -1, -1);
    [HideInInspector]
    public Quaternion enemyRot = new Quaternion(-1, -1, -1, -1);
    private string setBoolStr;
    private bool setBool;
    private string setTriggerStr;
    private bool isSetBool = false;
    private bool isSetTrigger = false;

	// Use this for initialization
	void Awake () {
        if (GameObject.Find("NetworkConnection"))
        {
            Destroy(this.gameObject);
            return;
        }
        this.name = "NetworkConnection";
        message = new Message();
        clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);         //创建客户端
        clientSocket.Connect(new IPEndPoint(IPAddress.Parse("192.168.1.106"), 88));                             //与服务器建立连接  本机: 127.0.0.1
        clientSocket.BeginReceive(message.Data, message.StartIndex, message.SurplusSize, SocketFlags.None, ReceiveCallBack, clientSocket);
        GameObject.DontDestroyOnLoad(gameObject);
	}

    void Update()
    {
        if (!enemyPos.Equals(new Vector3(-1,-1,-1)))
        {
            enemy.position = enemyPos;
        }
        if (!enemyRot.Equals(new Quaternion(-1,-1,-1,-1)))
        {
            enemy.rotation = enemyRot;
        }
        if (isSetBool)
        {
            isSetBool = false;
            enemyAnim.SetBool(setBoolStr, setBool);
        }
        if (isSetTrigger)
        {
            isSetTrigger = false;
            enemyAnim.SetTrigger(setTriggerStr);
        }
    }

    public void Send(OperationCode operationCode,string msg)
    {
        clientSocket.Send(message.PackData(operationCode, msg));
    }

    //异步接收消息回调函数
    void ReceiveCallBack(IAsyncResult ar)
    {
        Socket clientSocket = null;
        try
        {
            clientSocket = ar.AsyncState as Socket;
            //结束挂起的异步,读取接收到的字节数。
            int count = clientSocket.EndReceive(ar);
            /*
            string msg = Encoding.UTF8.GetString(message.Data, 0, count);
            if (msg != "" && msg != null)
            {
                print("从服务器端接受到的数据为： \n" + msg);
            }
            */
            //解析数据
            AnalyzeData(count);
            //继续异步接收客户端发送的数据
            clientSocket.BeginReceive(message.Data, message.StartIndex, message.SurplusSize, SocketFlags.None, ReceiveCallBack, clientSocket);
        }
        catch (Exception e)
        {
            Debug.Log(e);
            if (clientSocket != null)
            {
                clientSocket.Close();
            }
        }
    }

    //解析数据
    void AnalyzeData(int count)
    {
        message.StartIndex += count;
        while (true)
        {
            if (message.StartIndex <= 4)
            {
                return;
            }
            int dataCount = BitConverter.ToInt32(message.Data, 0);
            if (message.StartIndex >= dataCount + 8)
            {
                OperationCode code = (OperationCode)BitConverter.ToInt32(message.Data, 4);
                string dataStr = Encoding.UTF8.GetString(message.Data, 8, dataCount);
                OnReceiveCallBack(code, dataStr);
                Array.Copy(message.Data, dataCount + 8, message.Data, 0, message.Data.Length - 8 - dataCount);
                message.StartIndex = message.StartIndex - dataCount - 8;
            }
            else
            {
                return;
            }
        }
    }

    void OnReceiveCallBack(OperationCode operationCode, string data)
    {
        switch (operationCode)
        {
            case OperationCode.registersuccess:
                allClickListener.code = operationCode;
                allClickListener.data = data;
                allClickListener.isMsgChange = true;
                break;
            case OperationCode.loginsuccess:
                userName = data.Split(new char[] { '|' })[0];
                coin = data.Split(new char[] { '|' })[1];
                soul = data.Split(new char[] { '|' })[2];
                allClickListener.code = operationCode;
                allClickListener.isMsgChange = true;
                break;
            case OperationCode.error:
                allClickListener.code = operationCode;
                allClickListener.data = data;
                allClickListener.isMsgChange = true;
                break;
            case OperationCode.setselfcode:
                selfCode = data.Split(new char[] { '|' })[0];
                break;
            case OperationCode.setothercode:
                allClickListener.code = operationCode;
                otherCode = data.Split(new char[] { '|' })[0];
                allClickListener.data = data;
                allClickListener.isMsgChange = true;
                break;
            case OperationCode.setgamecode:
                gameCode = data.Split(new char[] { '|' })[0];
                break;
            case OperationCode.setenemytype:
                enemyType = data;
                break;
            case OperationCode.enemyname:
                enemyName = data;
                break;
            case OperationCode.setscene:
                allClickListener.code = operationCode;
                sceneName = data.Split(new char[] { '|' })[0];
                allClickListener.isMsgChange = true;
                break;
            case OperationCode.setplayerinitialpos:
                player_x = Convert.ToSingle(data.Split(new char[] { '|' })[0]);
                player_y = Convert.ToSingle(data.Split(new char[] { '|' })[1]);
                player_z = Convert.ToSingle(data.Split(new char[] { '|' })[2]);
                break;
            case OperationCode.setenemyinitialpos:
                enemy_x = Convert.ToSingle(data.Split(new char[] { '|' })[0]);
                enemy_y = Convert.ToSingle(data.Split(new char[] { '|' })[1]);
                enemy_z = Convert.ToSingle(data.Split(new char[] { '|' })[2]);
                break;
            case OperationCode.game:
                if (data.Split(new char[] { '|' })[0] == "position")
                {
                    enemyPos = new Vector3(Convert.ToSingle(data.Split(new char[] { '|' })[2]), Convert.ToSingle(data.Split(new char[] { '|' })[3]), Convert.ToSingle(data.Split(new char[] { '|' })[4]));
                }
                if (data.Split(new char[] { '|' })[0] == "rotation")
                {
                    enemyRot = new Quaternion(Convert.ToSingle(data.Split(new char[] { '|' })[2]), Convert.ToSingle(data.Split(new char[] { '|' })[3]), Convert.ToSingle(data.Split(new char[] { '|' })[4]), Convert.ToSingle(data.Split(new char[] { '|' })[5]));
                }
                if (data.Split(new char[] { '|' })[0] == "setbool")
                {
                    isSetBool = true;
                    setBoolStr = data.Split(new char[] { '|' })[2];
                    setBool = data.Split(new char[] { '|' })[3] == "true" ? true : false;
                }
                if (data.Split(new char[] { '|' })[0] == "settrigger")
                {
                    isSetTrigger = true;
                    setTriggerStr = data.Split(new char[] { '|' })[2];
                }
                break;
            case OperationCode.chat:
                allClickListener.code = operationCode;
                allClickListener.data = data;
                allClickListener.isMsgChange = true;
                break;
        }
    }

    //读取自身识别码
    public string GetSelfCode()
    {
        return selfCode;
    }

    //读取其他已连线玩家的识别码
    public string GetOtherCode()
    {
        return otherCode;
    }

    //读取GameCode
    public string GetGameCode()
    {
        return gameCode;
    }

    //读取scene
    public string GetSceneName()
    {
        return sceneName;
    }

    void OnDestroy()
    {
        if (clientSocket != null)
        {
            if (!selfCode.Equals("-1"))
            {
                this.Send(OperationCode.exit, selfCode + "|" + userName);
            }
            clientSocket.Close();
            clientSocket = null;
        }
    }
}