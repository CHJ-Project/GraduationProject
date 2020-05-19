using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class Test : MonoBehaviour {

    public int times = 100;
    public Text text1, text2;
    public Image img1, img2;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void btn1()
    {
        float startTime = Time.realtimeSinceStartup;
        for(int i = 100; i < times; i++)
        {
            img1.sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/coin.png");
        }
        text1.text = "非2N: " + (Time.realtimeSinceStartup - startTime).ToString();
    }

    public void btn2()
    {
        float startTime = Time.realtimeSinceStartup;
        for (int i = 100; i < times; i++)
        {
            img2.sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/coin2.png");
        }
        text2.text = "2N: " + (Time.realtimeSinceStartup - startTime).ToString();
    }
}
