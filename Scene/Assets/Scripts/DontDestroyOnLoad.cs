using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour {

    private string heroName = "Warrior";        //记录选择使用的英雄
    private string mode = "PVE_1v1";            //记录玩家选择的模式

	// Use this for initialization
	void Awake () {
        if (GameObject.Find("DontDestroyOnLoad"))
        {
            Destroy(this.gameObject);
            return;
        }
        this.name = "DontDestroyOnLoad";
        GameObject.DontDestroyOnLoad(gameObject);
	}

    //写入选择的英雄ID
    public void SetHero(string heroName)
    {
        this.heroName = heroName;
    }

    //读取选择的英雄ID
    public string GetHero()
    {
        return heroName;
    }

    //写入选择的模式
    public void SetMode(string mode)
    {
        this.mode = mode;
    }

    //读取选择的模式
    public string GetMode()
    {
        return mode;
    }
}
