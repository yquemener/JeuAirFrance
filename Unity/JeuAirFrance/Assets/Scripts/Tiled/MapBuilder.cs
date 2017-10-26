using UnityEngine;
using System.Collections;
using Tiled;
using Tiled.Parser;
using System.Linq;
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


        public Map map;
        public Map Map { get { return map; } }

        
        [Tooltip("Path to sprite used to create tiles")]
        public Texture2D tileset;
        public Sprite[] spriteSheet;

        [ContextMenu("Load Tileset")]
        public void LoadTileset()
        {
            string path = AssetDatabase.GetAssetPath(tileset);
            spriteSheet = AssetDatabase.LoadAllAssetsAtPath(path).OfType<Sprite>().ToArray();
        }





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

            if (map == null)
            {
                Debug.Log("Loading map data");
                map = new TiledMapLoader(
                    new JSONMapParser())
                    .Load(mapJson);

                MapContainer.Map = map;
            }

            foreach (Layer layer in map.Layers) {

                Debug.Log("Rendering layer: " + layer.Name);

                int x = 0, y = -map.Height;

                foreach (int d in layer.Data) {

                    if (d != 0) {

                        GameObject prefab = tilePrefab;

                        //  set a custom prefab if tile requires one
                        if (map.ObjectReferences.ContainsKey(d)) {
                            prefab = map.ObjectReferences[d];
                        }

                        GameObject t = (GameObject)GameObject.Instantiate(
                            prefab,
                            new Vector3(x * tileSize.x, -y * tileSize.y, layer.Height),
                            layer.Rotation);

                        t.name = x + ", " + y + ": " + tilePrefab.name;

                        //  only set the custom tile if using the tile prefab
                        if (prefab == tilePrefab) {
                            SpriteRenderer renderer = t.GetComponentInChildren<SpriteRenderer>();
                            if (renderer != null) {
                                renderer.sprite = spriteSheet[d - 1];
                            }
                        }

                        t.transform.parent = this.transform;
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
