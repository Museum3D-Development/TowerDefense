using UnityEngine;

[SelectionBase]
public class GameTileContent : MonoBehaviour
{
    [SerializeField]
    private GameTileContentType _type = default;

    private GameTileContentFactory _originFactory;

    //public bool IsBlockingPath => Type == GameTileContentType.Wall || Type == GameTileContentType.Tower;
    public bool IsBlockingPath => Type > GameTileContentType.BeforeBlockers;

    public GameTileContentType Type => _type;

    public GameTileContentFactory OriginFactory
    {
        get => _originFactory;
        set
        {
            Debug.Assert(_originFactory == null, "Redefined origin factory!");
            _originFactory = value;
        }
    }

    public void Recycle()
    {
        _originFactory.Reclaim(this);
    }

    public virtual void GameUpdate() { }
}