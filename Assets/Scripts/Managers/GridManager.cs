using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DilmerGames.Core.Singletons;
using Unity.Netcode;

public class GridManager : NetworkSingleton<GridManager>
{
    public static GridManager Instance;

    [SerializeField] 
    private Tile tilePrefab;

    [SerializeField] private Transform cam;

    private void Awake()
    {
        Instance = this;
    }

    public void GenerateGrid()
    {
        if (NetworkManager.IsServer)
            spawnTiles();

        GameManager.Instance.ChangeState(GameState.SpawnNPC);
    }

    private void spawnTiles()
    {
        //Plane Size is 10 x 10 unit mesh
        float tileSizeInstance, tileSize;

        if (GameSettings.TILE_SIZE > 1f)
        {
            tileSizeInstance = 1f/10;
            tileSize = 1f;
        }
        else
        {
            tileSizeInstance = GameSettings.TILE_SIZE / 10;
            tileSize = GameSettings.TILE_SIZE;
        }
        
        tilePrefab.transform.localScale = new Vector3(tileSizeInstance, tileSizeInstance, tileSizeInstance);
        float x = 0;
        while(x <= GameSettings.WIDTH - 1)
        {
            float z = 0;
            while(z <= GameSettings.HEIGHT - 1)
            {
                var spawnedTile = Instantiate(tilePrefab, new Vector3(x, 0f, z), Quaternion.identity);
                spawnedTile.name = $"Tile {x}, {z}";

                //even and odd colouring
                //var isOffset = (x + z) % 2 == 1;

                //spawnedTile.Init(isOffset);
                spawnedTile.GetComponent<NetworkObject>().Spawn();
                z = z + tileSize;
                z = (float)(Mathf.Round(z * 100) / 100.0);
                if (z > GameSettings.HEIGHT - 1f)
                    break;
            }
            x = x + tileSize;
            x = (float)(Mathf.Round(x * 100) / 100.0);
            //if (x > GameSettings.WIDTH - 1f)
             //   break;
        }
        //move camera (offset)
        float camX = 9f;
        float camY = 12.5f + (1f - GameSettings.TILE_SIZE);
        cam.transform.position = new Vector3(camX, camY, 4.5f);
    }
    public void deSpawnTiles()
    {

    }
}
