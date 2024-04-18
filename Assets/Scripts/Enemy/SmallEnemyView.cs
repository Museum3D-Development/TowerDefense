using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallEnemyView : EnemyView
{
    public void OnDieAnimationFinished()
    {
        _enemy.Recycle();
    }
}
