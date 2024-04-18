using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LargeEnemyView : EnemyView
{
    public void OnDieAnimationFinished()
    {
        _enemy.Recycle();
    }
}
