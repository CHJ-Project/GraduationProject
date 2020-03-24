using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using common;

public class CameraController : MonoBehaviour {

	public Material translucent;

    private GameObject player;				//摄像机跟随移动的目标(玩家)
    private Transform enemy;				//敌人
    private float camerarotateSpeed = 6f;	//控制摄像机旋转速度
    //private float moveSpeed = 0.5f;			//控制摄像机靠近/远离的速度
    private bool lockCamera = false;        //控制摄像机锁定敌人
    private bool isCameraFastMove = true;   //控制摄像机移动模式（速动/缓动）
    private bool canCameraTurn = true;      //控制摄像机能否围绕角色转动
    private float xz_distance;              //xz平面摄像机与玩家的距离
    private const float distanceUp = 2.6f;  //摄像机的高度
    private float distance_x;
    private float distance_y;
    private float distance_z;
    private Animator player_anim;
    private NetworkHelper networkHelper;    //获取网络连接
    private string mode;                    //获取模式

    //存储射线碰撞到的物体跟材质
	private Dictionary<GameObject, Material> materialRecorder = new Dictionary<GameObject,Material>();
	//存储的碰撞列表
	private List<GameObject> gameObjectRecorder = new List<GameObject>();
	//射线实时碰撞列表
	private List<GameObject> hitRecorder = new List<GameObject>();

	// Use this for initialization
	void Start () {
        //获取玩家选择的玩法模式
        mode = GameObject.Find("DontDestroyOnLoad").GetComponent<DontDestroyOnLoad>().GetMode();
        //获取player
        player = GameObject.Find("Player");
        //若不是练习模式，则获取enemy
        if (mode != "Practice")
        {
            enemy = GameObject.Find("Enemy").transform;
        }
        //获取网络连接
        networkHelper = GameObject.Find("NetworkConnection").GetComponent<NetworkHelper>();
        //获取玩家的animator组件控制玩家行走动画播放
        player_anim = player.GetComponent<Animator>();
        //初始化摄像机位置
        transform.position = player.transform.Find("CameraPos").position;
        transform.rotation = Quaternion.Euler(new Vector3(22, player.transform.eulerAngles.y, 0));
        //计算在xz平面摄像机与玩家的距离
        xz_distance = Vector3.Distance(player.transform.position, new Vector3(transform.position.x, player.transform.position.y, transform.position.z));
        //记录初始化时摄像机各坐标轴到玩家的偏移
        distance_x = transform.position.x - player.transform.position.x;
        distance_y = transform.position.y - player.transform.position.y;
        distance_z = transform.position.z - player.transform.position.z;
	}
	
	// Update is called once per frame
	void Update () {
		//镜头遮挡物处理
		Vector3 direction = (player.transform.position + Vector3.up) - transform.position;
		RaycastHit[] hit = Physics.RaycastAll(transform.position, direction, Vector3.Distance(player.transform.position, transform.position));
		hitRecorder.Clear ();
        Debug.DrawLine(transform.position, player.transform.position + Vector3.up);
		//防止因碰撞到透明墙壁出现的OutOfRange错误
		int z = 0;
		for (int i = 0; i < hit.Length; i++)
		{
            if (hit[i].collider.gameObject.tag != "Player" && hit[i].collider.gameObject.tag != "Wall" && hit[i].collider.gameObject.tag != "Enemy" && hit[i].collider.gameObject.tag != "EnemyWeapon" && hit[i].collider.gameObject.tag != "PlayerWeapon")
			{
				hitRecorder.Add (hit [i].collider.gameObject);
				translucent.color = new Color (translucent.color.r, translucent.color.g, translucent.color.b, 0.2f / hit.Length);
				if (!gameObjectRecorder.Contains (hit [i].collider.gameObject)) {
					gameObjectRecorder.Add (hitRecorder[i - z].gameObject);
					materialRecorder.Add (hitRecorder[i - z].gameObject, hitRecorder[i - z].gameObject.GetComponent<Renderer> ().material);
					hit [i].collider.gameObject.GetComponent<Renderer> ().material = translucent;
				}
			}
			else {
				z++;
			}
		}
		//存储需要删除的下标
		List<int> delete_index = new List<int>();
		for(int j = 0;j < gameObjectRecorder.Count;j++){
			if (!hitRecorder.Contains(gameObjectRecorder[j])) {
				Material material;
				materialRecorder.TryGetValue (gameObjectRecorder [j],out material);
				gameObjectRecorder [j].GetComponent<Renderer> ().material = material;
				delete_index.Add (j);
			}
		}
		foreach (int k in delete_index) {
			try{
				materialRecorder.Remove (gameObjectRecorder [k]);
				gameObjectRecorder.Remove (gameObjectRecorder [k]);
			}catch(Exception e){
				Debug.LogWarning (e);
			}
		}
	}

