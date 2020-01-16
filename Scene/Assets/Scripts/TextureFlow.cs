using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureFlow : MonoBehaviour {

    Material material = null;
    public float x, y;

	// Use this for initialization
	void Start () {
        material = GetComponent<Renderer>().material;
	}
	
	// Update is called once per frame
	void Update () {
        material.mainTextureOffset += new Vector2(x, y);
	}
}
