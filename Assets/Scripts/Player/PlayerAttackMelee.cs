using UnityEngine;
using Unity.Netcode;

public class PlayerAttackMelee : NetworkBehaviour
{
    [SerializeField] private GameObject weaponType;
    [SerializeField] private Transform spawner;
    [SerializeField] private float weaponSpeed = 2f;
    [SerializeField] private float weaponRange = 1f;
    [SerializeField] private int strength = 1;
    [SerializeField] private float cooldown = 1f;

    private float lastAttack = float.MinValue;

    void Update()
    {
        if (!IsOwner) return;

        if (Input.GetMouseButton(0) && lastAttack + cooldown < Time.time)
        {
            lastAttack = Time.time;
            var direction = transform.forward;

            // request to fire on all clients
            AttackServerRpc(direction);

            // fire locally
            SwingWeapon(direction);
        }
    }

    [ServerRpc]
    private void AttackServerRpc(Vector3 direction)
    {
        AttackClientRpc(direction);
    }

    [ClientRpc]
    private void AttackClientRpc(Vector3 direction)
    {
        if (!IsOwner) SwingWeapon(direction);
    }

    private void SwingWeapon(Vector3 direction)
    {
        var weapon = Instantiate(weaponType, spawner.position, Quaternion.identity);
        // weapon.GetComponent<Weapon>().Init(weaponSpeed, strength);
        // TODO attack effects
    }
}
