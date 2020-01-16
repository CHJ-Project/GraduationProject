using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PublicData : MonoBehaviour {

    //记录选择使用的英雄ID
    private int heroID = -1;

	// Use this for initialization
	void Start () {
        GameObject.DontDestroyOnLoad(gameObject);
	}

    //写入选择的英雄ID
    public void SetHeroID(int heroID)
    {
        this.heroID = heroID;
    }

    //读取选择的英雄ID
    public int GetHeroID()
    {
        return heroID;
    }
}
