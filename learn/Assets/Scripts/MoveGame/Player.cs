using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    //public GameObject skillsScope;

    private Ray ray;
    private RaycastHit hit;
    private Vector3 stepPos;
    private Quaternion stepRotation;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        /*
        if (Input.GetButtonDown("leftctrl") || Input.GetButtonDown("rightctrl"))
        {
            skillsScope.SetActive(true);
        }
        if (Input.GetButtonUp("leftctrl") || Input.GetButtonUp("rightctrl"))
        {
            skillsScope.SetActive(false);
        }
        */
        if (Input.GetMouseButtonDown(0))
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.gameObject.tag == "NPC" && Vector3.Distance(transform.position,hit.transform.position) < 4.8f)
                {
                    stepPos = (hit.transform.position - transform.position) / 25;
                    transform.LookAt(hit.transform.position);
                    StartCoroutine(PlayerMove(hit.transform.position));
                }
            }
        }
	}

    IEnumerator PlayerMove(Vector3 targetPos)
    {
        int i = 0;
        targetPos.y = transform.position.y;
        while (i < 25)
        {
            transform.position += stepPos;
            i++;
            yield return 0;
        }
    }

	private void OnTriggerEnter(Collider collider)
	{
		if (collider.transform.tag == "NPC") 
		{
			print ("...");
		}
	}

	private void OnTriggerExit(Collider collider)
	{
		if (collider.transform.tag == "NPC") {
			print (",,,");
		}
	}
}
