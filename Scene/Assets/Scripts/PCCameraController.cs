using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PCCameraController : MonoBehaviour {

	public Transform target;				//摄像机跟随移动的目标
	public float camerarotateSpeed = 3f;			//控制摄像机旋转速度
	public float moveSpeed = 0.5f;			//控制摄像机靠近/远离的速度
	public float playerRotateSpeed = 6f;	//控制玩家旋转速度
	public Material translucent;

    //存储射线碰撞到的物体跟材质
	private Dictionary<GameObject, Material> materialRecorder = new Dictionary<GameObject,Material>();

	//存储的碰撞列表
	private List<GameObject> gameObjectRecorder = new List<GameObject>();

	//射线实时碰撞列表
	private List<GameObject> hitRecorder = new List<GameObject>();

	// Use this for initialization
	void Start () {
        transform.position = target.position + new Vector3(0, 3, -6);
		transform.rotation = Quaternion.Euler (20, 0, 0);
	}
	
	// Update is called once per frame
	void Update () {
		//镜头遮挡物处理
		Vector3 direction = target.position - transform.position;
		RaycastHit[] hit = Physics.RaycastAll(transform.position, direction, Vector3.Distance(target.position, transform.position));
		Debug.DrawLine (transform.position, target.position);
		hitRecorder.Clear ();
		//防止因碰撞到透明墙壁出现的OutOfRange错误
		int z = 0;
		for (int i = 0; i < hit.Length; i++)
		{
			if (hit[i].collider.gameObject.tag != "Player" && hit[i].collider.gameObject.tag != "Wall")
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
		//PC端控制镜头
		/*
		if (Input.GetMouseButton (0)) {
			float mouse_x = Input.GetAxis("Mouse X");
			float mouse_y = -Input.GetAxis ("Mouse Y");
			if (Input.GetMouseButton (0)) {
				transform.RotateAround (target.position, Vector3.up, mouse_x * camerarotateSpeed);
				transform.RotateAround (target.position, transform.right, mouse_y * camerarotateSpeed);
				target.Rotate (0,mouse_x * camerarotateSpeed,0);
			}
		}
		*/
		if (Input.GetAxis ("Mouse ScrollWheel") > 0 && Vector3.Distance(target.position,transform.position) > 10) {
			transform.Translate (Vector3.forward * moveSpeed);
		}
		if (Input.GetAxis ("Mouse ScrollWheel") < 0 && Vector3.Distance(target.position,transform.position) < 20) {
			transform.Translate (Vector3.forward * -1f * moveSpeed);
		}
    }
}