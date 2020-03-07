using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using MySql.Data.MySqlClient;
using common;
using System.Collections.Generic;

namespace TCPServer
{
    class Program
    {
        static Socket serverSocket = null;
        static Message message = new Message();
        static Dictionary<string,Socket> clientDictionary = new Dictionary<string,Socket>();
        static int codeIndex = 0;
        static int gameCodeIndex = 0;
        static List<string> machingList = new List<string>();                                       //匹配队列
        static Dictionary<string, PlayerInfo> gameList = new Dictionary<string, PlayerInfo>();

        static void Main(string[] args)
        {
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            ///申请id    127.0.0.1  本机
            //IPAddress ip = new IPAddress(new byte[] { 127, 0, 0, 1 });
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
            //申请端口号
            IPEndPoint endPoint = new IPEndPoint(ipAddress, 88);
            //绑定ip和端口号
            serverSocket.Bind(endPoint);
            //开始监听端口号（0 表示没有数量限制）
            serverSocket.Listen(0);
            //同步接收一个客户端的连接
            //Socket clientSocket = serverSocket.Accept();
            //异步接收客户端连接
            serverSocket.BeginAccept(AcceptCallBack, null);
            Console.ReadKey();
        }

        //异步接收消息回调函数
        static void ReceiveCallBack(IAsyncResult ar)
        {
            Socket clientSocket = null;
            try
            {
                clientSocket = ar.AsyncState as Socket;
                /*
                //结束挂起的异步,读取接收到的字节数。
                int count = clientSocket.EndReceive(ar);
                //将接收到的数据转换为字符串
                string msg = Encoding.UTF8.GetString(message.Data, 0, count);
                Console.WriteLine("从客户端接受到的数据为： " + msg);
                */
                int count = clientSocket.EndReceive(ar);

                string msg = Encoding.UTF8.GetString(message.Data, 0, count);
                
                if (msg != "" && msg != null)
                {
                    Console.WriteLine("从客户端接受到的数据为： \n" + msg);
                }
                
                AnalyzeData(count,clientSocket);
                
                //继续异步接收客户端发送的数据
                clientSocket.BeginReceive(message.Data, message.StartIndex,message.SurplusSize, SocketFlags.None, ReceiveCallBack, clientSocket);
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                if (clientSocket != null)
                {
                    clientSocket.Close();
                }
            }
        }

        //异步接收客户端连接回调函数
        static void AcceptCallBack(IAsyncResult ar)
        {
            Socket clientSocket = serverSocket.EndAccept(ar);
            /*
            //向客户端发送消息
            string msg = "Hello client ！你好。。";
            //转成字符串数组进行传递
            clientSocket.Send(System.Text.Encoding.UTF8.GetBytes(msg));
            */
            //异步接收客户端发送的数据
            clientSocket.BeginReceive(message.Data, message.StartIndex, message.SurplusSize, SocketFlags.None, ReceiveCallBack, clientSocket);
            //继续异步接收客户端连接
            serverSocket.BeginAccept(AcceptCallBack, null);
        }

