using UnityEngine;

public class AutoAttackRanged : Projectile
{
    void Update()
    {
        transform.position += _projectileDirection * Time.deltaTime;

        if (Vector3.Distance(_spawnPosition, transform.position) > _projectileRange)
        {
            Destroy(gameObject);
        }
    }

    protected override void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Environment"))
            Destroy(gameObject);

        else if (other.gameObject.TryGetComponent<PlayerHealth>(out var player))
        {
            // TODO check if enemy or self/team
            player.ApplyDamage(_damage);
        }
    }
}
