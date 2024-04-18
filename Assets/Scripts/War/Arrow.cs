using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class Arrow : WarEntity
{
    private Vector3 _launchPoint, _targetPoint, _launchVelocity;
    private float _age;
    private float _blastRadius, _damage;

    public void Initialize(Vector3 launchPoint, Vector3 targetPoint,  Vector3 launchVelocity, float blastRadius, float damage)
    {
        _launchPoint = launchPoint;
        _targetPoint = targetPoint;
        _launchVelocity = launchVelocity;
        _blastRadius = blastRadius;
        _damage = damage;

        transform.position = launchPoint;
        _age = 0f;

        transform.rotation = Quaternion.LookRotation(launchVelocity.normalized);

        if (damage > 0f)
        {
            TargetPoint.FillBuffer(targetPoint, blastRadius);
            for (int i = 0; i < TargetPoint.BufferedCount; i++)
            {
                TargetPoint.GetBuffered(i).Enemy.TakeDamage(damage);
            }
        }
    }

    public override bool GameUpdate()
    {
        _age += Time.deltaTime;

        transform.position = _launchPoint + _launchVelocity * _age;

        if (transform.position.y <= 0f)
        {
            OriginFactory.Reclaim(this);
            return false;
        }

        return true;
    }

    //private Vector3 _targetPosition;
    //[SerializeField]
    //private float _speed = 10f;

    //public void Fire(Vector3 startPosition, Vector3 targetPosition)
    //{
    //    _targetPosition = targetPosition;
    //    transform.LookAt(targetPosition); 
    //    transform.position = startPosition; 

    //    Destroy(gameObject, 5f);
    //}

    //private void Update()
    //{
    //    transform.Translate(Vector3.forward * _speed * Time.deltaTime);

    //    if (Vector3.Distance(transform.position, _targetPosition) < 0.1f)
    //    {
    //        Destroy(gameObject);
    //    }
    //}
}