using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class MortarTower : Tower
{
    [SerializeField, Range(0.5f, 2f)]
    private float _shootsPerSeconds = 1f;

    [SerializeField, Range(0.5f, 3f)] 
    private float _shellBlastRadius = 1f;

    [SerializeField, Range(1f, 100f)]
    private float _damage = 10f;

    //[SerializeField]
    //private Transform _mortar;

    [SerializeField]
    private Transform _rotator;

    [SerializeField]
    private Transform _spawnPoint;

    private float _launchSpeed;
    private float _launchProgress;

    //public override TowerType TowerType => TowerType.Mortar;
    public override GameTileContentType Type => GameTileContentType.MortarTower;

    private void Awake()
    {
        OnValidate();
    }

    private void OnValidate()
    {
        float x = _targetingRange + 0.251f;
        //float y = -_mortar.position.y;
        float y = -_spawnPoint.position.y;
        _launchSpeed = Mathf.Sqrt(9.81f * (y + Mathf.Sqrt(x * x + y * y)));
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

        //Launch(new Vector3(3f, 0f, 0f));
        //Launch(new Vector3(0f, 0f, 1f));
        //Launch(new Vector3(1f, 0f, 1f));
        //Launch(new Vector3(3f, 0f, 1f));
    }

    private void Launch(TargetPoint target)
    {
        //Vector3 launchPoint = _mortar.position;
        Vector3 launchPoint = _spawnPoint.position;
        //Vector3 targetPoint = new Vector3(launchPoint.x + 3f, 0f, launchPoint.z);
        //Vector3 targetPoint = launchPoint + offset;
        Vector3 targetPoint = target.Position;
        targetPoint.y = 0f;

        //Vector2 dir;
        Vector3 dir;
        dir.x = targetPoint.x - launchPoint.x;
        dir.y = 0;
        dir.z = targetPoint.z - launchPoint.z;

        //float x = 3f;
        float x = dir.magnitude;
        float y = -launchPoint.y;
        dir /= x;

        float g = 9.81f;
        //float s = 5f;
        float s = _launchSpeed;
        float s2 = s * s;

        float r = s2 * s2 - g * (g * x * x + 2f * y * s2);
        r = MathF.Max(0, r); //ДОБАВЛЕНО

        float tanTheta = (s2 + Mathf.Sqrt(r)) / (g * x);
        float cosTheta = Mathf.Cos(Mathf.Atan(tanTheta));
        float sinTheta = cosTheta * tanTheta;

        //_mortar.localRotation = Quaternion.LookRotation(new Vector3(dir.x, tanTheta, dir.y));
        _rotator.localRotation = Quaternion.LookRotation(dir);

        //Game.SpawnShell().Initialize(launchPoint, targetPoint,
        //    new Vector3(s * cosTheta * dir.x, s * sinTheta, s * cosTheta * dir.y), _shellBlastRadius, _damage);
        Game.SpawnShell().Initialize(launchPoint, targetPoint,
            new Vector3(s * cosTheta * dir.x, s * sinTheta, s * cosTheta * dir.z), _shellBlastRadius, _damage);

        //Vector3 prev = launchPoint, next;
        //for (int i = 1; i <= 10; i++)
        //{
        //    float t = i / 10f;
        //    float dx = s * cosTheta * t;
        //    float dy = s * sinTheta * t - 0.5f * g * t * t;
        //    next = launchPoint + new Vector3(dir.x * dx, dy, dir.y * dx);
        //    Debug.DrawLine(prev, next, Color.blue);
        //    prev = next;
        //}

        //Debug.DrawLine(launchPoint, targetPoint, Color.yellow);
        //Debug.DrawLine(new Vector3(launchPoint.x, 0.01f, launchPoint.z),
        //    new Vector3(launchPoint.x + dir.x * x, 0.01f, launchPoint.z + dir.y * x), Color.white);
    }
}
