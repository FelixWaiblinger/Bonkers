using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoAttackMelee : Projectile
{
    private Quaternion _targetRotation;
    void Start()
    {
        Debug.Log("SpawnRotation: " + transform.rotation.eulerAngles.y);
        _targetRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y - 45, 0);
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + 45, 0);
    }

    void Update()
    {
        transform.rotation = Quaternion.RotateTowards(transform.rotation, _targetRotation, 1);

        if (transform.rotation == _targetRotation)
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
