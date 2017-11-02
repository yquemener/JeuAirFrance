using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// On vous a dit que c'est une mauvaise pratique mais tous les projets en ont. Deal with it

public class Globals : MonoBehaviour {

    public GameObject player;

    private static Globals s_instance = null;
    public static Globals inst()
    {
        if (s_instance == null)
            s_instance = GameObject.Find("Globals").GetComponent<Globals>();
        return s_instance;
    }
}
