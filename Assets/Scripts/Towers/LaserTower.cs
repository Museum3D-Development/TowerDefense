using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class LaserTower : Tower
{
    [SerializeField, Range(0.5f, 10f)]
    private float _shootsPerSeconds = 1f;

    [SerializeField]
    private float _arrowSpeed = 10f;

    [SerializeField]
    private float _arrowBlastRadius = 1f;

    [SerializeField]
    private float _damage = 10f;

    [SerializeField]
    private Transform _rotator;

    [SerializeField]
    private Transform _spawnPoint;

    private float _launchProgress;

    public override GameTileContentType Type => GameTileContentType.MortarTower;

    private void Awake()
    {
        OnValidate();
    }

    private void OnValidate()
    {
        
    }

    public override void GameUpdate()
    {
        _launchProgress += Time.deltaTime * _shootsPerSeconds;

        while (_launchProgress >= 1f)
        {
            if (IsAcquireTarget(out TargetPoint target))
            {
                Launch(target);
                _launchProgress -= 1f;
            }
            else
            {
                _launchProgress = 0.999f;
            }
        }
    }

    private void Launch(TargetPoint target)
    {
        Vector3 targetPoint = target.Position;
        targetPoint.y = 0f;
        Vector3 direction = (target.Position - _spawnPoint.position).normalized;

        _rotator.rotation = Quaternion.LookRotation(direction, Vector3.up);

        Arrow arrow = Game.SpawnArrow();
        arrow.Initialize(_spawnPoint.position, targetPoint, direction * _arrowSpeed, _arrowBlastRadius, _damage);
    }

    //[SerializeField, Range(1f, 100f)]
    //private float _damagePerSecond = 10f;

    //private TargetPoint _target;

    //[SerializeField]
    //private Transform _turret;

    //[SerializeField]
    //private Transform _laserBeam;

    //private Vector3 _laserBeamScale;
    //private Vector3 _laserBeamStartPosition;

    ////public override TowerType TowerType => TowerType.Laser;
    //public override GameTileContentType Type => GameTileContentType.LaserTower;

    //private void Awake()
    //{
    //    _laserBeamScale = _laserBeam.localScale;
    //    _laserBeamStartPosition = _laserBeam.localPosition;
    //}

    //public override void GameUpdate()
    //{
    //    if (IsTargetTracked(ref _target) || IsAcquireTarget(out _target))
    //    {
    //        Shoot();
    //    }
    //    else
    //    {
    //        _laserBeam.localScale = Vector3.zero;
    //    }
    //}

    //private void Shoot()
    //{
    //    var point = _target.Position;
    //    _turret.LookAt(point);
    //    _laserBeam.localRotation = _turret.localRotation;

    //    var distance = Vector3.Distance(_turret.position, point);
    //    _laserBeamScale.z = distance;
    //    _laserBeam.localScale = _laserBeamScale;
    //    _laserBeam.localPosition = _laserBeamStartPosition + 0.5f * distance * _laserBeam.forward;

    //    _target.Enemy.TakeDamage(_damagePerSecond * Time.deltaTime);
    //}
}