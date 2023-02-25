using UnityEngine;
using Unity.Netcode;

public class PlayerAttackRanged : NetworkBehaviour
{
    [SerializeField] private GameObject projectileType;
    [SerializeField] private Transform spawner;
    [SerializeField] private float projectileSpeed = 700f;
    [SerializeField] private float projectileRange = 2f;
    [SerializeField] private int strength = 1;
    [SerializeField] private float cooldown = 1f;

    private float lastFired = float.MinValue;

    void Update()
    {
        if (!IsOwner) return;

        if (Input.GetMouseButton(0) && lastFired + cooldown < Time.time)
        {
            lastFired = Time.time;
            var direction = transform.forward;

            // request to fire on all clients
            FireServerRpc(direction);

            // fire locally
            ShootProjectile(direction, projectileRange);
        }
    }

    [ServerRpc]
    private void FireServerRpc(Vector3 direction)
    {
        FireClientRpc(direction);
    }

    [ClientRpc]
    private void FireClientRpc(Vector3 direction)
    {
        if (!IsOwner) ShootProjectile(direction, projectileRange);
    }

    private void ShootProjectile(Vector3 direction, float range)
    {
        var projectile = Instantiate(projectileType, spawner.position, Quaternion.identity);
        projectile.GetComponent<Projectile>().Init(direction * projectileSpeed, range, strength);
        // TODO shoot effects
    }
}
