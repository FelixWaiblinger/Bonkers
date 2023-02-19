using UnityEngine;
using Unity.Netcode;

public class PlayerController : NetworkBehaviour
{
    [SerializeField] private Rigidbody _RB;
    [SerializeField] private float MovementSpeed = 0.1f;

    private Vector3 _Input = Vector3.zero;

    private float RotationSpeed = 450;
    private Plane _Ground = new(Vector3.up, Vector3.zero);
    private Camera Camera;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) this.enabled = false;
    }

    void Awake()
    {
        Camera = Camera.main;
    }

    void Update()
    {
        _Input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
    }

    void FixedUpdate()
    {
        // Translation
        _RB.position += _Input * MovementSpeed;

        // Rotation
        var ray = Camera.ScreenPointToRay(Input.mousePosition);

        if (_Ground.Raycast(ray, out var enter))
        {
            var Hit = ray.GetPoint(enter);
            var Direction = Hit - transform.position;
            var Rotation = Quaternion.LookRotation(Direction);

            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                Rotation,
                RotationSpeed * Time.deltaTime
            );
        }
    }
}
