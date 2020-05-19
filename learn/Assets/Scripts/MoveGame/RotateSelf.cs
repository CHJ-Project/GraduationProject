using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateSelf : MonoBehaviour {

    private bool isRotate = false;

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            isRotate = false;
        }
        if (isRotate)
        {
            transform.Rotate(0, -0.25f, 0);
        }
	}

    void EndOfPlay()
    {
        isRotate = true;
        GetComponent<Animator>().enabled = false;
    }
}
