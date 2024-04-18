using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MediumEnemyView : EnemyView
{
    public void OnDieAnimationFinished()
    {
        _enemy.Recycle();
    }
}
