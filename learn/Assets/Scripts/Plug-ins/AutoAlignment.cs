using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class AutoAlignment : MonoBehaviour {

	public bool isEdit = true;
	public bool isCreate = true;

	private float x, y, z;
	private Vector3 originPos;
	private int count = 0;
	private int counter = 0;

	// Use this for initialization
	void Start () {
		//isEdit = GetComponent<ScenePlug_in> ().isEdit;
		originPos = transform.position;
		x = GetComponent<Collider> ().bounds.size.x;
		y = GetComponent<Collider> ().bounds.size.y;
		z = GetComponent<Collider> ().bounds.size.z;
	}
	
	// Update is called once per frame
	void Update () {
		if (Tools.current == Tool.Move) {
			if (Mathf.Abs(transform.position.x - originPos.x) >= x && transform.position.y == originPos.y && transform.position.z == originPos.z) {
				count = (int)Mathf.Abs (transform.position.x - originPos.x);
				print (count);
				if (count > 1) {
					count--;
					counter++;
					//Instantiate (this.gameObject, new Vector3 (originPos.x + counter * x, originPos.y, originPos.z), Quaternion.identity);
				}
			}/* else if (transform.position.x == originPos.x && transform.position.y != originPos.y && transform.position.z == originPos.z) {
				Instantiate(this.gameObject,new Vector3(originPos.x,originPos.y + y,originPos.z),Quaternion.identity);
			} else if (transform.position.x == originPos.x && transform.position.y == originPos.y && transform.position.z != originPos.z) {
				Instantiate(this.gameObject,new Vector3(originPos.x,originPos.y,originPos.z + z),Quaternion.identity);
			}
			isCreate = false;*/
		}
	}
}
