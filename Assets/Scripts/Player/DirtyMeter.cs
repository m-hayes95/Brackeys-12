using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DirtyMeter : MonoBehaviour
{
    private const int GROUND_TILE_TYPE = 1;
    
    [SerializeField] private Tilemap tileMap;
    [SerializeField] private int currentMud;
    [SerializeField] private TileDataGround tileData;
    
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
        if (tileData.tileType == GROUND_TILE_TYPE)
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
    public int GetPlayerMudTotal() // For UI
    {
        return currentMud;
    }
}
