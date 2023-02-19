using UnityEngine;
using Unity.Netcode;

public class PlayerNetwork : NetworkBehaviour
{
    private readonly NetworkVariable<PlayerNetworkData> NetState = new NetworkVariable<PlayerNetworkData>(
        writePerm: NetworkVariableWritePermission.Owner
    );
    private Vector3 Velocity;
    private float RotationVelocity;
    [SerializeField] private float InterpolationTime = 0.1f;

    void Update()
    {
        if (IsOwner)
        {   // WRITE
            NetState.Value = new PlayerNetworkData() {
                Position = transform.position,
                Rotation = transform.rotation.eulerAngles
            };
        }
        else
        {   // READ
            transform.position = Vector3.SmoothDamp(
                transform.position,
                NetState.Value.Position,
                ref Velocity,
                InterpolationTime
            );
            transform.rotation = Quaternion.Euler(
                0,
                Mathf.SmoothDampAngle(
                    transform.rotation.eulerAngles.y,
                    NetState.Value.Rotation.y,
                    ref RotationVelocity,
                    InterpolationTime
                ),
                0
            );
        }
    }

    struct PlayerNetworkData : INetworkSerializable
    {
        private float _x, _z, _rot;

        internal Vector3 Position
        {
            get => new Vector3(_x, 0, _z);
            set {
                _x = value.x;
                _z = value.z;
            }
        }

        internal Vector3 Rotation
        {
            get => new Vector3(0, _rot, 0);
            set => _rot = value.y;
        }

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref _x);
            serializer.SerializeValue(ref _z);
            serializer.SerializeValue(ref _rot);
        }
    }
}
