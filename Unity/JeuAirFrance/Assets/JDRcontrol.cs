using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JDRcontrol : MonoBehaviour {

    private Animator anim;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        float speed = 0.01f;

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            anim.SetTrigger("Up");
            anim.ResetTrigger("Stop");
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            anim.SetTrigger("Down");
            anim.ResetTrigger("Stop");
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            anim.SetTrigger("Left");
            anim.ResetTrigger("Stop");
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            anim.SetTrigger("Right");
            anim.ResetTrigger("Stop");
        }

        if (Input.GetKeyUp(KeyCode.UpArrow) ||
            Input.GetKeyUp(KeyCode.DownArrow) ||
            Input.GetKeyUp(KeyCode.LeftArrow) ||
            Input.GetKeyUp(KeyCode.RightArrow))
        {
            if (Input.GetKey(KeyCode.UpArrow) ||
                Input.GetKey(KeyCode.DownArrow) ||
                Input.GetKey(KeyCode.LeftArrow) ||
                Input.GetKey(KeyCode.RightArrow))
            { }
            else
            {
                anim.SetTrigger("Stop");
            }
        }

        if (Input.GetKey(KeyCode.UpArrow))
            transform.Translate(speed * Vector3.up);
        if (Input.GetKey(KeyCode.DownArrow))
            transform.Translate(-speed * Vector3.up);
        if (Input.GetKey(KeyCode.RightArrow))
            transform.Translate(speed * Vector3.right);
        if (Input.GetKey(KeyCode.LeftArrow))
            transform.Translate(speed * Vector3.left);
    }
}
