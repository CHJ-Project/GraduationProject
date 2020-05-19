using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public GameObject[] NPC;
    public int amount = 50;                             //初始化生成NPC的数量

    private int index = -1;                             //随机生成的NPC下标
    private Transform Route;                           //正向路线
    private Transform Anti_Route;                      //反向路线

    void Awake()
    {
        int i = 0;
        int routeNum;
        int currentIndex;
        while (i < amount)
        {
            i++;
            //首先生成NPC
            GameObject newInitNPC = CreateNPC();
            newInitNPC.AddComponent<NPCInit>();
            //随机控制NPC行走的正反路线以及位置
            routeNum = GetRandomNum(0, 2);
            if (routeNum == 0)
            {
                newInitNPC.GetComponent<NPCInit>().direction = true;
                Route = GameObject.Find("Route_" + GetRandomNum(1, 8).ToString()).gameObject.transform;
                newInitNPC.GetComponent<NPCInit>().Route = Route;
                currentIndex = GetRandomNum(0, 15);
                if (currentIndex > 8)
                {
                    currentIndex = 4;
                }
                newInitNPC.GetComponent<NPCInit>().posIndex = currentIndex + 1;
                newInitNPC.transform.position = Route.GetChild(currentIndex).position + (Route.GetChild(currentIndex + 1).position - Route.GetChild(currentIndex).position) * Random.Range(0.0f, 1.0f) * Random.Range(0.5f, 1.0f);
                newInitNPC.transform.LookAt(Route.GetChild(currentIndex + 1).position);
            }
            else
            {
                newInitNPC.GetComponent<NPCInit>().direction = false;
                Route = GameObject.Find("Anti_Route_" + GetRandomNum(1, 8).ToString()).gameObject.transform;
                newInitNPC.GetComponent<NPCInit>().Route = Route;
                currentIndex = GetRandomNum(0, 15);
                if (currentIndex > 8)
                {
                    currentIndex = 4;
                }
                newInitNPC.GetComponent<NPCInit>().posIndex = currentIndex + 1;
                newInitNPC.transform.position = Route.GetChild(currentIndex).position + (Route.GetChild(currentIndex + 1).position - Route.GetChild(currentIndex).position) * Random.Range(0.0f, 1.0f);
                newInitNPC.transform.LookAt(Route.GetChild(currentIndex + 1).position);
            }
        }
    }

	// Use this for initialization
	void Start () {
        StartCoroutine(CreateMoveNPC());
	}

    private int GetRandomNum(int start, int end)
    {
        return Random.Range(start, end);
    }

    private GameObject CreateNPC()
    {
        index = Random.Range(0, 17);
        GameObject newNPC = Instantiate(NPC[index]);
        return newNPC;
    }

    IEnumerator CreateMoveNPC()
    {
        while (true)
        {
            GameObject newNPC = CreateNPC();
            newNPC.AddComponent<NPCMove>();
            yield return new WaitForSeconds(Random.Range(0.5f,2.5f));
        }
    }
}
