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

        private bool allowMudCollection = true;

        #region Event Subscriptions
        private void OnEnable()
        {
            HitBoxListener.OnHitPlayer += HitByWater;
            CollectMudTimer.OnTimeOver += DontAllowMudCollection;
        }

        private void OnDisable()
        {
            HitBoxListener.OnHitPlayer -= HitByWater;
            CollectMudTimer.OnTimeOver -= DontAllowMudCollection;
        }
        #endregion

        private void Update()
        {
            if (allowMudCollection)
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
            SoundManager.Instance.PlayMudSquelchSound();
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

        private void DontAllowMudCollection()
        {
            allowMudCollection = false;
        }
        public int GetPlayerMudTotal() // For UI and Sprite updates
        {
            return currentMud;
        }
    }
}
