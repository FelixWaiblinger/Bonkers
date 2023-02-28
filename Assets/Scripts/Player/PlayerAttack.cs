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
    private Vector3 _direction = Vector3.zero;
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
                _direction = (hit - transform.position).normalized;

                // request to attack on all clients
                AttackServerRpc();

                // attack locally
                Attack();
            }
        }
    }

    [ServerRpc]
    private void AttackServerRpc()
    {
        AttackClientRpc();
    }

    [ClientRpc]
    private void AttackClientRpc()
    {
        if (!IsOwner) Attack();
    }

    private void Attack()
    {
        var spawnPosition = transform.position + _direction * _spawnOffset;

        var attack = Instantiate(_attackType, spawnPosition, Quaternion.LookRotation(_direction));

        attack.Init(_direction * _projectileSpeed, _projectileRange, _strength);

        // TODO shoot effects
    }
}
