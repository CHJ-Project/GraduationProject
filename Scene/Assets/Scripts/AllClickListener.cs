using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using common;

public class AllClickListener : MonoBehaviour {

    //网络连接
    NetworkHelper networkHelper = null;

    //判断返回的消息是否改变，改变则操作
    [HideInInspector]
    public string data;
    [HideInInspector]
    public bool isMsgChange = false;
    [HideInInspector]
    public OperationCode code;

    //游戏角色
    public GameObject warrior;
    public GameObject ninja;
    public GameObject swordsman;

    //匹配中面板
    public GameObject machingpanel;

    //匹配成功面板
    public GameObject machingsuccesspanel;

	//通用遮罩
	public GameObject mask;

	//获取Hero相机以及UI相机
	public GameObject heroCamera;
    public GameObject UICamera;

    //获取背景音乐
    private AudioSource bgm;

    //获取提示面板
    public GameObject tipspanel;

    //获取登录注册界面
    public GameObject loginview;

	//获取主界面
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

	//成就面板显示隐藏控制
	public GameObject achievementview;

	//背包面板显示隐藏控制
	public GameObject backpackview;

	//任务面板显示隐藏控制
	public GameObject taskview;

	//匹配对战面板显示隐藏控制
	public GameObject fightview;
	//匹配对战面板
	public GameObject matchingselectview;
	public GameObject manmechineselectview;

	//小游戏选择面板显示隐藏控制
	public GameObject smallgameview;

	//练习选择面板显示隐藏控制
	public GameObject practiceview;

    //英雄选择面板显示隐藏控制
    public GameObject heroselectview;
    //倒计时
    private int surplusTime = 59;
    
    //获取承载数据的脚本
    [HideInInspector]
    public DontDestroyOnLoad dontDestroyOnLoad;

    //获取按钮音频
    public AudioSource btnAudio_normal;
    public AudioSource btnAudio_special;
    public AudioSource btnAudio_back;

	void Start(){
        //获取网络连接
        networkHelper = GameObject.Find("NetworkConnection").GetComponent<NetworkHelper>();
        //将自身赋值给networkHelper
        networkHelper.allClickListener = this;
        //获取DontDestoryOnLoad脚本
        dontDestroyOnLoad = GameObject.Find("DontDestroyOnLoad").GetComponent<DontDestroyOnLoad>();
        dontDestroyOnLoad.SetHero("Warrior");
        //设置武士模型默认显示，忍者、剑士模型默认隐藏
        warrior.SetActive(true);
        ninja.SetActive(false);
        swordsman.SetActive(false);
        //控制提示面板初始隐藏
        tipspanel.SetActive(false);
        //控制匹配中面板初始隐藏
        machingpanel.SetActive(false);
        //控制匹配成功面板初始隐藏
        machingsuccesspanel.SetActive(false);
        //首次进入时，控制登录面板初始显示
        if (networkHelper.GetSelfCode() == "-1")
        {
            loginview.SetActive(true);
        }
        else
        {
            loginview.SetActive(false);
            mainview.transform.Find("topLeft/userInfo/txtName").GetComponent<Text>().text = networkHelper.userName;
            mainview.transform.Find("topCenter/coinBar/txtCoin").GetComponent<Text>().text = networkHelper.coin;
            mainview.transform.Find("topCenter/soulBar/txtSoul").GetComponent<Text>().text = networkHelper.soul;
        }
		//控制主界面初始显示
		mainview.SetActive (true);
		//控制英雄摄像机初始隐藏
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
		//控制成就面板初始隐藏
		achievementview.SetActive(false);
		//控制背包面板初始隐藏
		backpackview.SetActive(false);
		//控制任务面板初始隐藏
		taskview.SetActive(false);
		//控制匹配面板初始隐藏
		fightview.SetActive(false);
		//控制匹配对战里的面板初始隐藏
		matchingselectview.SetActive (false);
		manmechineselectview.SetActive (false);
		//控制小游戏选择面板初始隐藏
		smallgameview.SetActive(false);
        //控制英雄选择面板初始隐藏
        heroselectview.SetActive(false);
        //获取按钮按下音频
        bgm = UICamera.GetComponent<AudioSource>();
	}

