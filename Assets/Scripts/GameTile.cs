using UnityEngine;

public class GameTile : MonoBehaviour
{
    [SerializeField]
    private Transform _arrow = default;

    private GameTile _north, _east, _south, _west, _nextOnPath;

    private int _distance;

    private GameTileContent _content;

    public GameTileContent Content
    {
        get => _content;
        set
        {
            Debug.Assert(value != null, "Null assigned to content!");
            if (_content != null)
            {
                _content.Recycle();
            }
            _content = value;
            _content.transform.localPosition = transform.localPosition;
        }
    }

    public Direction PathDirection { get; private set; }

    public Vector3 ExitPoint { get; private set; }

    public bool IsAlternative { get; set; }

    public bool HasPath => _distance != int.MaxValue;

    public GameTile NextTileOnPath => _nextOnPath;

    public void BecomeDestination()
    {
        _distance = 0;
        _nextOnPath = null;
        ExitPoint = transform.localPosition;
    }

    public void ClearPath()
    {
        _distance = int.MaxValue;
        _nextOnPath = null;
    }

    public GameTile GrowPathNorth() => GrowPathTo(_north, Direction.South);

    public GameTile GrowPathEast() => GrowPathTo(_east, Direction.West);

    public GameTile GrowPathSouth() => GrowPathTo(_south, Direction.North);

    public GameTile GrowPathWest() => GrowPathTo(_west, Direction.East);

    GameTile GrowPathTo(GameTile neighbor, Direction direction)
    {
        Debug.Assert(HasPath, "No path!");
        if (neighbor == null || neighbor.HasPath)
        {
            return null;
        }
        neighbor._distance = _distance + 1;
        neighbor._nextOnPath = this;
        neighbor.ExitPoint =
            neighbor.transform.localPosition + direction.GetHalfVector();
        neighbor.PathDirection = direction;
        return neighbor.Content.IsBlockingPath ? null : neighbor;
    }

    public void ShowPath()
    {
        if (_distance == 0)
        {
            _arrow.gameObject.SetActive(false);
            return;
        }
        _arrow.gameObject.SetActive(true);
        _arrow.localRotation =
            _nextOnPath == _north ? northRotation :
            _nextOnPath == _east ? eastRotation :
            _nextOnPath == _south ? southRotation :
            westRotation;
    }

    private static Quaternion
        northRotation = Quaternion.Euler(90f, 0f, 180f),
        eastRotation = Quaternion.Euler(90f, 90f, 180f),
        southRotation = Quaternion.Euler(90f, 180f, 180f),
        westRotation = Quaternion.Euler(90f, 270f, 180f);

    public static void MakeEastWestNeighbors(GameTile east, GameTile west)
    {
        Debug.Assert(
            west._east == null && east._west == null, "Redefined neighbors!"
        );
        west._east = east;
        east._west = west;
    }

    public static void MakeNorthSouthNeighbors(GameTile north, GameTile south)
    {
        Debug.Assert(
            south._north == null && north._south == null, "Redefined neighbors!"
        );
        south._north = north;
        north._south = south;
    }
}