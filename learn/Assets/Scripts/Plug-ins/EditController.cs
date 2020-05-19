using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class EditController : MonoBehaviour {

	public AutoAlignment script; 

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Selection.activeGameObject != null) {
			if (Selection.activeGameObject.name != "EditController" && Selection.activeGameObject.GetComponent<AutoAlignment> () == null) {
				Selection.activeGameObject.AddComponent<AutoAlignment> ();
				//print ("...");
			}
		}
	}
}
