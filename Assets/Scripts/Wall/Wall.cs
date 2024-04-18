using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public abstract class Wall : GameTileContent
{
    //public abstract WallType WallType { get; }
    public abstract GameTileContentType Type { get; }
}

