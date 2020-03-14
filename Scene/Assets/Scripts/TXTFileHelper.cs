using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class TXTFileHelper {
	public static void Write<T>(List<T> list,string path){
		FileStream stream = new FileStream (path,FileMode.OpenOrCreate,FileAccess.ReadWrite);
		StreamWriter writer = new StreamWriter (stream);
		stream.SetLength (0);
		foreach (var i in list){
			writer.WriteLine (i);
		}
		writer.Close ();
        stream.Close();
	}
}
