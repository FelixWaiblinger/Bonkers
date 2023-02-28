using UnityEngine;
using Unity.Netcode;

public class PlayerController : NetworkBehaviour
{
    // Player Movement
    [SerializeField] private Transform _visuals;
    [SerializeField] private Rigidbody _rigidBody;
    [SerializeField] private float _movementSpeed = 0.1f;
    private Vector3 _movementInput = Vector3.zero;
    private float _movementDelay = 0f;
    private float _rotationSpeed = 450;
    private Camera _playerCamera;
    private Plane _ground = new(Vector3.up, Vector3.zero);

    private void OnEnable()
    {
        PlayerAttack.PlayerAttackEvent += OnPlayerAttack;
    }

    private void OnDisable()
    {
        PlayerAttack.PlayerAttackEvent -= OnPlayerAttack;
    }

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) this.enabled = false;
        else _playerCamera = GameObject.FindObjectOfType<Camera>();
    }

    private void OnPlayerAttack(float delay)
    {
        _movementDelay = delay;
    }

    void Update()
    {
        if (_movementDelay > 0)
        {
            _movementDelay -= Time.deltaTime;
            _movementInput = Vector3.zero;
        }
        else
            _movementInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
    }

    void FixedUpdate()
    {
        // position-based movement
        _rigidBody.position += _movementInput.normalized * _movementSpeed;

        // camera position invariant rotation
        var ray = _playerCamera.ScreenPointToRay(Input.mousePosition);
        
        if (_ground.Raycast(ray, out var enter))
        {
            var hit = ray.GetPoint(enter);
            var direction = hit - transform.position;
            var rotation = Quaternion.LookRotation(direction);
        
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                rotation,
                _rotationSpeed * Time.deltaTime
            );
        }
    }
}
