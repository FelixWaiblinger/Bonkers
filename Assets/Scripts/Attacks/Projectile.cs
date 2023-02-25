using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    protected Vector3 spawnPosition = Vector3.zero;
    protected Vector3 projectileDirection = Vector3.zero;
    protected float projectileRange = 1f;
    protected int damage = 0;

    public void Init(Vector3 direction, float range, int strength)
    {
        projectileDirection = direction;
        spawnPosition = transform.position;
        projectileRange = range;
        damage = strength;
    }

    protected abstract void OnCollisionEnter(Collision other);
}
