using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DilmerGames.Core.Singletons;
using Unity.Netcode;

public class GridManager : NetworkSingleton<GridManager>
{
    public static GridManager Instance;

    [SerializeField] private int _width, _height;

    [SerializeField] private Tile _tilePrefab;

    [SerializeField] private Transform _cam;

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
        for (int x = 0; x < _width; x++)
        {
            for (int z = 0; z < _height; z++)
            {
                var spawnedTile = Instantiate(_tilePrefab, new Vector3(x, 0f, z), Quaternion.identity);
                spawnedTile.name = $"Tile {x} {z}";

                //even and odd colouring
                var isOffset = (x + z) % 2 == 1;

                spawnedTile.Init(isOffset);
                spawnedTile.GetComponent<NetworkObject>().Spawn();
            }
        }
        //move camera (offset)
        //_cam.transform.position = new Vector3((float)_width / 2 - 3.5f, (float)_height / 2 - 0.5f, -10);
    }
}
