using UnityEngine;
using Unity.Netcode;
using Cinemachine;

public class PlayerCamera : NetworkBehaviour
{
    // [SerializeField] private Vector3 _offset = new Vector3(0, 10, -8);
    // [SerializeField] private float _rotationX = 60;

    private CinemachineVirtualCamera _playerCamera;

    void Start()
    {
        Debug.Log("Grab camera");
        if (IsOwner)
        {
            _playerCamera =
                GameObject.FindGameObjectWithTag("PlayerCamera")
                          .GetComponent<CinemachineVirtualCamera>();

            _playerCamera.Follow = transform;
            _playerCamera.LookAt = transform;
        }
    }

    // void Update()
    // {
    //     if (!IsOwner) return;
    // 
    //     _playerCamera.position = Vector3.zero + transform.localPosition + _offset;
    // }

    // private void OnApplicationFocus(bool focusStatus) {
    //     if (focusStatus && _playerCamera != null)
    //         _playerCamera.rotation = Quaternion.Euler(_rotationX, 0, 0);
    // }
}
