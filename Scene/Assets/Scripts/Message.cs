using System;
using System.Collections.Generic;
using UnityEngine;
using common;
using System.Text;
using System.Linq;

public class Message : MonoBehaviour {
    private int startIndex = 0;
    private byte[] data = new byte[1024];

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

    public byte[] PackData(OperationCode code, string data)
    {
        byte[] codeBytes = BitConverter.GetBytes((int)code);
        byte[] dataBytes = Encoding.UTF8.GetBytes(data);
        byte[] lengthBytes = BitConverter.GetBytes(dataBytes.Length);
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
