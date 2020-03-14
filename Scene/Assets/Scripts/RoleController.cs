using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using common;

public class RoleController : MonoBehaviour{

    private new string tag;
    private Image blood;
    private Animator anim;
    private string mode;
    private bool isVictory;

    void Start()
    {
        mode = GameObject.Find("DontDestroyOnLoad").GetComponent<DontDestroyOnLoad>().GetMode();
        anim = GetComponent<Animator>();
        if (transform.name == "Player")
        {
            blood = GameObject.Find("imgPlayerBlood_1").GetComponent<Image>();
            tag = "EnemyWeapon";
        }
        else
        {
            blood = GameObject.Find("imgEnemyBlood").GetComponent<Image>();
            tag = "PlayerWeapon";
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.transform.tag == tag)
        {
            if (blood.fillAmount > 0)
            {
                if (anim.GetCurrentAnimatorClipInfo(0)[0].clip.name == "BlockIdle")
                {
                    anim.SetTrigger("BlockGetHit");
                }
                else
                {
                    blood.fillAmount -= 0.05f;
                    anim.SetTrigger("GetHit");
                }
            }
            else
            {
                anim.SetTrigger("Dead");
                if (mode == "PVE_1v1")
                {
                    Destroy(GameObject.Find("Enemy").GetComponent<AIController>());
                    //玩家胜利
                    if(collider.tag == "PlayerWeapon")
                    {
                        isVictory = true;
                    }
                    //敌人胜利
                    else
                    {
                        isVictory = false;
                    }
                }
                Invoke("End", 1f);
            }
        }
    }

    void End()
    {
        GameObject endview = Instantiate(Resources.Load("endview", typeof(GameObject))) as GameObject;
        endview.transform.SetParent(GameObject.Find("UIROOT").transform);
        endview.transform.localScale = Vector3.one;
        endview.transform.localPosition = Vector3.zero;
        endview.transform.Find("imgBG/btnOK").GetComponent<Button>().onClick.AddListener(BackToMainview);
        if (isVictory)
        {
            endview.transform.Find("imgBG/imgVictory").gameObject.SetActive(true);
        }
        else
        {
            endview.transform.Find("imgBG/imgDefeat").gameObject.SetActive(true);
        }
        if (mode == "PVE_1v1")
        {
            endview.transform.Find("imgBG/reward").gameObject.SetActive(false);
        }
        if (mode == "PVP_1v1")
        {
            NetworkHelper networkHelper = GameObject.Find("NetworkConnection").GetComponent<NetworkHelper>();
            networkHelper.Send(OperationCode.endgame, networkHelper.GetGameCode() + "|" + networkHelper.userName);
            networkHelper.enemyPos = new Vector3(-1, -1, -1);
            networkHelper.enemyRot = new Quaternion(-1, -1, -1, -1);
        }
    }

    void BackToMainview()
    {
        SceneManager.LoadScene("Start");
    }
}