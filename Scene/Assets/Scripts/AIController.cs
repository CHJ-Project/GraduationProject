using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour {

    private GameObject player;
    private Animator enemy_anim;
    private AIState state = AIState.Wait;
    private PlayerType playerType;

	void Start () {
        player = GameObject.Find("Player");
        if (transform.Find("Warrior"))
        {
            playerType = PlayerType.Warrior;
        }
        if (transform.Find("Ninja"))
        {
            playerType = PlayerType.Ninja;
        }
        if (transform.Find("Swordsman"))
        {
            playerType = PlayerType.Swordsman;
        }
        enemy_anim = GetComponent<Animator>();
        ChangeState();
	}
	
	void LateUpdate () {
        transform.rotation = Quaternion.LookRotation(player.transform.position - transform.position, Vector3.up);
        /*
        if (Vector3.Distance(transform.position, player.transform.position) > 2.7f)
        {
         * 
        }
        if (Vector3.Distance(transform.position, player.transform.position) > 1.5f && Vector3.Distance(transform.position, player.transform.position) <= 2.7f)
        {
         * 
        }
        if (Vector3.Distance(transform.position, player.transform.position) <= 1.5f)
        {
         * 
        }*/
	}

    string AutoAttackMoveSkill()
    {
        if (Vector3.Distance(transform.position, player.transform.position) > 2.7f)
        {
            return "Flash";
        }
        if (playerType == PlayerType.Warrior)
        {
            return "Dodge_Front";
        }
        if (playerType == PlayerType.Ninja)
        {
            return "DashForward";
        }
        if (playerType == PlayerType.Swordsman)
        {
            switch ((int)Random.Range(1, 3))
            {
                case 1: return "Rolling_Front";
                case 2: return "Dash";
                default: return null;
            }
        }
        return null;
    }

    string AutoDefenseMoveSkill()
    {
        if (playerType == PlayerType.Warrior)
        {
            switch ((int)Random.Range(1, 7))
            {
                case 1: return "Dash_Back";
                case 2: return "Dodge_Back";
                case 3: return "Dash_Left";
                case 4: return "Dodge_Left";
                case 5: return "Dash_Right";
                case 6: return "Dodge_Right";
                default: return null;
            }
        }
        if (playerType == PlayerType.Ninja)
        {
            switch ((int)Random.Range(1, 4))
            {
                case 1: return "DashBackward";
                case 2: return "DashLeft";
                case 3: return "DashRight";
                default: return null;
            }
        }
        if (playerType == PlayerType.Swordsman)
        {
            switch ((int)Random.Range(1, 5))
            {
                case 1: return "Rolling_Back";
                case 2: return "Rolling_Left";
                case 3: return "Rolling_Right";
                case 4: return "Step_Back";
                default: return null;
            }
        }
        return null;
    }

    string AutoAttackSkill()
    {
        if (playerType == PlayerType.Warrior)
        {
            switch ((int)Random.Range(1, 9))
            {
                case 1: return "ChargeAttk";
                case 2: return "HardAttk";
                case 3: return "LightAttk1";
                case 4: return "MediumAttk1";
                case 5: return "StrongAttk1";
                case 6: return "RoundKick";
                case 7: return "SideKick";
                case 8: return "SprintStrongAttk";
                default: return null;
            }
        }
        if (playerType == PlayerType.Ninja)
        {
            switch ((int)Random.Range(1, 4))
            {
                case 1: return "Attack";
                case 2: return "RangeAttack";
                case 3: return "MoveAttack";
                default: return null;
            }
        }
        if (playerType == PlayerType.Swordsman)
        {
            switch ((int)Random.Range(1, 10))
            {
                case 1: return "Attack_01";
                case 2: return "Attack_02";
                case 3: return "Attack_03";
                case 4: return "Attack_04";
                case 5: return "Attack_05";
                case 6: return "Attack_06";
                case 7: return "Attack_07";
                case 8: return "Attack_11";
                case 9: return "Attack_12";
                default: return null;
            }
        }
        return null;
    }

    float RandomWaitSeconds()
    {
        return Random.Range(1f, 4f);
    }

    void ChangeState()
    {
        if (state == AIState.Wait)
        {
            if (Vector3.Distance(transform.position, player.transform.position) > 1.5f)
            {
                state = AIState.AttackMove;
            }
            else
            {
                state = AIState.Attack;
            }
            Invoke("ChangeState", RandomWaitSeconds());
        }
        else if (state == AIState.AttackMove)
        {
            enemy_anim.SetTrigger(AutoAttackMoveSkill());
            state = AIState.Attack;
            Invoke("ChangeState",0.45f);
        }
        else if (state == AIState.DefenseMove)
        {
            enemy_anim.SetTrigger(AutoDefenseMoveSkill());
            state = AIState.Wait;
            Invoke("ChangeState",0.1f);
        }
        else if(state == AIState.Attack){
            enemy_anim.SetTrigger(AutoAttackSkill());
            state = AIState.DefenseMove;
            Invoke("ChangeState", 1.5f);
        }
        else if (state == AIState.Defense)
        {
            enemy_anim.SetBool("BlockIdle", true);
        }
    }

    public enum AIState{ Wait, AttackMove, DefenseMove, Attack , Defense}
    public enum PlayerType { Warrior, Ninja, Swordsman}

    void OnDestroy()
    {
        CancelInvoke();
    }
}
