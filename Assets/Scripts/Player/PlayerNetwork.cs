using UnityEngine;
using Unity.Netcode;

public class PlayerNetwork : NetworkBehaviour
{
    private readonly NetworkVariable<PlayerNetworkData> netState = new NetworkVariable<PlayerNetworkData>(
        writePerm: NetworkVariableWritePermission.Owner
    );

    [SerializeField] private float interpolationTime = 0.1f;
    private Vector3 velocity;
    private float rotationVelocity;

    void Update()
    {
        if (IsOwner)
        {   // WRITE
            netState.Value = new PlayerNetworkData() {
                position = transform.position,
                rotation = transform.rotation.eulerAngles
            };
        }
        else
        {   // READ
            transform.position = Vector3.SmoothDamp(
                transform.position,
                netState.Value.position,
                ref velocity,
                interpolationTime
            );
            transform.rotation = Quaternion.Euler(
                0,
                Mathf.SmoothDampAngle(
                    transform.rotation.eulerAngles.y,
                    netState.Value.rotation.y,
                    ref rotationVelocity,
                    interpolationTime
                ),
                0
            );
        }
    }

    struct PlayerNetworkData : INetworkSerializable
    {
        // position & rotation
        private float x, z, rot;

        internal Vector3 position
        {
            get => new Vector3(x, 0, z);
            set {
                x = value.x;
                z = value.z;
            }
        }

        internal Vector3 rotation
        {
            get => new Vector3(0, rot, 0);
            set => rot = value.y;
        }

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref x);
            serializer.SerializeValue(ref z);
            serializer.SerializeValue(ref rot);
        }
    }
}
