using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Player
{
    public class DirtyMeter : MonoBehaviour
    {
        public delegate void AllMudLost();
        public static event AllMudLost OnGameOver;
        
        [SerializeField] private float currentMud; // Serialized for Debugging

        [SerializeField, Range(1, 100), Tooltip("Set how much mud water will remove when hit.")]
        int waterDamage;
        
        private PlayerMudPaintScript playerMudPaintScript;
        private bool canCollectMud = true;

        #region Event Subscription
        private void OnEnable()
        {
            HitBoxListener.OnHitPlayer += HitByWater;
            CollectMudTimer.OnTimeOver += StopMudCollection;
        }

        private void OnDisable()
        {
            HitBoxListener.OnHitPlayer -= HitByWater;
            CollectMudTimer.OnTimeOver -= StopMudCollection;
        }
        #endregion

        private void Start()
        {
            playerMudPaintScript = GetComponent<PlayerMudPaintScript>();
        }

        private void Update()
        {
            if (canCollectMud)
                UpdateCurrentMud();
        }

        private void UpdateCurrentMud()
        {
            currentMud = playerMudPaintScript.GetTotalMud();
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

        private void StopMudCollection()
        {
            canCollectMud = false;
        }
        public float GetPlayerMudTotal() // For UI and Sprite updates
        {
            return currentMud;
        }

        #region Code for tiles (not in use with painting script)
        /*
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
        */
        #endregion
    }
}
