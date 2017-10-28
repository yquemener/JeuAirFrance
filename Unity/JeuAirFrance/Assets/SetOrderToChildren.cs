using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

public class SetOrderToChildren : MonoBehaviour {

    public int NewOrder = 0;
    public string SortingLayer;


    [ContextMenu("Set new order to all children")]
    public void SetNewOrder()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.GetComponent<SpriteRenderer>().sortingOrder = NewOrder;
            child.gameObject.GetComponent<SpriteRenderer>().sortingLayerName = SortingLayer;
        }
    }

    [ContextMenu("Remove Colliders")]
    public void RemoveColliders()
    {
        foreach (Transform child in transform)
        {
            foreach (Collider2D comp in child.gameObject.GetComponents<Collider2D>())
            {
                DestroyImmediate(comp);
            }
        }
    }
}
