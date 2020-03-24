using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using common;

public class GameController : MonoBehaviour {

    private GameObject player;
    private GameObject enemy;
    private Animator playerAnim;
    private DontDestroyOnLoad dontDestoryOnLoad;
    private string mode;
    private string playerType = "Warrior";
    private string btnList = "";
    private float time = 0;
    private NetworkHelper networkHelper;
    private SkillController skillController;

	// Use this for initialization
	void Awake() {
        //获取玩家选择的玩法，角色等信息的脚本
        dontDestoryOnLoad = GameObject.Find("DontDestroyOnLoad").GetComponent<DontDestroyOnLoad>();
        mode = dontDestoryOnLoad.GetMode();
        //根据玩家选择的角色加载对应的角色
        if (dontDestoryOnLoad != null)
        {
            if (dontDestoryOnLoad.GetHero() == "Warrior")
            {
                playerType = "Warrior";
                player = Instantiate(Resources.Load("WarriorPlayer", typeof(GameObject))) as GameObject;
            }
            if (dontDestoryOnLoad.GetHero() == "Ninja")
            {
                playerType = "Ninja";
                player = Instantiate(Resources.Load("NinjaPlayer", typeof(GameObject))) as GameObject;
            }
            if (dontDestoryOnLoad.GetHero() == "Swordsman")
            {
                playerType = "Swordsman";
                player = Instantiate(Resources.Load("SwordsmanPlayer", typeof(GameObject))) as GameObject;
            }
        }
        //获取网络连接脚本
        networkHelper = GameObject.Find("NetworkConnection").GetComponent<NetworkHelper>();
        //获取技能字典脚本
        skillController = GameObject.Find("SkillController").GetComponent<SkillController>();
        if (dontDestoryOnLoad != null)
        {
            //练习场景初始化
            if (dontDestoryOnLoad.GetMode() == "Practice")
            {
                player.GetComponent<RoleController>().enabled = false;
                GameObject content = GameObject.Find("content");
                string skillName;
                foreach (string i in skillController.GetKeyList(playerType))
                {
                    GameObject item = Instantiate(Resources.Load("skillitem", typeof(GameObject))) as GameObject;
                    skillController.GetSkillDic(playerType).TryGetValue(i, out skillName);
                    item.transform.Find("txtSkillCode").GetComponent<Text>().text = i;
                    item.transform.Find("txtSkillName").GetComponent<Text>().text = skillName;
                    item.transform.SetParent(content.transform);
                    item.transform.localScale = Vector3.one;
                    LayoutRebuilder.ForceRebuildLayoutImmediate(content.GetComponent<RectTransform>());
                }
            }
            //玩家对战1v1初始化
            if (dontDestoryOnLoad.GetMode() == "PVP_1v1")
            {
                //初始化player以及enemy
                enemy = Instantiate(Resources.Load(networkHelper.enemyType + "Enemy", typeof(GameObject))) as GameObject;
                player.transform.position = new Vector3(networkHelper.player_x, networkHelper.player_y, networkHelper.player_z);
                enemy.transform.position = new Vector3(networkHelper.enemy_x, networkHelper.enemy_y, networkHelper.enemy_z);
                enemy.name = "Enemy";
                player.transform.rotation = Quaternion.LookRotation(enemy.transform.position - player.transform.position, Vector3.up);
                enemy.transform.rotation = Quaternion.LookRotation(player.transform.position - enemy.transform.position, Vector3.up);
                networkHelper.enemy = enemy.transform;
                networkHelper.enemyAnim = enemy.GetComponent<Animator>();
                GameObject.Find("txtPlayerName_1").GetComponent<Text>().text = networkHelper.enemyName;
            }
            //人机对战1v1初始化
            if (dontDestoryOnLoad.GetMode() == "PVE_1v1")
            {
                switch ((int)Random.Range(1, 4))
                {
                    case 1:
                        enemy = Instantiate(Resources.Load("WarriorEnemy", typeof(GameObject))) as GameObject;
                        break;
                    case 2:
                        enemy = Instantiate(Resources.Load("NinjaEnemy", typeof(GameObject))) as GameObject;
                        break;
                    case 3:
                        enemy = Instantiate(Resources.Load("SwordsmanEnemy", typeof(GameObject))) as GameObject;
                        break;
                    default:
                        enemy = GameObject.Find("Enemy");
                        break;
                }
                enemy.AddComponent<AIController>();
                //获取当前场景的名字
                string sceneName = SceneManager.GetActiveScene().name;
                //根据不同的场景控制角色初始化信息
                switch (sceneName)
                {
                    case "scene_01":
                        float random_x = Random.Range(0f, 15.0f);
                        float player_x = -35 - random_x;
                        float enemy_x = -50 + random_x;
                        player.transform.position = new Vector3(player_x, 5.9f, Random.Range(-61.0f, -63.0f));
                        enemy.transform.position = new Vector3(enemy_x, 5.9f, Random.Range(-70.0f, -72.0f));
                        break;
                    case "scene_02":
                        player.transform.position = new Vector3(Random.Range(0f, 14f), 5.9f, Random.Range(0.5f, 3f));
                        enemy.transform.position = new Vector3(Random.Range(-2f, 16f), 5.9f, Random.Range(8f, 10.5f));
                        break;
                    case "scene_03":
                        player.transform.position = new Vector3(Random.Range(-4f, 11f), 5.9f, Random.Range(16f, 18.5f));
                        enemy.transform.position = new Vector3(Random.Range(-6f, 13f), 5.9f, Random.Range(2f, 4.5f));
                        break;
                    case "scene_04":
                        player.transform.position = new Vector3(Random.Range(-9f, 15.5f), 5.9f, Random.Range(24f, 26.5f));
                        enemy.transform.position = new Vector3(Random.Range(-2f, 22f), 5.9f, Random.Range(6f, 8.5f));
                        break;
                    default:
                        break;
                }
                enemy.name = "Enemy";
                player.transform.rotation = Quaternion.LookRotation(enemy.transform.position - player.transform.position, Vector3.up);
                enemy.transform.rotation = Quaternion.LookRotation(player.transform.position - enemy.transform.position, Vector3.up);
            }
            //通用初始化player
            player.name = "Player";
            playerAnim = player.GetComponent<Animator>();
        }
	}

