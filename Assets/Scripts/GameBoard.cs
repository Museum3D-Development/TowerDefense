using System;
using System.Collections.Generic;
using UnityEngine;

public class GameBoard : MonoBehaviour
{
    //[SerializeField]
    //private Transform ground = default;

    [SerializeField]
    private GameTile _tilePrefab = default;

    private Vector2Int _size;

    private GameTile[] _tiles;

    private List<GameTile> _spawnPoints = new List<GameTile>();

    private List<GameTileContent> _updatingContent = new List<GameTileContent>();

    private Queue<GameTile> _searchFrontier = new Queue<GameTile>();

    private GameTileContentFactory _contentFactory;

    public int SpawnPointCount => _spawnPoints.Count;

    public void Initialize(Vector2Int size, GameTileContentFactory contentFactory)
    {
        _size = size;
        _contentFactory = contentFactory;
        //ground.localScale = new Vector3(size.x, size.y, 1f);

        Vector2 offset = new Vector2(
            (size.x - 1) * 0.5f, (size.y - 1) * 0.5f
        );
        _tiles = new GameTile[size.x * size.y];
        for (int i = 0, y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++, i++)
            {
                GameTile tile = _tiles[i] = Instantiate(_tilePrefab);
                tile.transform.SetParent(transform, false);
                tile.transform.localPosition = new Vector3(
                    x - offset.x, 0f, y - offset.y
                );

                if (x > 0)
                {
                    GameTile.MakeEastWestNeighbors(tile, _tiles[i - 1]);
                }
                if (y > 0)
                {
                    GameTile.MakeNorthSouthNeighbors(tile, _tiles[i - size.x]);
                }

                tile.IsAlternative = (x & 1) == 0;
                if ((y & 1) == 0)
                {
                    tile.IsAlternative = !tile.IsAlternative;
                }
            }
        }
        Clear();
    }

    public void Clear()
    {
        foreach (GameTile tile in _tiles)
        {
            tile.Content = _contentFactory.Get(GameTileContentType.Empty);
        }
        _spawnPoints.Clear();
        _updatingContent.Clear();
        //ToggleDestination(tiles[tiles.Length / 2]);
        //ToggleSpawnPoint(tiles[0]);
        BuildDestination(_tiles[_tiles.Length / 2]);
        BuildSpawnPoint(_tiles[149]);
        BuildSpawnPoint(_tiles[14]);
    }

    public void GameUpdate()
    {
        for (int i = 0; i < _updatingContent.Count; i++)
        {
            _updatingContent[i].GameUpdate();
        }
    }

    //public void ToggleDestination(GameTile tile)
    public void Build(GameTile tile, GameTileContentType type)
    {
        //if (tile.Content.Type == GameTileContentType.Destination)
        //{
        //    //tile.Content = contentFactory.Get(GameTileContentType.Empty);
        //    if (!FindPaths())
        //    {
        //        tile.Content =
        //            contentFactory.Get(GameTileContentType.Destination);
        //        FindPaths();
        //    }
        //}
        //else if (tile.Content.Type == GameTileContentType.Empty)
        //{
        //    tile.Content = contentFactory.Get(GameTileContentType.Destination);
        //    FindPaths();
        //}
        switch (type)
        {
            case GameTileContentType.Destination:
                BuildDestination(tile);
                break;
            case GameTileContentType.SpawnPoint:
                BuildSpawnPoint(tile);
                break;
            case GameTileContentType.WallStraight:
                BuildWall(tile, type);
                break;
            case GameTileContentType.WallAngle:
                BuildWall(tile, type);
                break;
            case GameTileContentType.LaserTower:
                BuildTower(tile, type);
                break;
            case GameTileContentType.MortarTower:
                BuildTower(tile, type);
                break;
        }
    }

    //public void ToggleWall(GameTile tile, WallType wallType)
    //{
    //    if (tile.Content.Type == GameTileContentType.Wall)
    //    {
    //        updatingContent.Remove(tile.Content);
    //        if (((Wall)tile.Content).WallType == wallType)
    //        {
    //            tile.Content = contentFactory.Get(GameTileContentType.Empty);
    //            FindPaths();
    //        }
    //        else
    //        {
    //            tile.Content = contentFactory.Get(wallType);
    //            updatingContent.Add(tile.Content);
    //        }
    //    }
    //    else if (tile.Content.Type == GameTileContentType.Empty)
    //    {
    //        tile.Content = contentFactory.Get(wallType);
    //        if (FindPaths())
    //        {
    //            updatingContent.Add(tile.Content);
    //        }
    //        else
    //        {
    //            tile.Content = contentFactory.Get(GameTileContentType.Empty);
    //            FindPaths();
    //        }
    //    }
    //    else if (tile.Content.Type == GameTileContentType.Wall)
    //    {
    //        tile.Content = contentFactory.Get(wallType);
    //        updatingContent.Add(tile.Content);
    //    }
    //}

    private void BuildWall(GameTile tile, GameTileContentType type)
    {
        if (tile.Content.Type != GameTileContentType.Empty)
            return;

        tile.Content = _contentFactory.Get(type);
        if (FindPaths() == false)
        {
            tile.Content = _contentFactory.Get(GameTileContentType.Empty);
            FindPaths();
        }
    }

    private void BuildDestination(GameTile tile)
    {
        if (tile.Content.Type != GameTileContentType.Empty)
            return;

        tile.Content = _contentFactory.Get(GameTileContentType.Destination);
        FindPaths();
    }

    //public void ToggleSpawnPoint(GameTile tile)
    //{
    //    if (tile.Content.Type == GameTileContentType.SpawnPoint)
    //    {
    //        if (spawnPoints.Count > 1)
    //        {
    //            spawnPoints.Remove(tile);
    //            tile.Content = contentFactory.Get(GameTileContentType.Empty);
    //        }
    //    }
    //    else if (tile.Content.Type == GameTileContentType.Empty)
    //    {
    //        tile.Content = contentFactory.Get(GameTileContentType.SpawnPoint);
    //        spawnPoints.Add(tile);
    //    }
    //}

    //public void ToggleTower(GameTile tile, TowerType towerType)
    //{
    //    if (tile.Content.Type == GameTileContentType.Tower)
    //    {
    //        updatingContent.Remove(tile.Content);
    //        if (((Tower)tile.Content).TowerType == towerType)
    //        {
    //            tile.Content = contentFactory.Get(GameTileContentType.Empty);
    //            FindPaths();
    //        }
    //        else
    //        {
    //            tile.Content = contentFactory.Get(towerType);
    //            updatingContent.Add(tile.Content);
    //        }
    //    }
    //    else if (tile.Content.Type == GameTileContentType.Empty)
    //    {
    //        tile.Content = contentFactory.Get(towerType);
    //        if (FindPaths())
    //        {
    //            updatingContent.Add(tile.Content);
    //        }
    //        else
    //        {
    //            tile.Content = contentFactory.Get(GameTileContentType.Empty);
    //            FindPaths();
    //        }
    //    }
    //    else if (tile.Content.Type == GameTileContentType.Wall)
    //    {
    //        tile.Content = contentFactory.Get(towerType);
    //        updatingContent.Add(tile.Content);
    //    }
    //}

    private void BuildTower(GameTile tile, GameTileContentType type)
    {
        if (tile.Content.Type != GameTileContentType.Empty || type <= GameTileContentType.BeforeAttackers)
            return;

        tile.Content = _contentFactory.Get(type);
        if (FindPaths())
        {
            _updatingContent.Add(tile.Content);
        }
        else
        {
            tile.Content = _contentFactory.Get(GameTileContentType.Empty);
            FindPaths();
        }
    }

    private void BuildSpawnPoint(GameTile tile)
    {
        if (tile.Content.Type != GameTileContentType.Empty)
            return;

        tile.Content = _contentFactory.Get(GameTileContentType.SpawnPoint);
        _spawnPoints.Add(tile);
    }

    private void DestroyDestination(GameTile tile)
    {
        if (tile.Content.Type != GameTileContentType.Destination)
            return;

        tile.Content = _contentFactory.Get(GameTileContentType.Empty);
        if (FindPaths() == false)
        {
            tile.Content = _contentFactory.Get(GameTileContentType.Destination);
            FindPaths();
        }
    }

    private void DestroySpawnPoint(GameTile tile)
    {
        if (tile.Content.Type != GameTileContentType.SpawnPoint)
            return;
        if (_spawnPoints.Count <= 1)
            return;

        _spawnPoints.Remove(tile);
        tile.Content = _contentFactory.Get(GameTileContentType.Empty);
    }

    private void DestroyWall(GameTile tile)
    {
        if (tile.Content.Type <= GameTileContentType.BeforeAttackers)
            return;

        tile.Content = _contentFactory.Get(GameTileContentType.Empty);
        FindPaths();
    }

    private void DestroyTower(GameTile tile)
    {
        if (tile.Content.Type <= GameTileContentType.BeforeAttackers)
            return;

        _updatingContent.Remove(tile.Content);
        tile.Content = _contentFactory.Get(GameTileContentType.Empty);
        FindPaths();
    }

    public GameTile GetSpawnPoint(int index)
    {
        return _spawnPoints[index];
    }

    public GameTile GetTile(Ray ray)
    {
        if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, 1))
        {
            int x = (int)(hit.point.x + _size.x * 0.5f);
            int y = (int)(hit.point.z + _size.y * 0.5f);
            if (x >= 0 && x < _size.x && y >= 0 && y < _size.y)
            {
                return _tiles[x + y * _size.x];
            }
        }
        return null;
    }

    private bool FindPaths()
    {
        foreach (GameTile tile in _tiles)
        {
            if (tile.Content.Type == GameTileContentType.Destination)
            {
                tile.BecomeDestination();
                _searchFrontier.Enqueue(tile);
            }
            else
            {
                tile.ClearPath();
            }
        }
        if (_searchFrontier.Count == 0)
        {
            return false;
        }

        while (_searchFrontier.Count > 0)
        {
            GameTile tile = _searchFrontier.Dequeue();
            if (tile != null)
            {
                if (tile.IsAlternative)
                {
                    _searchFrontier.Enqueue(tile.GrowPathNorth());
                    _searchFrontier.Enqueue(tile.GrowPathSouth());
                    _searchFrontier.Enqueue(tile.GrowPathEast());
                    _searchFrontier.Enqueue(tile.GrowPathWest());
                }
                else
                {
                    _searchFrontier.Enqueue(tile.GrowPathWest());
                    _searchFrontier.Enqueue(tile.GrowPathEast());
                    _searchFrontier.Enqueue(tile.GrowPathSouth());
                    _searchFrontier.Enqueue(tile.GrowPathNorth());
                }
            }
        }

        foreach (GameTile tile in _tiles)
        {
            if (!tile.HasPath)
            {
                return false;
            }
        }

        foreach (GameTile tile in _tiles)
        {
            tile.ShowPath();
        }
        return true;
    }
}


