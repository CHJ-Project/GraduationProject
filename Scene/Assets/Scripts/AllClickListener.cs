using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AllClickListener : MonoBehaviour {

	//通用遮罩
	public GameObject mask;

	//获取主相机
	public Camera mainCamera;

	//获取Hero相机
	public GameObject heroCamera;

	//获取主界面背景以及主界面
	public GameObject mainviewbg;
	public GameObject mainview;

	//主界面左侧好友列表显示隐藏控制
	public GameObject display;
	public GameObject hide;
	public GameObject friendList;
	public RuntimeAnimatorController smallfriendlist_display_controller, smallfriendlist_hide_controller;
	private Animator friendList_anim;

	//id显示隐藏控制
	public GameObject selfID;

	//金币介绍面板显示隐藏控制
	public GameObject coininfo;

	//魂魄介绍面板显示隐藏控制
	public GameObject diamondinfo;

	//系统设置面板显示隐藏控制
	public GameObject settingview;

	//聊天面板显示隐藏控制
	public GameObject chatview;
	public RuntimeAnimatorController chatview_display_controller,chatview_hide_controller;
	private Animator chatview_anim;

	//邮箱面板显示隐藏控制
	public GameObject mailview;
	//写邮件面板显示隐藏控制
	public GameObject mailwrittingview;

	//好友面板显示隐藏控制
	public GameObject friendview;
	//查找好友面板显示隐藏控制
	public GameObject friendfindingview;

	//英雄面板显示隐藏控制
	public GameObject heroview;

	//商城面板显示隐藏控制
	public GameObject shopview;

	//活动面板显示隐藏控制
	public GameObject activityview;

	//成就面板显示隐藏控制
	public GameObject achievementview;

	//背包面板显示隐藏控制
	public GameObject backpackview;

	//任务面板显示隐藏控制
	public GameObject taskview;

	//匹配对战面板显示隐藏控制
	public GameObject fightview;
	//匹配对战三个面板
	public GameObject matchingselectview;
	public GameObject manmechineselectview;
	public GameObject createroomview;

	//小游戏选择面板显示隐藏控制
	public GameObject smallgameview;

	//练习选择面板显示隐藏控制
	public GameObject practiceview;

    //英雄选择面板显示隐藏控制
    public GameObject heroselectview;

    //获取承载场景间数据的游戏对象
    public GameObject publicDataGameObject;
    //获取承载数据的脚本
    private PublicData dontDestoryOnLoad;

	void Start(){
        //获取DontDestoryOnLoad脚本
        dontDestoryOnLoad = publicDataGameObject.GetComponent<PublicData>();
		//控制主界面背景以及主界面初始显示
		mainviewbg.SetActive(true);
		mainview.SetActive (true);
		//控制主相机显示，英雄相机初始隐藏
		mainCamera.enabled = true;
		heroCamera.SetActive(false);
		//控制通用遮罩初始隐藏
		mask.SetActive(false);
		//控制好友列表初始状态
		display.SetActive (true);
		hide.SetActive (false);
		//获取主界面好友的Animator组件
		friendList_anim = friendList.GetComponent<Animator> ();
		//控制id初始状态
		selfID.SetActive(false);
		//控制金币介绍面板初始隐藏
		coininfo.SetActive(false);
		//控制魂魄介绍面板初始隐藏
		diamondinfo.SetActive(false);
		//控制系统设置面板初始隐藏
		settingview.SetActive (false);
		//获取聊天界面的Animator组件
		chatview_anim = chatview.GetComponent<Animator> ();
		//控制邮箱面板初始隐藏
		mailview.SetActive(false);
		//控制写邮件面板初始隐藏
		mailwrittingview.SetActive (false);
		//控制好友面板初始隐藏
		friendview.SetActive(false);
		//控制查找好友面板初始隐藏
		friendfindingview.SetActive(false);
		//控制英雄面板初始隐藏
		heroview.SetActive(false);
		//控制商城面板初始隐藏
		shopview.SetActive(false);
		//控制活动面板初始隐藏
		activityview.SetActive(false);
		//控制成就面板初始隐藏
		achievementview.SetActive(false);
		//控制背包面板初始隐藏
		backpackview.SetActive(false);
		//控制任务面板初始隐藏
		taskview.SetActive(false);
		//控制匹配面板初始隐藏
		fightview.SetActive(false);
		//控制匹配对战里的三个面板初始隐藏
		matchingselectview.SetActive (false);
		manmechineselectview.SetActive (false);
		createroomview.SetActive (false);
		//控制小游戏选择面板初始隐藏
		smallgameview.SetActive(false);
        //控制英雄选择面板初始隐藏
        heroselectview.SetActive(false);
	}

	//好友列表隐藏
	public void MainviewfriendlistHide(){
		display.SetActive (false);
		hide.SetActive (true);
		friendList_anim.runtimeAnimatorController = smallfriendlist_hide_controller;
	}

	//好友列表显示
	public void MainviewfriendlistDisplay(){
		display.SetActive (true);
		hide.SetActive (false);
		friendList_anim.runtimeAnimatorController = smallfriendlist_display_controller;
	}

	//id显示隐藏控制
	public void IDInfo(){
		if (selfID.activeInHierarchy) {
			selfID.SetActive (false);
		} else {
			selfID.SetActive (true);
		}
	}

	//id显示隐藏控制
	public void CoinInfo(){
		if (coininfo.activeInHierarchy) {
			coininfo.SetActive (false);
		} else {
			coininfo.SetActive (true);
		}
	}

	//id显示隐藏控制
	public void DiamondInfo(){
		if (diamondinfo.activeInHierarchy) {
			diamondinfo.SetActive (false);
		} else {
			diamondinfo.SetActive (true);
		}
	}

	//主界面系统设置按钮
	public void SettingviewButton(){
		settingview.SetActive (true);
	}

	//系统设置返回按钮
	public void SettingviewButtonBack(){
		settingview.SetActive (false);
	}

	//主界面聊天按钮
	public void ChatviewButton(){
		chatview.SetActive (true);
		mask.SetActive (true);
		chatview_anim.runtimeAnimatorController = chatview_display_controller;
	}

	//聊天界面返回按钮
	public void ChatviewButtonBack(){
		chatview.SetActive (false);
		mask.SetActive (false);
		chatview_anim.runtimeAnimatorController = chatview_hide_controller;
	}

	//主界面邮箱按钮
	public void MailviewButton(){
		mailview.SetActive (true);
	}

	//邮箱界面返回按钮
	public void MailviewButtonBack(){
		mailview.SetActive (false);
	}

	//写邮件按钮
	public void Mailwrittingview(){
		mask.SetActive (true);
		mailwrittingview.SetActive (true);
	}

	//关闭写邮件按钮
	public void MailwrittingviewBack(){
		mask.SetActive (false);
		mailwrittingview.SetActive (false);
	}

	//主界面好友按钮
	public void FriendviewButton(){
		friendview.SetActive (true);
	}

	//主界面好友按钮
	public void FriendviewButtonBack(){
		friendview.SetActive (false);
	}

	//查找好友界面按钮
	public void Friendfindingview(){
		mask.SetActive (true);
		friendfindingview.SetActive (true);
	}

	//查找好友界面关闭按钮
	public void FriendfindingviewBack(){
		mask.SetActive (false);
		friendfindingview.SetActive (false);
	}

	//主界面英雄按钮
	public void HeroviewButton(){
		heroCamera.SetActive(true);
		mainCamera.enabled = false;
		mainview.SetActive (false);
		mainviewbg.SetActive (false);
		heroview.SetActive (true);
	}

	//英雄返回按钮
	public void HeroviewButtonBack(){
		mainCamera.enabled = true;
		mainview.SetActive (true);
		mainviewbg.SetActive (true);
		heroCamera.SetActive(false);
		heroview.SetActive (false);
	}

	//主界面商城按钮
	public void ShopviewButton(){
		shopview.SetActive (true);
	}

	//商城返回按钮
	public void ShopviewButtonBack(){
		shopview.SetActive (false);
	}

	//主界面活动按钮
	public void ActivityButton(){
		activityview.SetActive (true);
	}

	//活动返回按钮
	public void ActivityButtonBack(){
		activityview.SetActive (false);
	}

	//主界面成就按钮
	public void AchievementButton(){
		achievementview.SetActive (true);
	}

	//成就返回按钮
	public void AchievementButtonBack(){
		achievementview.SetActive (false);
	}

	//主界面背包按钮
	public void BackpackviewButton(){
		backpackview.SetActive (true);
	}

	//背包返回按钮
	public void BackpackviewButtonBack(){
		backpackview.SetActive (false);
	}

	//主界面任务按钮
	public void TaskviewButton(){
		taskview.SetActive (true);
	}

	//任务返回按钮
	public void TaskviewButtonBack(){
		taskview.SetActive (false);
	}

	//主界面匹配对战按钮
	public void FightviewButton(){
		fightview.SetActive (true);
	}

	//匹配对战返回按钮
	public void FightviewButtonBack(){
		fightview.SetActive (false);
	}

	//选择匹配对战按钮
	public void MatchingselectviewButton(){
		matchingselectview.SetActive (true);
		fightview.SetActive (false);
	}

	//选择匹配对战返回按钮
	public void MatchingselectviewButtonBack(){
		fightview.SetActive (true);
		matchingselectview.SetActive (false);
	}

	//选择人机对战按钮
	public void ManmechineselectviewButton(){
		manmechineselectview.SetActive (true);
		fightview.SetActive (false);
	}

	//选择人机对战返回按钮
	public void ManmechineselectviewButtonButton(){
		fightview.SetActive (true);
		manmechineselectview.SetActive (false);
	}

	//创建房间按钮
	public void CreateroomviewButton(){
		createroomview.SetActive (true);
	}

	//创建房间按钮
	public void CreateroomviewButtonBack(){
		createroomview.SetActive (false);
	}

	//主界面小游戏选择面板按钮
	public void SmallgameviewButton(){
		smallgameview.SetActive (true);
	}

	//小游戏选择面板返回按钮
	public void SmallgameviewButtonBack(){
		smallgameview.SetActive (false);
	}

	//主界面练习选择面板
	public void PracticeviewButton(){
		practiceview.SetActive (true);
	}

	//练习选择面板返回按钮
	public void PracticeviewButtonBack(){
		practiceview.SetActive (false);
	}

	///
	///跳转场景按钮事件
	///

	//1v1按钮
	public void Button1v1(){
        heroselectview.SetActive(true);
        matchingselectview.SetActive(false);
        mainviewbg.SetActive(false);
        mainview.SetActive(false);
        heroCamera.SetActive(true);
        mainCamera.enabled = false;
	}

    //选择英雄后确定进入游戏按钮
    public void selectHero()
    {
        int i = Random.Range(1, 5);
        SceneManager.LoadScene("scene_0" + i.ToString());
    }
}