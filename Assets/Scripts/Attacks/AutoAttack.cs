using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoAttack : Projectile
{
    void Update()
    {
        transform.position += projectileDirection * Time.deltaTime;

        if (Vector3.Distance(spawnPosition, transform.position) > projectileRange)
        {
            Destroy(gameObject);
        }
    }

    protected override void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Environment"))
        {
            Debug.Log("environment");
            Destroy(gameObject);
            return;
        }

        if (other.gameObject.TryGetComponent<PlayerHealth>(out var player))
        {
            // TODO check if enemy or self/team
            player.ApplyDamage(damage);
        }
    }
}