    void Update()
    {
        if (isMsgChange)
        {
            string[] datas = data.Split(new char[] { '|' });
            if (code.Equals(OperationCode.registersuccess) || code.Equals(OperationCode.error))
            {
                tipspanel.SetActive(true);
                tipspanel.transform.Find("txtTips").GetComponent<Text>().text = datas[0];
                isMsgChange = false;
            }
            if (code.Equals(OperationCode.loginsuccess))
            {
                isMsgChange = false;
                mainview.transform.Find("topLeft/userInfo/txtName").GetComponent<Text>().text = networkHelper.userName;
                mainview.transform.Find("topCenter/coinBar/txtCoin").GetComponent<Text>().text = networkHelper.coin;
                mainview.transform.Find("topCenter/soulBar/txtSoul").GetComponent<Text>().text = networkHelper.soul;
                loginview.SetActive(false);
            }
            if (code.Equals(OperationCode.setothercode))
            {
                isMsgChange = false;
                machingsuccesspanel.SetActive(true);
            }
            if (code.Equals(OperationCode.setscene))
            {
                isMsgChange = false;
                SceneManager.LoadScene(networkHelper.GetSceneName());
            }
            if (code.Equals(OperationCode.chat))
            {
                isMsgChange = false;
                GameObject chatviewitem = Instantiate(Resources.Load("chatviewitem", typeof(GameObject))) as GameObject;
                Transform chatviewcontent = chatview.transform.Find("listView/content");
                chatviewitem.transform.SetParent(chatviewcontent);
                chatviewitem.transform.Find("txtName").GetComponent<Text>().text = datas[0];
                chatviewitem.transform.Find("imgChatBG/txtContent").GetComponent<Text>().text = datas[1];
                chatviewitem.transform.localScale = Vector3.one;
                LayoutRebuilder.ForceRebuildLayoutImmediate(chatviewcontent.GetComponent<RectTransform>());
                GameObject smallchatviewitem = Instantiate(Resources.Load("smallchatviewitem", typeof(GameObject))) as GameObject;
                Transform smallchatviewcontent = mainview.transform.Find("bottomLeft/smallChatView/content");
                smallchatviewitem.transform.SetParent(smallchatviewcontent);
                smallchatviewitem.transform.Find("area/txtPlayerName").GetComponent<Text>().text = datas[0];
                smallchatviewitem.transform.Find("txtContent").GetComponent<Text>().text = datas[1];
                smallchatviewitem.transform.localScale = Vector3.one;
                LayoutRebuilder.ForceRebuildLayoutImmediate(smallchatviewcontent.GetComponent<RectTransform>());
            }
        }
    }

    //登录按钮
    public void LoginButton(){
        string account = loginview.transform.Find("loginpanel/AccountInputField").GetComponent<InputField>().text;
        string password = loginview.transform.Find("loginpanel/PasswordInputField").GetComponent<InputField>().text;
        if (account == "")
        {
            tipspanel.SetActive(true);
            tipspanel.transform.Find("txtTips").GetComponent<Text>().text = "账号不能为空!";
        }
        else if (password == "")
        {
            tipspanel.SetActive(true);
            tipspanel.transform.Find("txtTips").GetComponent<Text>().text = "密码不能为空!";
        }
        else
        {
            networkHelper.Send(OperationCode.login, account + "|" + password);
        }
    }

    //注册按钮
    public void RegisterButton(){
        if (loginview.transform.Find("registerpanel").gameObject.activeInHierarchy)
        {
            string account = loginview.transform.Find("registerpanel/AccountInputField").GetComponent<InputField>().text;
            string password1 = loginview.transform.Find("registerpanel/PasswordInputField").GetComponent<InputField>().text;
            string password2 = loginview.transform.Find("registerpanel/ConfirmPasswordInputField").GetComponent<InputField>().text;
            if (account == "")
            {
                tipspanel.SetActive(true);
                tipspanel.transform.Find("txtTips").GetComponent<Text>().text = "账号不能为空!";
            }
            else if (password1 == "")
            {
                tipspanel.SetActive(true);
                tipspanel.transform.Find("txtTips").GetComponent<Text>().text = "密码不能为空!";
            }
            else if (password1 != password2)
            {
                tipspanel.SetActive(true);
                tipspanel.transform.Find("txtTips").GetComponent<Text>().text = "两次输入密码不相同!";
            }
            else
            {
                networkHelper.Send(OperationCode.register, account + "|" + password1);
            }
        }
        else
        {
            loginview.transform.Find("loginpanel").gameObject.SetActive(false);
            loginview.transform.Find("registerpanel").gameObject.SetActive(true);
        }
    }

