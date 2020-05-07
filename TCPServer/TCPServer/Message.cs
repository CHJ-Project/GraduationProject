﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using common;
using System.Net.Sockets;

namespace TCPServer
{
    class Message
    {
        int startIndex = 0;
        static byte[] data = new byte[3096];

        public byte[] Data
        {
            get { return data; }
        }

        public int StartIndex
        {
            get { return startIndex; }
            set { startIndex = value; }
        }

        public int SurplusSize
        {
            get { return data.Length - startIndex; }
        }

        public byte[] PackData(OperationCode code,string data)
        {
            byte[] codeBytes = BitConverter.GetBytes((int)code);
            byte[] dataBytes = Encoding.UTF8.GetBytes(data);
            byte[] lengthBytes = BitConverter.GetBytes(dataBytes.Length);
            //将转换好的byte数组以主要数据长度，枚举类型，主要数据顺序连接到一个byte数组中
            return lengthBytes.Concat(codeBytes).ToArray<byte>().Concat(dataBytes).ToArray<byte>();
        }
        #region 解析数据方法
        /*
        public void AnalyzeData(int count)
        {
            startIndex += count;
            while (true)
            {
                if (startIndex <= 4)
                {
                    return;
                }
                int dataCount = BitConverter.ToInt32(data, 0);
                if (startIndex >= dataCount + 8)
                {
                    OperationCode code = (OperationCode)BitConverter.ToInt32(data, 4);
                    string dataStr = Encoding.UTF8.GetString(data, 8, dataCount);
                    Console.WriteLine(dataStr);
                    Array.Copy(data, dataCount + 8, data, 0, data.Length - 8 - dataCount);
                    startIndex = startIndex - dataCount - 8;
                }
                else
                {
                    return;
                }
            }
        }
        */
        #endregion
    }
}
