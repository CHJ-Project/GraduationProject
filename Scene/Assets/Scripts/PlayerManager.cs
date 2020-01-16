using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

	//public GameObject hero;
	//public GameObject sword;

	private Animator anim;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();

	}
	
	// Update is called once per frame
	void LateUpdate () {
		/*
		if (anim.GetCurrentAnimatorStateInfo (0).IsName("fight2")) {
			if (anim.GetCurrentAnimatorStateInfo (0).normalizedTime < 1.0f) {
				hero.GetComponent<AfterImageEffects> ().enabled = true;
				sword.GetComponent<AfterImageEffects> ().enabled = true;
			}
			if (anim.GetCurrentAnimatorStateInfo (0).normalizedTime > 1.0f) {
				hero.GetComponent<AfterImageEffects> ().enabled = false;
				sword.GetComponent<AfterImageEffects> ().enabled = false;
			}
		}
		*/
		//控制角色移动
		if (Input.GetKeyDown (KeyCode.W)) {
			anim.SetBool ("walk", true);
		}
		if (Input.GetKeyDown (KeyCode.A)) {
			anim.SetBool ("flash", true);
		}
		if (Input.GetKeyDown (KeyCode.S)) {
			anim.SetBool ("walkbackaway", true);
		}
		if (Input.GetKeyDown (KeyCode.D)) {
			anim.SetBool ("walkright", true);
		}
		if (Input.GetKeyUp (KeyCode.W)) {
			anim.SetBool ("walk", false);
		}
		if (Input.GetKeyUp (KeyCode.A)) {
			anim.SetBool ("flash", false);
		}
		if (Input.GetKeyUp (KeyCode.S)) {
			anim.SetBool ("walkbackaway", false);
		}
		if (Input.GetKeyUp (KeyCode.D)) {
			anim.SetBool ("walkright", false);
		}
		//控制角色战斗
		if (Input.GetKeyDown (KeyCode.J)) {
			anim.SetBool ("fight2", true);
		}
		if (Input.GetKeyDown (KeyCode.K)) {
			anim.SetBool ("fight3", true);
		}
		if (Input.GetKeyDown (KeyCode.L)) {
			anim.SetBool ("fight4", true);
		}
		if (Input.GetKeyDown (KeyCode.U)) {
			anim.SetBool ("fight5", true);
		}
		if (Input.GetKeyDown (KeyCode.I)) {
			anim.SetBool ("fight6", true);
		}
		if (Input.GetKeyDown (KeyCode.O)) {
			anim.SetBool ("fight7", true);
		}
		if (Input.GetKeyUp (KeyCode.J)) {
			anim.SetBool ("fight2", false);
		}
		if (Input.GetKeyUp (KeyCode.K)) {
			anim.SetBool ("fight3", false);
		}
		if (Input.GetKeyUp (KeyCode.L)) {
			anim.SetBool ("fight4", false);
		}
		if (Input.GetKeyUp (KeyCode.U)) {
			anim.SetBool ("fight5", false);
		}
		if (Input.GetKeyUp (KeyCode.I)) {
			anim.SetBool ("fight6", false);
		}
		if (Input.GetKeyUp (KeyCode.O)) {
			anim.SetBool ("fight7", false);
		}
	}
}
