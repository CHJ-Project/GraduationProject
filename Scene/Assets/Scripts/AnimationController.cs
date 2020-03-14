using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour {

    public MeshCollider[] weaponCollider;               //武器碰撞器
    
    private WeaponTrail myTrail;                        //拖尾效果
    private MeshRenderer bodyTrailRender;                      //身体拖尾
    private BoxCollider leftFootCollider;               //武士左脚碰撞器
    private BoxCollider rightFootCollider;              //武士右脚碰撞器
    private AfterImageEffects[] afterImageEffects = new AfterImageEffects[3];      //残影效果
    private new ParticleSystem particleSystem;              //chargeattk的横扫特效（使用粒子系统制作）
    private float t = 0.033f;
    private float tempT = 0;
    private float animationIncrement = 0.003f;

    void Start()
    {
        //武士初始化
        if (transform.Find("Joint_GRP/Bind_Joint_GRP/Bind_Root/transform1/Bind_Hips/Bind_RightUpLeg/Bind_RightLeg/Bind_RightFoot/RightFootCollider") != null)
        {
            leftFootCollider = transform.Find("Joint_GRP/Bind_Joint_GRP/Bind_Root/transform1/Bind_Hips/Bind_LeftUpLeg/Bind_LeftLeg/Bind_LeftFoot/LeftFootCollider").GetComponent<BoxCollider>();
            rightFootCollider = transform.Find("Joint_GRP/Bind_Joint_GRP/Bind_Root/transform1/Bind_Hips/Bind_RightUpLeg/Bind_RightLeg/Bind_RightFoot/RightFootCollider").GetComponent<BoxCollider>();
            myTrail = transform.Find("Joint_GRP/Bind_Joint_GRP/Bind_Root/Bind_Spine/Bind_Spine1/Bind_Spine2/Bind_RightShoulder/Bind_RightArm/Bind_RightForeArm/Bind_RightHand/joint1/Katana_Left/trail").GetComponent<WeaponTrail>();
            particleSystem = transform.Find("Particle System").GetComponent<ParticleSystem>();
            particleSystem.Stop();
            leftFootCollider.enabled = false;
            rightFootCollider.enabled = false;
        }
        //忍者初始化
        if (transform.Find("Ninja_Mesh"))
        {
            afterImageEffects[0] = transform.Find("Jnt_Root/Jnt_Hips/Jnt_Spine1/Jnt_Spine2/Jnt_Chest/Jnt_L_Shoulder/Jnt_L_Arm/Jnt_L_ForeArm/Jnt_L_ForeArmTwist/Jnt_L_Hand/Sword_Left/Sword_Mesh").GetComponent<AfterImageEffects>();
            afterImageEffects[1] = transform.Find("Jnt_Root/Jnt_Hips/Jnt_Spine1/Jnt_Spine2/Jnt_Chest/Jnt_R_Shoulder/Jnt_R_Arm/Jnt_R_ForeArm/Jnt_R_ForeArmTwist/Jnt_R_Hand/Sword_Right/Sword_Mesh").GetComponent<AfterImageEffects>();
            afterImageEffects[2] = transform.Find("Ninja_Mesh").gameObject.GetComponent<AfterImageEffects>();
        }
        if (myTrail)
        {
            // 默认没有拖尾效果
            myTrail.SetTime(0.0f, 0.0f, 1.0f);
        }
        bodyTrailRender = transform.Find("BodyTrail").GetComponent<MeshRenderer>();
        bodyTrailRender.enabled = false;
        foreach (var i in weaponCollider)
        {
            i.enabled = false;
        }
    }

    void LateUpdate()
    {
        if (!myTrail)
        {
            return;
        }
        t = Mathf.Clamp(Time.deltaTime, 0, 0.066f);

        if (t > 0)
        {
            while (tempT < t)
            {
                tempT += animationIncrement;

                if (myTrail.time > 0)
                {
                    myTrail.Itterate(Time.time - t + tempT);
                }
                else
                {
                    myTrail.ClearTrail();
                }
            }

            tempT -= t;

            if (myTrail.time > 0)
            {
                myTrail.UpdateTrail(Time.time, t);
            }
        }
    }

    //开启碰撞器
    private void OpenCollider()
    {
        foreach (var i in weaponCollider)
        {
            i.enabled = true;
        }
    }

    //关闭碰撞器
    private void CloseCollider()
    {
        foreach (var i in weaponCollider)
        {
            i.enabled = false;
        }
    }

    //动画播放时控制碰撞器和拖尾开启
    public void AttackStart()
    {
        OpenCollider();                                     //动画播放到攻击点时开启碰撞器
        if (myTrail)
        {
            myTrail.SetTime(2.0f, 0.0f, 1.0f);              //设置拖尾时长
            myTrail.StartTrail(0.5f, 0.4f);                 //开始进行拖尾
        }
    }

    //动画播放完时控制碰撞器和拖尾关闭
    public void AttackEnd()
    {
        CloseCollider();                                    //攻击结束后关闭碰撞器
        if (myTrail)
        {
            myTrail.ClearTrail();                           //清除拖尾
        }
        
    }

    //动画播放时控制碰撞器开启
    public void OpenWeaponCollider()
    {
        OpenCollider();                                     //动画播放到攻击点时开启碰撞器
    }

    //动画播放完时控制碰撞器和拖尾关闭
    public void CloseWeaponCollider()
    {
        CloseCollider();                                    //攻击结束后关闭碰撞器
    }

    //动画播放时控制脚部碰撞器开启
    public void OpenFootCollider(int isLeft)
    {
        if (isLeft == 1 && leftFootCollider)
        {
            leftFootCollider.enabled = true;                //动画播放到攻击点时开启碰撞器
        }
        if (isLeft != 1 && rightFootCollider)
        {
            rightFootCollider.enabled = true;               //动画播放到攻击点时开启碰撞器
        }
        
    }

    //动画播放完时控制脚部碰撞器关闭
    public void CloseFootCollider(int isLeft)
    {
        if (isLeft == 1 && leftFootCollider)
        {
            leftFootCollider.enabled = false;               //攻击结束后关闭碰撞器
        }
        if (isLeft != 1 && rightFootCollider)
        {
            rightFootCollider.enabled = false;              //动画播放到攻击点时开启碰撞器
        }
    }

    //控制chargeattk技能的拖尾特效
    public void ChargeAttackEffectStart()
    {
        OpenCollider();                                     //动画播放到攻击点时开启碰撞器
        if (particleSystem)
        {
            particleSystem.Play();
        }
    }

    public void ChargeAttackEffectEnd()
    {
        CloseCollider();                                    //攻击结束后关闭碰撞器
        if (particleSystem)
        {
            particleSystem.Stop();
        }
    }

    //动画播放时控制脚部碰撞器和残影开启
    public void OpenAfterImageEffect()
    {
        OpenCollider();
        if (afterImageEffects.Length > 0)
        {
            foreach (var i in afterImageEffects)
            {
                i._OpenAfterImage = true;
            }
        }
    }

    public void CloseAfterImageEffect()
    {
        CloseCollider();
        if (afterImageEffects.Length > 0)
        {
            foreach (var i in afterImageEffects)
            {
                i._OpenAfterImage = false;
            }
        }
    }

    //瞬移效果
    public void FlashMoveStart()
    {
        bodyTrailRender.enabled = true;
        if (transform.tag == "Player")
        {
            Transform enemy = GameObject.Find("Enemy").transform;
            transform.position = enemy.position + new Vector3(transform.position.x - enemy.position.x, 0, transform.position.z - enemy.position.z).normalized * 1.5f;
        }
        else
        {
            Transform player = GameObject.Find("Player").transform;
            transform.position = player.position + new Vector3(transform.position.x - player.position.x, 0, transform.position.z - player.position.z).normalized * 1.5f;
        }
        Invoke("CloseBodyTrailRender", 0.3f);
    }

    public void CloseBodyTrailRender()
    {
        bodyTrailRender.enabled = false;
    }
}