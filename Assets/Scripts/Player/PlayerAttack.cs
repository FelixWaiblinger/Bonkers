using System;
using UnityEngine;
using Unity.Netcode;

public class PlayerAttack : NetworkBehaviour
{
    [SerializeField] private Projectile _attackType;
    [SerializeField] private float _projectileSpeed = 10;
    [SerializeField] private float _projectileRange = 2;
    [SerializeField] private int _strength = 1;
    [SerializeField] private float _cooldown = 1;
    [SerializeField] private float _attackLockTime = 0.2f;
    [SerializeField] private float _spawnOffset = 1;

    private Camera _camera;
    private Plane _ground = new(Vector3.up, Vector3.zero);
    private float lastFired = float.MinValue;

    public static event Action<float> PlayerAttackEvent;

    public override void OnNetworkSpawn()
    {
        _camera = GameObject.FindObjectOfType<Camera>();
    }

    void Update()
    {
        if (!IsOwner) return;

        if (Input.GetMouseButton(0) && lastFired + _cooldown < Time.time)
        {
            PlayerAttackEvent.Invoke(_attackLockTime);
            
            lastFired = Time.time;

            var ray = _camera.ScreenPointToRay(Input.mousePosition);
        
            if (_ground.Raycast(ray, out var enter))
            {
                var hit = ray.GetPoint(enter);
                var direction = (hit - transform.position).normalized;

                // request to attack on all clients
                AttackServerRpc(direction);

                // attack locally
                Attack(direction);
            }
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
        if (!IsOwner) Attack(direction);
    }

    private void Attack(Vector3 direction)
    {
        var spawnPosition = transform.position + direction * _spawnOffset;
        var attack = Instantiate(_attackType, spawnPosition, Quaternion.LookRotation(direction));

        attack.Init(direction * _projectileSpeed, _projectileRange, _strength, gameObject.layer);

        // TODO shoot effects
    }
}
