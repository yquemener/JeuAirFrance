using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healing : MonoBehaviour
{
    public float speed; // Speed for full healing in seconds
    public bool healing = true;

    private GameObject player = null;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            healing = true;
            player = col.gameObject;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            healing = false;
            player = null;
        }
    }

    private void Update()
    {
        if (player != null)
        {
            player.GetComponent<PlayerHealth>().health += Time.deltaTime * 100.0f / speed;
            player.GetComponent<PlayerHealth>().health = Mathf.Min(100.0f, player.GetComponent<PlayerHealth>().health);
            player.GetComponent<PlayerHealth>().UpdateHealthBar();
        }
    }
}
