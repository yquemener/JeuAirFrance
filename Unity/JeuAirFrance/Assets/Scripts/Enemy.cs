using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
	public float runSpeed = 2f;		    // The speed the enemy can move at.
	public int HP = 2;					// How many times the enemy can be hit before it dies.
	public Sprite deadEnemy;			// A sprite of the enemy when it's dead.
	public AudioClip[] deathClips;		// An array of audioclips that can play when the enemy dies.
    public float reaction_distance = 15.0f; // Distance at which the enemy reacts and moves toward the player


    private SpriteRenderer ren;			// Reference to the sprite renderer.
	private Transform frontCheck;		// Reference to the position of the gameobject used for checking if something is in front.
	private bool dead = false;			// Whether or not the enemy is dead.
	private Score score;				// Reference to the Score script.
    private GameObject hero;
    private float moveSpeed = 0f;       // The speed the enemy currently moves at.




    void Awake()
	{
		// Setting up the references.
		ren = transform.Find("body").GetComponent<SpriteRenderer>();
		frontCheck = transform.Find("frontCheck").transform;
        hero = Globals.inst().player;
	}

	void FixedUpdate ()
	{
        // if there is a ground tile between the enemy and its frontcheck position, change direction
        if (Physics2D.Linecast(transform.position, frontCheck.position, 1 << LayerMask.NameToLayer("Ground")))
        {
            Flip();
        }

        // if there is a line of sight to the player
        if  ((!Physics2D.Linecast(transform.position, hero.transform.position, 1 << LayerMask.NameToLayer("Ground"))) &&
            ((transform.position - hero.transform.position).magnitude < reaction_distance))   // if the player is close to the enemy
        {
            // Then start running!
            moveSpeed = runSpeed;
            GetComponent<Animator>().SetTrigger("Run");
        }

        // Set the enemy's velocity to moveSpeed in the x direction.
        GetComponent<Rigidbody2D>().velocity = new Vector2(transform.localScale.x * moveSpeed, GetComponent<Rigidbody2D>().velocity.y);


        // If the enemy has zero or fewer hit points and isn't dead yet...
        if (HP <= 0 && !dead)
			// ... call the death function.
			Death ();
	}
	
	public void Hurt()
	{
		// Reduce the number of hit points by one.
		HP--;
	}
	
	void Death()
	{
		// Find all of the sprite renderers on this object and it's children.
		SpriteRenderer[] otherRenderers = GetComponentsInChildren<SpriteRenderer>();

		// Disable all of them sprite renderers.
		foreach(SpriteRenderer s in otherRenderers)
		{
			s.enabled = false;
		}

		// Re-enable the main sprite renderer and set it's sprite to the deadEnemy sprite.
		ren.enabled = true;
		ren.sprite = deadEnemy;

		// Set dead to true.
		dead = true;


		// Find all of the colliders on the gameobject and set them all to be triggers.
		Collider2D[] cols = GetComponents<Collider2D>();
		foreach(Collider2D c in cols)
		{
			c.isTrigger = true;
		}

		// Play a random audioclip from the deathClips array.
		int i = Random.Range(0, deathClips.Length);
		AudioSource.PlayClipAtPoint(deathClips[i], transform.position);
	}


	public void Flip()
	{
		// Multiply the x component of localScale by -1.
		Vector3 enemyScale = transform.localScale;
		enemyScale.x *= -1;
		transform.localScale = enemyScale;
	}
}
