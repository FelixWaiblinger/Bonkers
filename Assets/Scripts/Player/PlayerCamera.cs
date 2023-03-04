using UnityEngine;
using Unity.Netcode;
using Cinemachine;

public class PlayerCamera : NetworkBehaviour
{
    void Start()
    {
        Debug.Log("Grab camera");
        if (!IsOwner) return;
        
        var cam = GameObject.FindGameObjectWithTag("PlayerCamera")
                            .GetComponent<CinemachineVirtualCamera>();

        cam.Follow = transform;
        cam.LookAt = transform;
    }
}
