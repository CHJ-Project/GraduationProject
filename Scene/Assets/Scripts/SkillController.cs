using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillController : MonoBehaviour{

    private Dictionary<string, string> warriorSkillDic = new Dictionary<string, string>();
    private Dictionary<string, string> ninjaSkillDic = new Dictionary<string, string>();
    private Dictionary<string, string> swordsmanSkillDic = new Dictionary<string, string>();

    void Awake()
    {
        if (GameObject.Find("SkillController"))
        {
            Destroy(this.gameObject);
            return;
        }
        this.name = "SkillController";
        GameObject.DontDestroyOnLoad(gameObject);

        warriorSkillDic.Add("12", "Dash_Back");
        warriorSkillDic.Add("13", "Dash_Left");
        warriorSkillDic.Add("14", "Dash_Right");
        warriorSkillDic.Add("21", "Dodge_Front");
        warriorSkillDic.Add("22", "Dodge_Back");
        warriorSkillDic.Add("23", "Dodge_Left");
        warriorSkillDic.Add("24", "Dodge_Right");
        warriorSkillDic.Add("321", "LightAttk1");
        warriorSkillDic.Add("322", "LightAttk2");
        warriorSkillDic.Add("323", "LightAttk3");
        warriorSkillDic.Add("324", "LightAttk4");
        warriorSkillDic.Add("331", "MediumAttk1");
        warriorSkillDic.Add("333", "MediumAttk2");
        warriorSkillDic.Add("335", "MediumAttk3");
        warriorSkillDic.Add("341", "StrongAttk1");
        warriorSkillDic.Add("342", "StrongAttk2");
        warriorSkillDic.Add("343", "StrongAttk3");
        warriorSkillDic.Add("344", "StrongAttk4");
        warriorSkillDic.Add("4254", "HardAttk");
        warriorSkillDic.Add("4135", "ChargeAttk");

        ninjaSkillDic.Add("11", "DashForward");
        ninjaSkillDic.Add("12", "DashBackward");
        ninjaSkillDic.Add("13", "DashLeft");
        ninjaSkillDic.Add("14", "DashRight");
        ninjaSkillDic.Add("321", "Attack");
        ninjaSkillDic.Add("325", "RangeAttack");
        ninjaSkillDic.Add("435124", "MoveAttack");

        swordsmanSkillDic.Add("11", "Dash");
        swordsmanSkillDic.Add("12", "Step_Back");
        swordsmanSkillDic.Add("21", "Rolling_Front");
        swordsmanSkillDic.Add("22", "Rolling_Back");
        swordsmanSkillDic.Add("23", "Rolling_Left");
        swordsmanSkillDic.Add("24", "Rolling_Right");
        swordsmanSkillDic.Add("321", "Attack_01");
        swordsmanSkillDic.Add("322", "Attack_02");
        swordsmanSkillDic.Add("323", "Attack_03");
        swordsmanSkillDic.Add("324", "Attack_04");
        swordsmanSkillDic.Add("325", "Attack_05");
        swordsmanSkillDic.Add("331", "Attack_06");
        swordsmanSkillDic.Add("332", "Attack_07");
        swordsmanSkillDic.Add("333", "Attack_11");
        swordsmanSkillDic.Add("334", "Attack_12");
    }

    public string[] GetKeyList(string heroType)
    {
        
        if (heroType == "Warrior")
        {
            string[] keys = new string[warriorSkillDic.Keys.Count];
            warriorSkillDic.Keys.CopyTo(keys, 0);
            return keys;
        }
        else if (heroType == "Ninja")
        {
            string[] keys = new string[ninjaSkillDic.Keys.Count];
            ninjaSkillDic.Keys.CopyTo(keys, 0);
            return keys;
        }
        else
        {
            string[] keys = new string[swordsmanSkillDic.Keys.Count];
            swordsmanSkillDic.Keys.CopyTo(keys, 0);
            return keys;
        }
    }

    public string GetSkill(string heroType, string key)
    {
        string skill;
        if (heroType == "Warrior")
        {
            warriorSkillDic.TryGetValue(key, out skill);
        }
        else if (heroType == "Ninja")
        {
            ninjaSkillDic.TryGetValue(key, out skill);
        }
        else
        {
            swordsmanSkillDic.TryGetValue(key, out skill);
        }
        return skill;
    }

    public Dictionary<string, string> GetSkillDic(string playerType)
    {
        if (playerType == "Warrior")
        {
            return warriorSkillDic;
        }
        else if(playerType == "Ninja")
        {
            return ninjaSkillDic;
        }
        else
        {
            return swordsmanSkillDic;
        }
    }
}
