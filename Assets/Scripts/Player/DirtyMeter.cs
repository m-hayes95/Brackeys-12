using System;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

namespace Player
{
    public class DirtyMeter : MonoBehaviour
    {
        public delegate void AllMudLost();
        public static event AllMudLost OnGameOver;
        private const int GroundTileType = 1;
        
        [SerializeField] private Tilemap tileMap;
        [SerializeField] private int currentMud; // Serialized for Debugging
        [SerializeField] private TileDataGround tileData;
        [SerializeField, Range(1,100), Tooltip("Set how much mud water will remove when hit.")]
        int waterDamage;

        #region Event Subscription
        private void OnEnable()
        {
            HitBoxListener.OnHitPlayer += HitByWater;
        }

        private void OnDisable()
        {
            HitBoxListener.OnHitPlayer -= HitByWater;
        }
        #endregion

        private void Update()
        {
            FindCurrentTileInfo();
        }
        private void FindCurrentTileInfo()
        {
            Vector3Int currentTileLocation = tileMap.WorldToCell(transform.position);
            TileBase tile = tileMap.GetTile(currentTileLocation);
            if (!tile)
            {
                //Debug.Log("No tiles found");
                return;
            }
            if (tileData.tileType == GroundTileType)
                CollectMudOnTile(currentTileLocation);
        }
        private void CollectMudOnTile(Vector3Int location)
        {
            // Add mud to player
            currentMud++;
            if (currentMud > 100)
                currentMud = 100;
            tileMap.SetTile(location, null);
        }

        private void HitByWater()
        {
            //Debug.Log("Hit by water!");
            currentMud -= waterDamage;
            if (currentMud < 0)
            {
                currentMud = 0;
                OnGameOver?.Invoke(); // No subs atm
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
        public int GetPlayerMudTotal() // For UI and Sprite updates
        {
            return currentMud;
        }
    }
}
