using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    protected Vector3 _spawnPosition = Vector3.zero;
    protected Vector3 _projectileDirection = Vector3.zero;
    protected float _projectileRange = 1f;
    protected int _damage = 0;
    protected int _team = -1;

    public void Init(Vector3 direction, float range, int strength, int team)
    {
        _projectileDirection = direction;
        _spawnPosition = transform.position;
        _projectileRange = range;
        _damage = strength;
        _team = team;
    }

    protected abstract void OnCollisionEnter(Collision other);
}
