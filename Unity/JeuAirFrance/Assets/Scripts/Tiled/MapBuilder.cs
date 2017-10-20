using UnityEngine;
using System.Collections;
using Tiled;
using Tiled.Parser;
using UnityEditor;


namespace Tiled.Builder {

    [AddComponentMenu("Tiled/MapBuilder")]
    public class MapBuilder : MonoBehaviour {

        [Tooltip("Exported map as .json")]
        public TextAsset mapJson;

        [Tooltip("Distance between tiles")]
        public Vector2 tileSize;

        [Tooltip("Path to sprite used to create tiles")]
        public string spriteResource;

        [Tooltip("Prefab used for individual tiles")]
        public GameObject tilePrefab;

        public Sprite[] spriteSheet;

        public Map map;
        public Map Map { get { return map; } }

        void Start() {
            initialize();
        }

        void Update() {
            initialize();
        }

        [ContextMenu("Create Tiles")]
        private void initialize() {

            if (map == null) {
                Debug.Log("Loading map data");
                map = new TiledMapLoader(
                    new JSONMapParser())
                    .Load(mapJson);

                MapContainer.Map = map;
            }


            createTiles();
        }

        
        private void createTiles() {

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
