using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : NetworkBehaviour {
    [SerializeField] private PlayerData _playerData;
    [SerializeField] private LobbyData _lobbyData;
    [SerializeField] private List<GameObject> _playerPrefabs;
    
    private List<int> _teamPlayerCount = new List<int>();

    public override void OnNetworkSpawn()
    {
        var team = _lobbyData.players[_playerData.playerName];
        SpawnPlayerServerRpc(_lobbyData.clientId, _playerData.playerName, team);

        // LMS
        if (_lobbyData.gameMode == Constants.GameModes[0])
        {
            foreach (var entry in NetworkManager.Singleton.ConnectedClientsList)
                _teamPlayerCount.Add(1);
        }
        // Teams
        else
        {
            int members = _lobbyData.gameMode == Constants.GameModes[1] ? 3 : 2;
            _teamPlayerCount.Add(members);
            _teamPlayerCount.Add(members);
        }
    }

    void OnEnable()
    {
        PlayerInfo.PlayerDeathEvent += CheckGameState;
    }

    void OnDisable()
    {
        PlayerInfo.PlayerDeathEvent -= CheckGameState;
    }

    private void CheckGameState(int teamIndex)
    {
        // TODO remove later ---------------------------------
        if (teamIndex < 0)
        {
            Debug.Log("Error - no team assigned!");
            return;
        }
        if (_teamPlayerCount.Count < teamIndex + 1)
        {
            Debug.Log("Error - not enough teams assigned!");
            return;
        }
        if (_teamPlayerCount[teamIndex] == 0)
        {
            Debug.Log("Error - team has no members!");
            return;
        }
        // ---------------------------------------------------

        _teamPlayerCount[teamIndex] -= 1;

        // check win condition: one team left standing
        int teamsAlive = 0;
        foreach (int teamSize in _teamPlayerCount)
        {
            teamsAlive += teamSize > 0 ? 1 : 0;
        }

        if (teamsAlive > 1) return;

        // TODO show end screen and return to lobby
        Debug.Log("GAME OVER - TEAM " + _teamPlayerCount.FindIndex(teamSize => teamSize > 0) + " HAS WON!");

        using (new Load("Returning to client...")) {
            SceneManager.LoadSceneAsync("Lobby");
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void SpawnPlayerServerRpc(ulong id, string name, int team)
    {
        var spawn = Instantiate(_playerPrefabs.Find(x => x.name == _playerData.prefab));
        spawn.GetComponent<NetworkObject>().SpawnWithOwnership(id);
        SpawnPlayerClientRpc(spawn.name, id, name, team);

        spawn.name = name;
        spawn.layer = team + 20;
    }

    public static event Action OnPlayerSpawned;

    [ClientRpc]
    private void SpawnPlayerClientRpc(string instance, ulong id, string name, int team)
    {
        var player = GameObject.Find(instance);
        player.name = name;
        player.layer = team + 20;

        OnPlayerSpawned?.Invoke();
    }

    public override void OnDestroy() {
        base.OnDestroy();
        MatchmakingService.LeaveLobby();
        if (NetworkManager.Singleton != null) NetworkManager.Singleton.Shutdown();
    }
}