using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class GameManager : NetworkBehaviour {
    [SerializeField] private PlayerData _playerData;
    [SerializeField] private List<GameObject> _playerPrefabs;

    public override void OnNetworkSpawn() {
        SpawnPlayerServerRpc(NetworkManager.Singleton.LocalClientId);
    }   

    [ServerRpc(RequireOwnership = false)]
    private void SpawnPlayerServerRpc(ulong playerId) {
        foreach (GameObject prefab in _playerPrefabs)
        {
            if (prefab.name == _playerData.prefab)
            {
                var spawn = Instantiate(prefab);
                spawn.GetComponent<NetworkObject>().SpawnWithOwnership(playerId);
                break;
            }
        }
        
    }

    public override void OnDestroy() {
        base.OnDestroy();
        MatchmakingService.LeaveLobby();
        if(NetworkManager.Singleton != null )NetworkManager.Singleton.Shutdown();
    }
}