using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour {

    public Transform player;

    private Vector3 pos;                     //摄像机到玩家的初始方位

	// Use this for initialization
	void Start () {
        pos = player.position - transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        //transform.position = Vector3.Lerp(transform.position, player.position - pos, 2 * Time.deltaTime);
        transform.position = player.position - pos;
	}
}