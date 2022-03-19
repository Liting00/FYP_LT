using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;

    [SerializeField] private int _width, _height;

    [SerializeField] private Tile _titlePrefab;

    [SerializeField] private Transform _cam;

    private void Awake()
    {
        Instance = this;
    }

    //when Start run GenerateGrid
    private void Start()
    {
        //GenerateGrid();
    }

    public void GenerateGrid()
    {
        for (int x = 0; x < _width - 3; x++)
        {
            for (int z = 0; z < _height; z++)
            {
                var spawnedTile = Instantiate(_titlePrefab, new Vector3(x, 0.02f, z), Quaternion.identity);
                spawnedTile.name = $"Tile {x} {z}";

                //even and odd colouring
                var isOffset = (x + z) % 2 == 1;
                spawnedTile.Init(isOffset);
            }
        }
        //move camera (offset)
        //_cam.transform.position = new Vector3((float)_width / 2 - 3.5f, (float)_height / 2 - 0.5f, -10);

        GameManager.Instance.ChangeState(GameState.SpawnNPC);
    }
}
