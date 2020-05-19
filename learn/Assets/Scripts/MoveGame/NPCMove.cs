using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMove : MonoBehaviour {

    private Transform startPos;
    private Transform Route;
    private int posIndex = 0;                               //目标位置下标
    private Vector3 lookDirection;                          //当前位置到目标位置的向量
    private string routeName;
    private float moveSpeed;                                //移动速度
    private Animator anim;                                  //获取当前NPC的动画控制器组件

	// Use this for initialization
	void Awake () {
        int routeIndex = Random.Range(1, 3);
        if (routeIndex == 1)
        {
            Route = GameObject.Find("Route_" + Random.Range(1,8).ToString()).gameObject.transform;
        }
        else
        {
            Route = GameObject.Find("Anti_Route_" + Random.Range(1, 8).ToString()).gameObject.transform;
        }
		//获取动画控制器组件
		anim = GetComponent<Animator>();
		//随机设置NPC的移动速度
		anim.speed = Random.Range(1.0f,1.3f);
        startPos = Route.GetChild(0);
        moveSpeed = 1;
        transform.position = startPos.position;
        transform.LookAt(Route.GetChild(1).position);
	}
	
	// Update is called once per frame
	void Update () {
        transform.Translate(Vector3.forward * Time.deltaTime * moveSpeed);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Route.GetChild(posIndex).position - transform.position), Time.deltaTime * 3);
	}

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.transform.parent == Route)
        {
            posIndex++;
            if (posIndex == 10)
            {
                Destroy(this.gameObject);
            }
        }
    }
}