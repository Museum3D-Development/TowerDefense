using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public abstract class Tower : GameTileContent
{
    [SerializeField, Range(1.5f, 10.5f)]
    //private float _targetingRange = 1.5f;
    protected float _targetingRange = 1.5f;
    //[SerializeField, Range(1, 100f)]
    //private float _damagePerSecond = 10f;
    //private TargetPoint _target;
    //[SerializeField]
    //private Transform _turret;
    //[SerializeField]
    //private Transform _laserBeam;
    //private Vector3 _laserBeamScale;
    //private const int ENEMY_LAYER_MASK = 1 << 9;

    //public abstract TowerType TowerType { get; }
    public abstract GameTileContentType Type { get; }

    //private void Awake()
    //{
    //    _laserBeamScale = _laserBeam.localScale;
    //}

    //public override void GameUpdate()
    //{
    //    if(IsTargetTracked() || IsAcquireTarget())
    //    {
    //        Shoot();
    //    }
    //    else
    //    {
    //        _laserBeam.localScale = Vector3.zero;
    //    }
    //}

    protected bool IsAcquireTarget(out TargetPoint target)
    {
        //Collider[] targets = Physics.OverlapSphere(transform.localPosition, _targetingRange, ENEMY_LAYER_MASK);
        //if (targets.Length > 0)
        if (TargetPoint.FillBuffer(transform.localPosition, _targetingRange))
        {
            //target = targets[0].GetComponent<TargetPoint>();
            target = TargetPoint.GetBuffered(0);
            return true;
        }

        target = null;
        return false;
    }

    protected bool IsTargetTracked(ref TargetPoint target)
    {
        if (target == null)
        {
            return false;
        }
        
        Vector3 myPos = transform.localPosition;
        Vector3 targetPos = target.Position;

        if (Vector3.Distance(myPos, targetPos) > _targetingRange + 
            target.ColliderSize * target.Enemy.Scale || target.IsEnabled == false)
        {
            target = null;
            return false;
        }

        return true;
    }

    //private void Shoot()
    //{
    //    var point = _target.Position;
    //    _turret.LookAt(point);
    //    _laserBeam.localRotation = _turret.localRotation;

    //    var distance = Vector3.Distance(_turret.position, point);
    //    _laserBeamScale.z = distance;
    //    _laserBeam.localScale = _laserBeamScale;
    //    _laserBeam.localPosition = _turret.localPosition + 0.5f * distance * _laserBeam.forward;

    //    _target.Enemy.TakeDamage(_damagePerSecond * Time.deltaTime);
    //}

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector3 position = transform.localPosition;
        position.y += 0.01f;
        Gizmos.DrawWireSphere(position, _targetingRange);

        //if(_target != null)
        //{
        //    Gizmos.color = Color.red;
        //    Gizmos.DrawLine(position, _target.Position);
        //}
    }
}
