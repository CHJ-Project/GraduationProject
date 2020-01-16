using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCCharacterController : MonoBehaviour {

    public float m_speed = 10.0f;

    private CharacterController m_character;

	// Use this for initialization
	void Start () {
        m_character = GetComponent<CharacterController>();
	}
	
	// Update is called once per frame
	void Update () {
        float horizontal = Input.GetAxis("Horizontal"); //A D 左右
        float vertical = Input.GetAxis("Vertical"); //W S 上 下
 
        m_character.SimpleMove(transform.forward * vertical * m_speed);
        m_character.SimpleMove(transform.right * horizontal * m_speed);
	}
}
