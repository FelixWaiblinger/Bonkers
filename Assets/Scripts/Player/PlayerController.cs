using UnityEngine;
using Unity.Netcode;

public class PlayerController : NetworkBehaviour
{
    // Player Movement
    [SerializeField] private Rigidbody body;
    [SerializeField] private float movementSpeed = 0.1f;
    private Vector3 movementInput = Vector3.zero;
    private float rotationSpeed = 450;
    private Plane ground = new(Vector3.up, Vector3.zero);
    private Camera cam;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) this.enabled = false;
    }

    void Awake()
    {
        cam = GameObject.FindObjectOfType<Camera>();
    }

    void Update()
    {
        movementInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
    }

    void FixedUpdate()
    {
        // position-based movement
        body.position += movementInput * movementSpeed;

        // camera position invariant rotation
        var ray = cam.ScreenPointToRay(Input.mousePosition);

        if (ground.Raycast(ray, out var enter))
        {
            var hit = ray.GetPoint(enter);
            var direction = hit - transform.position;
            var rotation = Quaternion.LookRotation(direction);

            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                rotation,
                rotationSpeed * Time.deltaTime
            );
        }
    }
}