        //解析数据
        static void AnalyzeData(int count,Socket clientSocket)
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
                    OnReceiveCallBack(code, dataStr, clientSocket);
                    Array.Copy(message.Data, dataCount + 8, message.Data, 0, message.Data.Length - 8 - dataCount);
                    message.StartIndex = message.StartIndex - dataCount - 8;
                }
                else
                {
                    return;
                }
            }
        }

        //数据库操作（需要先添加Mysql.Data.dll到项目引用中）
        static void OnDatabaseCallBack(OperationCode code,string commandStr,Socket clientSocket)
        {
            MySqlConnection sqlConnection = new MySqlConnection("Database = mygamedb;Data Source = 127.0.0.1;port = 3306;user ID = root;password = chj123;");
            //打开数据库连接
            sqlConnection.Open();
            MySqlCommand cmd = null;
            switch (code)
            {
                case OperationCode.exit:
                    try
                    {
                        cmd = new MySqlCommand("update user set isLogin = 0 where username = @username;", sqlConnection);
                        cmd.Parameters.AddWithValue("username", commandStr);
                        cmd.ExecuteNonQuery();
                        Console.WriteLine("设置登录状态成功！");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("设置登录状态失败！");
                        Console.WriteLine(e);
                        Console.WriteLine();
                        Console.WriteLine();
                        Console.WriteLine();
                    }
                    break;
                case OperationCode.register:
                    try
                    {
                        cmd = new MySqlCommand("insert into user set username = @username ,password = @password;", sqlConnection);
                        cmd.Parameters.AddWithValue("username", commandStr.Split('|')[0]);
                        cmd.Parameters.AddWithValue("password", commandStr.Split('|')[1]);
                        if (cmd.ExecuteNonQuery() == 1)
                        {
                            clientSocket.Send(message.PackData(OperationCode.registersuccess, "注册成功"));
                            Console.WriteLine("注册成功！");
                        }
                    }
                    catch(Exception e)
                    {
                        clientSocket.Send(message.PackData(OperationCode.error, "注册失败，账号已存在！"));
                        Console.WriteLine("注册失败，账号已存在！");
                        Console.WriteLine(e);
                        Console.WriteLine();
                        Console.WriteLine();
                        Console.WriteLine();
                    }
                    break;
                case OperationCode.login:
                    try
                    {
                        cmd = new MySqlCommand("select username from user where username = @username", sqlConnection);
                        cmd.Parameters.AddWithValue("username", commandStr.Split('|')[0]);
                        MySqlDataReader nameReader = cmd.ExecuteReader();
                        if (nameReader.Read())
                        {
                            nameReader.Close();
                            cmd = new MySqlCommand("select username,password,coin,soul,isLogin from user where username = @username and password = @password", sqlConnection);
                            cmd.Parameters.AddWithValue("username", commandStr.Split('|')[0]);
                            cmd.Parameters.AddWithValue("password", commandStr.Split('|')[1]);
                            MySqlDataReader reader = cmd.ExecuteReader();
                            if (reader.Read())
                            {
                                if (Convert.ToInt32(reader["isLogin"]) == 1)
                                {
                                    Console.WriteLine("该账号已登录!");
                                    clientSocket.Send(message.PackData(OperationCode.error, "该账号已登录!"));
                                    nameReader.Close();
                                    reader.Close();
                                    return;
                                }
                                string name = reader["username"].ToString();
                                string coin = reader["coin"].ToString();
                                string soul = reader["soul"].ToString();
                                clientSocket.Send(message.PackData(OperationCode.setselfcode, codeIndex.ToString()));
                                clientSocket.Send(message.PackData(OperationCode.loginsuccess, name + "|" + coin + "|" + soul));
                                clientDictionary.Add(codeIndex.ToString(), clientSocket);
                                codeIndex++;
                                nameReader.Close();
                                reader.Close();
                                //登录后将该账号的isLogin状态设置为1
                                cmd = new MySqlCommand("update user set isLogin = 1 where username = @username;", sqlConnection);
                                cmd.Parameters.AddWithValue("username", name);
                                cmd.ExecuteNonQuery();
                                Console.WriteLine("登陆成功！");
                            }
                            else
                            {
                                clientSocket.Send(message.PackData(OperationCode.error, "用户名或密码错误！"));
                                Console.WriteLine("用户名或密码错误！");
                                nameReader.Close();
                                reader.Close();
                            }
                        }
                        else
                        {
                            clientSocket.Send(message.PackData(OperationCode.error, "查无此号！"));
                            Console.WriteLine("查无此号！");
                            nameReader.Close();
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        Console.WriteLine();
                        Console.WriteLine();
                        Console.WriteLine();
                    }
                    break;
            }
            #region 增加数据
            //增加数据
            /*
            string username = Console.ReadLine();
            string password = Console.ReadLine();
            MySqlCommand cmd = new MySqlCommand("insert into user set username = @username ,password = @password;", sqlConnection);
            cmd.Parameters.AddWithValue("username", username);
            cmd.Parameters.AddWithValue("password", password);
            cmd.ExecuteNonQuery();
            */
            #endregion
            #region 删除数据
            //删除数据
            /*
            int id = int.Parse(Console.ReadLine());
            MySqlCommand cmd = new MySqlCommand("delete from user where id = @id;", sqlConnection);
            cmd.Parameters.AddWithValue("id", id);
            cmd.ExecuteNonQuery();
            */
            #endregion
            #region 修改数据
            //修改数据
            /*
            string password = Console.ReadLine();
            MySqlCommand cmd = new MySqlCommand("update user set password = @password where id = 4;", sqlConnection);
            cmd.Parameters.AddWithValue("password", password);
            cmd.ExecuteNonQuery();
            */
            #endregion
            #region 查询数据
            //查询数据
            /*MySqlCommand cmd = new MySqlCommand("select * from user", sqlConnection);
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                int id = reader.GetInt32("id");
                string username = reader.GetString("username");
                string password = reader.GetString("password");
                Console.WriteLine(id + ": " + username + ", " + password);
            }
            reader.Close();
            */
            #endregion
            //关闭数据库连接
            sqlConnection.Close();
        }

        static void OnReceiveCallBack(OperationCode code, string data,Socket clientSocket)
        {
            switch (code)
            {
                case OperationCode.maching:
                    if (machingList.Count > 0)
                    {
                        Socket client1;
                        Socket client2;
                        //获取两个客户端
                        clientDictionary.TryGetValue(machingList[0],out client1);
                        clientDictionary.TryGetValue(data.Split(new char[] { '|' })[0], out client2);
                        //发送gameCode
                        client1.Send(message.PackData(OperationCode.setgamecode, gameCodeIndex.ToString()));
                        client2.Send(message.PackData(OperationCode.setgamecode, gameCodeIndex.ToString()));
                        //分别发送对方的识别码给两个客户端
                        client1.Send(message.PackData(OperationCode.setothercode, data.Split(new char[] { '|' })[0]));
                        client2.Send(message.PackData(OperationCode.setothercode, machingList[0]));
                        //将两个游戏中的客户端加入到正在游戏队列
                        PlayerInfo info1 = new PlayerInfo(machingList[0], data.Split(new char[] { '|' })[0]);
                        gameList.Add(gameCodeIndex.ToString(), info1);
                        gameCodeIndex++;
                        machingList.Remove(machingList[0]);
                    }
                    else
                    {
                        machingList.Add(data.Split(new char[] { '|' })[0]);
                    }
                    break;
                case OperationCode.sethero:
                    PlayerInfo info2 = null;
                    gameList.TryGetValue(data.Split(new char[] { '|' })[0], out info2);
                    if (info2.player1 == "" && info2.player2 == "")
                    {
                        if (data.Split(new char[] { '|' })[1] == info2.code1)
                        {
                            info2.player1 = data.Split(new char[] { '|' })[2];
                        }
                        else
                        {
                            info2.player2 = data.Split(new char[] { '|' })[2];
                        }
                    }
                    else
                    {
                        if (data.Split(new char[] { '|' })[1] == info2.code1)
                        {
                            info2.player1 = data.Split(new char[] { '|' })[2];
                        }
                        else
                        {
                            info2.player2 = data.Split(new char[] { '|' })[2];
                        }
                        Socket client1, client2;
                        clientDictionary.TryGetValue(info2.code1, out client1);
                        clientDictionary.TryGetValue(info2.code2, out client2);
                        Random random = new Random();
                        int r = (int)new Random().Next(1,5);
                        info2.sceneName = "scene_0" + r.ToString();
                        client1.Send(message.PackData(OperationCode.setenemytype, info2.player2));
                        client2.Send(message.PackData(OperationCode.setenemytype, info2.player1));
                        float player1_x, player1_y, player1_z, player2_x, player2_y, player2_z;
                        player1_y = 5.9f;
                        player2_y = 5.9f;
                        if (info2.sceneName == "scene_01")
                        {
                            float random_x = random.Next(0, 1500) / 100f;
                            player1_x = -35 - random_x;
                            player2_x = -50 + random_x;
                            player1_z = random.Next(-6100, -6300) / 100f;
                            player2_z = random.Next(-7000, -7200) / 100f;
                            client1.Send(message.PackData(OperationCode.setplayerinitialpos, player1_x.ToString() + "|" + player1_y.ToString() + "|" + player1_z.ToString()));
                            client1.Send(message.PackData(OperationCode.setenemyinitialpos, player2_x.ToString() + "|" + player2_y.ToString() + "|" + player2_z.ToString()));
                            client2.Send(message.PackData(OperationCode.setplayerinitialpos, player2_x.ToString() + "|" + player2_y.ToString() + "|" + player2_z.ToString()));
                            client2.Send(message.PackData(OperationCode.setenemyinitialpos, player1_x.ToString() + "|" + player1_y.ToString() + "|" + player1_z.ToString()));
                        }
                        else if (info2.sceneName == "scene_02")
                        {
                            player1_x = random.Next(0, 1400) / 100f;
                            player1_z = random.Next(50, 300) / 100f;
                            player2_x = random.Next(-200, 1600) / 100f;
                            player2_z = random.Next(800, 1050) / 100f;
                            client1.Send(message.PackData(OperationCode.setplayerinitialpos, player1_x.ToString() + "|" + player1_y.ToString() + "|" + player1_z.ToString()));
                            client1.Send(message.PackData(OperationCode.setenemyinitialpos, player2_x.ToString() + "|" + player2_y.ToString() + "|" + player2_z.ToString()));
                            client2.Send(message.PackData(OperationCode.setplayerinitialpos, player2_x.ToString() + "|" + player2_y.ToString() + "|" + player2_z.ToString()));
                            client2.Send(message.PackData(OperationCode.setenemyinitialpos, player1_x.ToString() + "|" + player1_y.ToString() + "|" + player1_z.ToString()));
                        }
                        else if (info2.sceneName == "scene_03")
                        {
                            player1_x = random.Next(-400, 1100) / 100f;
                            player1_z = random.Next(1600, 1850) / 100f;
                            player2_x = random.Next(-600, 1300) / 100f;
                            player2_z = random.Next(200, 450) / 100f;
                            client1.Send(message.PackData(OperationCode.setplayerinitialpos, player1_x.ToString() + "|" + player1_y.ToString() + "|" + player1_z.ToString()));
                            client1.Send(message.PackData(OperationCode.setenemyinitialpos, player2_x.ToString() + "|" + player2_y.ToString() + "|" + player2_z.ToString()));
                            client2.Send(message.PackData(OperationCode.setplayerinitialpos, player2_x.ToString() + "|" + player2_y.ToString() + "|" + player2_z.ToString()));
                            client2.Send(message.PackData(OperationCode.setenemyinitialpos, player1_x.ToString() + "|" + player1_y.ToString() + "|" + player1_z.ToString()));
                        }
                        else if (info2.sceneName == "scene_04")
                        {
                            player1_x = random.Next(-900, 1550) / 100f;
                            player1_z = random.Next(2400, 2650) / 100f;
                            player2_x = random.Next(-200, 2200) / 100f;
                            player2_z = random.Next(600, 850) / 100f;
                            client1.Send(message.PackData(OperationCode.setplayerinitialpos, player1_x.ToString() + "|" + player1_y.ToString() + "|" + player1_z.ToString()));
                            client1.Send(message.PackData(OperationCode.setenemyinitialpos, player2_x.ToString() + "|" + player2_y.ToString() + "|" + player2_z.ToString()));
                            client2.Send(message.PackData(OperationCode.setplayerinitialpos, player2_x.ToString() + "|" + player2_y.ToString() + "|" + player2_z.ToString()));
                            client2.Send(message.PackData(OperationCode.setenemyinitialpos, player1_x.ToString() + "|" + player1_y.ToString() + "|" + player1_z.ToString()));
                        }
                        else { }
                        client1.Send(message.PackData(OperationCode.setscene, info2.sceneName));
                        client2.Send(message.PackData(OperationCode.setscene, info2.sceneName));
                    }
                    break;
                case OperationCode.exit:
                    clientDictionary.Remove(data.Split(new char[] { '|' })[0]);
                    OnDatabaseCallBack(code, data.Split(new char[] { '|' })[1],clientSocket);
                    break;
                case OperationCode.register:
                    OnDatabaseCallBack(code, data, clientSocket);
                    break;
                case OperationCode.login:
                    OnDatabaseCallBack(code, data, clientSocket);
                    break;
                case OperationCode.game:
                    Socket client;
                    PlayerInfo info3;
                    gameList.TryGetValue(data.Split(new char[] { '|' })[2], out info3);
                    clientDictionary.TryGetValue(info3.GetOtherCode(data.Split(new char[] { '|' })[1]), out client);
                    client.Send(message.PackData(OperationCode.game, data));
                    break;
            }
        }
    }

    public class PlayerInfo{
        public string code1;
        public string player1 = "";
        public string code2;
        public string player2 = "";
        public string sceneName = "";

        public PlayerInfo(string code1, string code2)
        {
            this.code1 = code1;
            this.code2 = code2;
        }

        public string GetOtherCode(string selfCode)
        {
            return selfCode == code1 ? code2 : code1;
        }
    }
}