	void LateUpdate () {
        if (lockCamera)
        {
            if (isCameraFastMove)
            {
                //镜头速动
                transform.position = player.transform.position + (player.transform.position - enemy.position).normalized * (xz_distance + 2) + new Vector3(0, distanceUp + 0.5f, 0);
            }
            else
            {
                //镜头缓动
                transform.position = Vector3.Lerp(transform.position, player.transform.position + new Vector3(distance_x, distance_y, distance_z), Time.deltaTime * 2f);
            }
            transform.LookAt(enemy.position + Vector3.up);
        }
        else
        {
            if (isCameraFastMove)
            {
                //镜头速动
                transform.position = player.transform.position + new Vector3(distance_x, distance_y, distance_z);
            }
            else
            {
                //镜头缓动
                transform.position = Vector3.Lerp(transform.position, player.transform.position + new Vector3(distance_x, distance_y, distance_z), Time.deltaTime * 2f);
            }
        }
        //鼠标滚轮控制放大缩小镜头
        /*
		if (Input.GetAxis ("Mouse ScrollWheel") > 0 && Vector3.Distance(player.position,transform.position) > 5) {
			transform.Translate (Vector3.forward * moveSpeed);
		}
		if (Input.GetAxis ("Mouse ScrollWheel") < 0 && Vector3.Distance(player.position,transform.position) < 15) {
			transform.Translate (Vector3.forward * -1f * moveSpeed);
		}
        */
        if (mode == "PVP_1v1")
        {
            networkHelper.Send(OperationCode.game, "rotation|" + networkHelper.GetOtherCode() + "|" + player.transform.rotation.x + "|" + player.transform.rotation.y + "|" + player.transform.rotation.z + "|" + player.transform.rotation.w);
        }
    }

    public void CameraTurn()
    {
        //控制镜头
        if (Input.GetMouseButton (0) && canCameraTurn) {
            float mouse_x = Input.GetAxis("Mouse X");
            float mouse_y = -Input.GetAxis ("Mouse Y");
            transform.RotateAround (player.transform.position + Vector3.up, Vector3.up, mouse_x * camerarotateSpeed);
            transform.RotateAround(player.transform.position + Vector3.up, transform.right, mouse_y * camerarotateSpeed);
            //旋转镜头时重新记录摄像机各坐标轴到玩家的偏移
            distance_x = transform.position.x - player.transform.position.x;
            distance_y = transform.position.y - player.transform.position.y;
            distance_z = transform.position.z - player.transform.position.z;
        }
    }

    public void Btn_LockCamera()
    {
        if (lockCamera)
        {
            lockCamera = false;
            canCameraTurn = true;
            transform.position = player.transform.position + new Vector3(distance_x, distance_y, distance_z);
            transform.rotation = Quaternion.LookRotation(player.transform.position - transform.position + Vector3.up * 1.4f, Vector3.up);
        }
        else
        {
            lockCamera = true;
            canCameraTurn = false;
        }
    }

    //玩家移动时播放跑步动画
    public void MoveStart()
    {
        player_anim.SetBool("Run", true);
        if (mode == "PVP_1v1")
        {
            networkHelper.Send(OperationCode.game, "setbool|" + networkHelper.GetOtherCode() + "|" + "Run|true");
        }
    }

    public void MoveEnd()
    {
        player_anim.SetBool("Run", false);
        if (mode == "PVP_1v1")
        {
            networkHelper.Send(OperationCode.game, "setbool|" + networkHelper.GetOtherCode() + "|" + "Run|false");
        }
    }
}