	void LateUpdate () {
        if (btnList != "")
        {
            time += Time.deltaTime;
            if (time >= 2f)
            {
                btnList = "";
                time = 0;
            }
        }
	}

    public void Btn1()
    {
        time = 0;
        btnList += "1";
        IsUseSkill();
    }

    public void Btn2()
    {
        time = 0;
        btnList += "2";
        IsUseSkill();
    }

    public void Btn3()
    {
        time = 0;
        btnList += "3";
        IsUseSkill();
    }

    public void Btn4()
    {
        time = 0;
        btnList += "4";
        IsUseSkill();
    }

    public void Btn5()
    {
        time = 0;
        btnList += "5";
        IsUseSkill();
    }

    public void CancelPrepareSkill()
    {
        time = 0;
        btnList = "";
    }

    void IsUseSkill()
    {
        if (btnList == "4445")
        {
            btnList = "";
            time = 0;
            player.transform.rotation = Quaternion.LookRotation(enemy.transform.position - player.transform.position, Vector3.up);
            playerAnim.SetTrigger("Flash");
            if (mode == "PVP_1v1")
            {
                networkHelper.Send(OperationCode.game, "settrigger|" + networkHelper.GetOtherCode() + "|" + "Flash");
            }
            return;
        }
        if (btnList == "523")
        {
            btnList = "";
            time = 0;
            if (playerAnim.GetCurrentAnimatorClipInfo(0)[0].clip.name.IndexOf("Block") != -1)
            {
                playerAnim.SetBool("BlockIdle", false);
                if (mode == "PVP_1v1")
                {
                    networkHelper.Send(OperationCode.game, "setbool|" + networkHelper.GetOtherCode() + "|BlockIdle|false");
                }
            }
            else
            {
                playerAnim.SetBool("BlockIdle", true);
                if (mode == "PVP_1v1")
                {
                    networkHelper.Send(OperationCode.game, "setbool|" + networkHelper.GetOtherCode() + "|BlockIdle|true");
                }
            }
            return;
        }
        foreach (string i in skillController.GetKeyList(playerType))
        {
            if (i.Equals(btnList))
            {
                UseSkill(btnList);
                btnList = "";
            }
        }
    }

    void UseSkill(string list)
    {
        string skill = skillController.GetSkill(playerType, list);
        playerAnim.SetTrigger(skill);
        if (mode == "PVP_1v1")
        {
            networkHelper.Send(OperationCode.game, "settrigger|" + networkHelper.GetOtherCode() + "|" + skill);
        }
    }

    //练习界面返回按钮
    public void EndPracticeButton()
    {
        SceneManager.LoadScene("Start");
    }
}
