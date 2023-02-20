using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Vector3 projectileDirection = Vector3.zero;
    private Vector3 spawnPosition = Vector3.zero;
    private float projectileRange = 1f;
    private int damage = 0;

    void Update()
    {
        transform.position += projectileDirection * Time.deltaTime;

        if (Vector3.Distance(spawnPosition, transform.position) > projectileRange)
        {
            Destroy(gameObject);
        }
    }

    public void Init(Vector3 direction, float range, int strength)
    {
        projectileDirection = direction;
        spawnPosition = transform.position;
        projectileRange = range;
        damage = strength;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent<PlayerHealth>(out var enemy))
            enemy.ApplyDamage(damage);
    }
}
