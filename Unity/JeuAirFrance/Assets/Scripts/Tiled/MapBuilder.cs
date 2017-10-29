using UnityEngine;
using System.Collections;
using Tiled;
using Tiled.Parser;
using System.Linq;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif


namespace Tiled.Builder {

    [AddComponentMenu("Tiled/MapBuilder")]
    public class MapBuilder : MonoBehaviour {

        [Tooltip("Exported map as .json")]
        public TextAsset mapJson;

        [Tooltip("Distance between tiles")]
        public Vector2 tileSize;

        [Tooltip("Prefab used for individual tiles")]
        public GameObject tilePrefab;

        [Tooltip("Prefab used for layers parent object")]
        public GameObject layerPrefab;


        public Map map;
        public Map Map { get { return map; } }

        
        [Tooltip("Path to sprite used to create tiles")]
        public Texture2D[] tilesets;

        public int[] StartingGID;
        public Sprite[] spriteSheet;

#if UNITY_EDITOR
        [ContextMenu("Load Tileset")]
        public void LoadTileset()
        {
            List<Sprite> spriteList = new List<Sprite>();
            for(int i=0;i<tilesets.Length;i++)
            {
                while(spriteList.Count<StartingGID[i])
                {
                    spriteList.Add(null);
                }
                string path = AssetDatabase.GetAssetPath(tilesets[i]);
                spriteList.AddRange(AssetDatabase.LoadAllAssetsAtPath(path).OfType<Sprite>());
            }

            spriteSheet = spriteList.ToArray();
        }
#endif





        void Start() {
            
        }

        void Update() {
            
        }

        [ContextMenu("Clear Tiles")]
        private void clearTiles()
        {
            int numobjs = transform.childCount;
            for(int i=numobjs-1;i>=0; i--)
            {
                DestroyImmediate(transform.GetChild(i).gameObject);
            }
        }

        [ContextMenu("Create Tiles")]
        private void createTiles() {

            Debug.Log("Loading map data");
            map = new TiledMapLoader(
                new JSONMapParser())
                .Load(mapJson);

            MapContainer.Map = map;

            foreach (Layer layer in map.Layers) {

                Debug.Log("Rendering layer: " + layer.Name);

                GameObject layergo = (GameObject)GameObject.Instantiate(layerPrefab, transform);
                layergo.name = layer.Name;

                int x = 0, y = -map.Height;

                foreach (int d in layer.Data) {

                    if (d != 0) {

                        GameObject prefab = tilePrefab;

                        //  set a custom prefab if tile requires one
                        if (map.ObjectReferences.ContainsKey(d)) {
                            prefab = map.ObjectReferences[d];
                        }

                        if (spriteSheet[d] != null)
                        {

                            GameObject t = (GameObject)GameObject.Instantiate(
                                prefab,
                                Vector3.zero,
                                layer.Rotation,
                                layergo.transform);
                            t.transform.localPosition = new Vector3(x * tileSize.x, -y * tileSize.y, layer.Height);
                            t.name = x + ", " + y + ": " + tilePrefab.name;

                            //  only set the custom tile if using the tile prefab
                            if (prefab == tilePrefab)
                            {
                                SpriteRenderer renderer = t.GetComponentInChildren<SpriteRenderer>();
                                if (renderer != null)
                                {
                                    renderer.sprite = spriteSheet[d];
                                }
                            }
                        }
                    }

                    x++;

                    if (x >= map.Width) {
                        x = 0;
                        y++;
                    }
                }
            }
        }
    }
}
