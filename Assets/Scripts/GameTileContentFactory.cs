using UnityEngine;

[CreateAssetMenu]
public class GameTileContentFactory : GameObjectFactory
{
    [SerializeField]
    private GameTileContent _destinationPrefab = default;

    [SerializeField]
    private GameTileContent _emptyPrefab = default;

    //[SerializeField]
    //Wall[] wallPrefabs = default;

    [SerializeField]
    private Wall _wallStraightPrefab = default;

    [SerializeField]
    private Wall _wallAnglePrefab = default;

    [SerializeField]
    private GameTileContent _spawnPointPrefab = default;

    //[SerializeField]
    //Tower[] towerPrefabs = default;

    [SerializeField]
    private Tower _laserTowerPrefab = default;

    [SerializeField]
    private Tower _mortarTowerPrefab = default;

    public GameTileContent Get(GameTileContentType type)
    {
        switch (type)
        {
            case GameTileContentType.Destination: 
                return Get(_destinationPrefab);
            case GameTileContentType.Empty: 
                return Get(_emptyPrefab);
            case GameTileContentType.SpawnPoint: 
                return Get(_spawnPointPrefab);
            case GameTileContentType.LaserTower:
                return Get(_laserTowerPrefab);
            case GameTileContentType.MortarTower:
                return Get(_mortarTowerPrefab);
            case GameTileContentType.WallStraight:
                return Get(_wallStraightPrefab);
            case GameTileContentType.WallAngle:
                return Get(_wallAnglePrefab);
        }
        Debug.Assert(false, "Unsupported non-tower type: " + type);
        return null;
    }

    //public Tower Get(TowerType type)
    //{
    //    Debug.Assert((int)type < towerPrefabs.Length, "Unsupported tower type!");
    //    Tower prefab = towerPrefabs[(int)type];
    //    Debug.Assert(type == prefab.TowerType, "Tower prefab at wrong index!");
    //    return Get(prefab);
    //}

    //public Wall Get(WallType type)
    //{
    //    Debug.Assert((int)type < wallPrefabs.Length, "Unsupported tower type!");
    //    Wall prefab = wallPrefabs[(int)type];
    //    Debug.Assert(type == prefab.WallType, "Tower prefab at wrong index!");
    //    return Get(prefab);
    //}

    public void Reclaim(GameTileContent content)
    {
        Debug.Assert(content.OriginFactory == this, "Wrong factory reclaimed!");
        Destroy(content.gameObject);
    }

    T Get<T>(T prefab) where T : GameTileContent
    {
        T instance = CreateGameObjectInstance(prefab);
        instance.OriginFactory = this;
        return instance;
    }
}