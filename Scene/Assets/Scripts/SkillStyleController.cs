using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillStyleController : MonoBehaviour {

    private Image imgEvaluate;
    private Image imgEvaluateFill;
    private bool isPractice;
    private string[] gradeList = new string[]{"d" ,"c" ,"b" ,"a" ,"s" ,"ss" ,"sss"};
    private string grade = "d";
    private int gradeIndex;
    private float time = 0;

    void Start()
    {
        isPractice = GameObject.Find("DontDestroyOnLoad").GetComponent<DontDestroyOnLoad>().GetMode() == "Practice";
        if (!isPractice)
        {
            gradeIndex = 0;
            grade = gradeList[0];
            imgEvaluate = GameObject.Find("imgEvaluate").GetComponent<Image>();
            imgEvaluateFill = GameObject.Find("imgEvaluateFill").GetComponent<Image>();
            imgEvaluate.sprite = Resources.Load("evaluate_d",typeof(Sprite)) as Sprite;
            imgEvaluateFill.fillAmount = 0f;
        }
        else
        {
            this.enabled = false;
        }
    }

    void LateUpdate()
    {
        if (!isPractice)
        {
            time += Time.deltaTime;
            if (time >= 0.3f)
            {
                time = 0;
                if (imgEvaluateFill.fillAmount < 0.02f && !grade.Equals("d"))
                {
                    imgEvaluateFill.fillAmount = 1f;
                    grade = gradeList[--gradeIndex];
                    imgEvaluate.sprite = Resources.Load("evaluate_" + grade, typeof(Sprite)) as Sprite;
                }
                else
                {
                    imgEvaluateFill.fillAmount -= 0.02f;
                }
            }
        }
    }

    void ChangeGrade(float changeNum)
    {
        imgEvaluateFill.fillAmount = imgEvaluateFill.fillAmount + changeNum - 1f;
        grade = gradeList[++gradeIndex];
        imgEvaluate.sprite = Resources.Load("evaluate_" + grade, typeof(Sprite)) as Sprite;
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.name == "Enemy" && transform.tag == "PlayerWeapon" && !isPractice)
        {
            if (grade == gradeList[0])
            {
                if (imgEvaluateFill.fillAmount + 0.8f >= 1)
                {
                    ChangeGrade(0.8f);
                }
                else
                {
                    imgEvaluateFill.fillAmount += 0.8f;
                }
            }
            if (grade == gradeList[1])
            {
                if (imgEvaluateFill.fillAmount + 0.6f >= 1)
                {
                    ChangeGrade(0.6f);
                }
                else
                {
                    imgEvaluateFill.fillAmount += 0.6f;
                }
            }
            if (grade == gradeList[2])
            {
                if (imgEvaluateFill.fillAmount + 0.33f >= 1)
                {
                    ChangeGrade(0.33f);
                }
                else
                {
                    imgEvaluateFill.fillAmount += 0.33f;
                }
            }
            if (grade == gradeList[3])
            {
                if (imgEvaluateFill.fillAmount + 0.2f >= 1)
                {
                    ChangeGrade(0.2f);
                }
                else
                {
                    imgEvaluateFill.fillAmount += 0.2f;
                }
            }
            if (grade == gradeList[4])
            {
                if (imgEvaluateFill.fillAmount + 0.1f >= 1)
                {
                    ChangeGrade(0.1f);
                }
                else
                {
                    imgEvaluateFill.fillAmount += 0.1f;
                }
            }
            if (grade == gradeList[5])
            {
                if (imgEvaluateFill.fillAmount + 0.1f >= 1)
                {
                    ChangeGrade(0.1f);
                }
                else
                {
                    imgEvaluateFill.fillAmount += 0.1f;
                }
            }
            if (grade == gradeList[6])
            {
                imgEvaluateFill.fillAmount += 0.05f;
            }
        }
    }
}