    //注册面板返回登录按钮
    public void BackToLoginButton()
    {
        loginview.transform.Find("loginpanel").gameObject.SetActive(true);
        loginview.transform.Find("registerpanel").gameObject.SetActive(false);
    }

    //提示面板确定按钮
    public void OKButton()
    {
        tipspanel.SetActive(false);
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
        btnAudio_normal.Play();
		settingview.SetActive (true);
	}

	//系统设置返回按钮
	public void SettingviewButtonBack(){
        btnAudio_back.Play();
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

    //聊天内容发送按钮
    public void SendChatContentButton()
    {
        string content = chatview.transform.Find("InputField/Text").GetComponent<Text>().text;
        networkHelper.Send(OperationCode.chat, networkHelper.userName + "|" + content);
    }

	//主界面邮箱按钮
	public void MailviewButton(){
        btnAudio_normal.Play();
		mailview.SetActive (true);
	}

	//邮箱界面返回按钮
	public void MailviewButtonBack(){
        btnAudio_back.Play();
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
        btnAudio_normal.Play();
		friendview.SetActive (true);
	}

	//主界面好友返回按钮
	public void FriendviewButtonBack(){
        btnAudio_back.Play();
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
        btnAudio_normal.Play();
        warrior.SetActive(true);
        ninja.SetActive(false);
        swordsman.SetActive(false);
        dontDestroyOnLoad.SetHero("Warrior");
		heroCamera.SetActive(true);
		mainview.SetActive (false);
		heroview.SetActive (true);
	}

	//英雄返回按钮
	public void HeroviewButtonBack(){
        btnAudio_back.Play();
		mainview.SetActive (true);
		heroCamera.SetActive(false);
		heroview.SetActive (false);
	}

	//主界面商城按钮
	public void ShopviewButton(){
        btnAudio_normal.Play();
		shopview.SetActive (true);
	}

	//商城返回按钮
	public void ShopviewButtonBack(){
        btnAudio_back.Play();
		shopview.SetActive (false);
	}

	//主界面成就按钮
	public void AchievementButton(){
        btnAudio_normal.Play();
		achievementview.SetActive (true);
	}

	//成就返回按钮
	public void AchievementButtonBack(){
        btnAudio_back.Play();
		achievementview.SetActive (false);
	}

	//主界面背包按钮
	public void BackpackviewButton(){
        btnAudio_normal.Play();
		backpackview.SetActive (true);
	}

	//背包返回按钮
	public void BackpackviewButtonBack(){
        btnAudio_back.Play();
		backpackview.SetActive (false);
	}

	//主界面任务按钮
	public void TaskviewButton(){
        btnAudio_normal.Play();
		taskview.SetActive (true);
	}

	//任务返回按钮
	public void TaskviewButtonBack(){
        btnAudio_back.Play();
		taskview.SetActive (false);
	}

	//主界面匹配对战按钮
	public void FightviewButton(){
        btnAudio_special.Play();
		fightview.SetActive (true);
	}

	//匹配对战返回按钮
	public void FightviewButtonBack(){
        btnAudio_back.Play();
		fightview.SetActive (false);
	}

	//选择匹配对战按钮
	public void MatchingselectviewButton(){
        btnAudio_special.Play();
		matchingselectview.SetActive (true);
		fightview.SetActive (false);
	}

	//选择匹配对战返回按钮
	public void MatchingselectviewButtonBack(){
        btnAudio_back.Play();
		fightview.SetActive (true);
		matchingselectview.SetActive (false);
	}

	//选择人机对战按钮
	public void ManmechineselectviewButton(){
        btnAudio_special.Play();
		manmechineselectview.SetActive (true);
		fightview.SetActive (false);
	}

	//选择人机对战返回按钮
	public void ManmechineselectviewButtonButton(){
        btnAudio_back.Play();
		fightview.SetActive (true);
		manmechineselectview.SetActive (false);
	}

	//主界面小游戏选择面板按钮
	public void SmallgameviewButton(){
        btnAudio_special.Play();
		smallgameview.SetActive (true);
	}

	//小游戏选择面板返回按钮
	public void SmallgameviewButtonBack(){
        btnAudio_back.Play();
		smallgameview.SetActive (false);
	}

	//主界面练习选择面板
	public void PracticeviewButton(){
        btnAudio_special.Play();
		practiceview.SetActive (true);
	}

	//练习选择面板返回按钮
	public void PracticeviewButtonBack(){
        btnAudio_back.Play();
		practiceview.SetActive (false);
	}

    //选择英雄按钮
    public void SelectWarrior()
    {
        warrior.SetActive(true);
        ninja.SetActive(false);
        swordsman.SetActive(false);
        dontDestroyOnLoad.SetHero("Warrior");
    }
    public void SelectNinja()
    {
        warrior.SetActive(false);
        ninja.SetActive(true);
        swordsman.SetActive(false);
        dontDestroyOnLoad.SetHero("Ninja");
    }
    public void SelectSwordsman()
    {
        warrior.SetActive(false);
        ninja.SetActive(false);
        swordsman.SetActive(true);
        dontDestroyOnLoad.SetHero("Swordsman");
    }

	///
	///跳转场景按钮事件
	///

	//跳转选择英雄面板
	public void ButtonSelectHero(){
        warrior.SetActive(true);
        ninja.SetActive(false);
        swordsman.SetActive(false);
        dontDestroyOnLoad.SetHero("Warrior");

        surplusTime = 59;
        heroselectview.SetActive(true);
        Reciprocal();
        practiceview.SetActive(false);
        matchingselectview.SetActive(false);
        manmechineselectview.SetActive(false);
        machingpanel.SetActive(false);
        machingsuccesspanel.SetActive(false);
        mainview.SetActive(false);
        heroCamera.SetActive(true);
	}

    //选择英雄面板倒计时
    private void Reciprocal()
    {
        heroselectview.transform.Find("txtSurplusTime").GetComponent<Text>().text = surplusTime.ToString();
        surplusTime--;
        if (surplusTime >= 0)
        {
            Invoke("Reciprocal", 1f);
        }
        else
        {
            selectHero();
        }
    }

    //practice按钮
    public void PracticeButton()
    {
        dontDestroyOnLoad.SetMode("Practice");
        ButtonSelectHero();
    }

    //PVP_1v1按钮
    public void PVP_1v1()
    {
        dontDestroyOnLoad.SetMode("PVP_1v1");
        machingpanel.SetActive(true);
        networkHelper.Send(OperationCode.maching, networkHelper.GetSelfCode());
        mainview.SetActive(true);
        matchingselectview.SetActive(false);
        manmechineselectview.SetActive(false);
        
    }

    //PVE_1v1按钮
    public void PVE_1v1()
    {
        dontDestroyOnLoad.SetMode("PVE_1v1");
        ButtonSelectHero();
    }

    //选择英雄后确定进入游戏按钮
    public void selectHero()
    {
        if (dontDestroyOnLoad.GetMode() == "PVP_1v1")
        {
            networkHelper.Send(OperationCode.sethero, networkHelper.GetGameCode() + "|" + networkHelper.GetSelfCode() + "|" + dontDestroyOnLoad.GetHero() + "|" + networkHelper.userName);
            heroselectview.transform.Find("btnGO").gameObject.SetActive(false);
        }
        if (dontDestroyOnLoad.GetMode() == "PVE_1v1")
        {
            SceneManager.LoadScene("scene_0" + Random.Range(1, 5).ToString());
        }
        if (dontDestroyOnLoad.GetMode() == "Practice")
        {
            SceneManager.LoadScene("practice");
        }
    }

    //修改音效音量大小
    public void SoundEffectOnValueChange()
    {
        btnAudio_back.volume = settingview.transform.Find("SoundEffectSlider").GetComponent<Slider>().value;
        btnAudio_normal.volume = settingview.transform.Find("SoundEffectSlider").GetComponent<Slider>().value;
        btnAudio_special.volume = settingview.transform.Find("SoundEffectSlider").GetComponent<Slider>().value;
    }

    //修改背景音乐音量大小
    public void BGMOnValueChange()
    {
        bgm.volume = settingview.transform.Find("BGMSlider").GetComponent<Slider>().value;
    }
}