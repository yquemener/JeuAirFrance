using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TerrainGenerator : MonoBehaviour {

    public GameObject blockPrefab;
    public List<Sprite> tilesTop;
    public List<Sprite> tilesIn;

    public int length = 15;

    private List<GameObject> _generated = new List<GameObject>();

    [ContextMenu("Generate")]
    public void Generate()
    {
        Vector3 v = this.transform.position;
        float width = tilesTop[0].bounds.size.x;
        for (int i = 0; i < length; i++)
        {
            v.x = this.transform.position.x + i * width;
            GameObject newTile = Instantiate(blockPrefab, v, new Quaternion(), this.transform);
            _generated.Add(newTile);
            int ind = (int)(Random.Range(0.0f, 3.999f));
            newTile.GetComponent<SpriteRenderer>().sprite = tilesTop[ind];
        }

        width = tilesIn[0].bounds.size.x;
        float height = tilesIn[0].bounds.size.y;
        for (int y = 0; y < 10; y++)
        {
            for (int x = 7; x < 17; x++)
            {
                v.x = this.transform.position.x + x * width;
                v.y = this.transform.position.y + y * height;
                GameObject newTile = Instantiate(blockPrefab, v, new Quaternion(), this.transform);
                _generated.Add(newTile);
                int ind = (int)(Random.Range(0, tilesIn.Count));
                newTile.GetComponent<SpriteRenderer>().sprite = tilesIn[ind];
            }
        }

    }

    [ContextMenu("Clear")]
    public void Clear()
    {
        foreach(GameObject go in _generated)
        {
            DestroyImmediate(go);
        }

        List<GameObject> todestroy = new List<GameObject>();
        foreach (Transform t in this.transform.GetComponentsInChildren<Transform>())
        {
            if(t.gameObject.name.StartsWith("TerrainBlock"))
            {
                todestroy.Add(t.gameObject);
            }
        }
        foreach(GameObject go in todestroy)
        {
            DestroyImmediate(go);
        }
    }

    // Use this for initialization
    void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
