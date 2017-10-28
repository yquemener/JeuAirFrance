using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour
{
	public Rigidbody2D rocket;				// Prefab of the rocket.
	public float speed = 20f;				// The speed the rocket will fire at.


	private PlayerControl playerCtrl;		// Reference to the PlayerControl script.
	private Animator anim;					// Reference to the Animator component.


	void Awake()
	{
		// Setting up the references.
		anim = transform.root.gameObject.GetComponent<Animator>();
		playerCtrl = transform.root.GetComponent<PlayerControl>();
	}


	void Update ()
	{
		// If the fire button is pressed...
		if(Input.GetButtonDown("Fire1"))
		{
			// ... set the animator Shoot trigger parameter and play the audioclip.
			//anim.SetTrigger("Shoot");
			GetComponent<AudioSource>().Play();

            Vector3 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 dir = target - transform.position;
            dir.z = 0;
            dir.Normalize();

            float angle = Mathf.Atan2(dir.y, dir.x) * 180 /3.14159f;
            Rigidbody2D bulletInstance = Instantiate(rocket, transform.position, Quaternion.Euler(new Vector3(0, 0, angle))) as Rigidbody2D;
            bulletInstance.velocity = new Vector2(dir.x * speed, dir.y * speed);
		}
	}
}
