using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    protected Vector3 _spawnPosition = Vector3.zero;
    protected Vector3 _projectileDirection = Vector3.zero;
    protected float _projectileRange = 1f;
    protected int _damage = 0;

    public void Init(Vector3 direction, float range, int strength)
    {
        _projectileDirection = direction;
        _spawnPosition = transform.position;
        _projectileRange = range;
        _damage = strength;
    }

    protected abstract void OnCollisionEnter(Collision other);
}
