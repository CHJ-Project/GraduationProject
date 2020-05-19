using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInit : MonoBehaviour
{
    public bool direction;                                  //当前NPC移动方向，true为正，falsa为反
    public int posIndex;                                    //目标位置下标
    public Transform Route;

    private Vector3 lookDirection;                          //当前位置到目标位置的向量
    private string routeName;
    

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime);
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