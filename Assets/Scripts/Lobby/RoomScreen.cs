using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
///     NetworkBehaviours cannot easily be parented, so the network logic will take place
///     on the network scene object "NetworkLobby"
/// </summary>
public class RoomScreen : NetworkBehaviour {
    [SerializeField] private LobbyPlayerPanel _playerPanelPrefab;
    [SerializeField] private Transform _playerPanelParentLMS;
    [SerializeField] private Transform _playerPanelParentTeams;
    [SerializeField] private TMP_Text _waitingText;
    [SerializeField] private GameObject _startButton, _readyButton;
    [SerializeField] private LobbyData _lobbyData;
    [SerializeField] private PlayerData _playerData;

    private Transform _playerPanelTeamLeft;
    private Transform _playerPanelTeamRight;
    private readonly List<LobbyPlayerPanel> _playerPanels = new();
    private bool _isGameModeLMS = false;
    private int _team = -1;
    private bool _allReady;
    private bool _ready;

    public static event Action StartPressed; 

    private void OnEnable()
    {
        _playerPanelTeamLeft = _playerPanelParentTeams.GetChild(0);
        _playerPanelTeamRight = _playerPanelParentTeams.GetChild(1);

        ResetRoom();
        
        _isGameModeLMS = (_lobbyData.gameMode == Constants.GameModes[0]);

        if (_isGameModeLMS) _playerPanelParentLMS.gameObject.SetActive(true);
        else _playerPanelParentTeams.gameObject.SetActive(true);

        LobbyOrchestrator.LobbyPlayersUpdated += NetworkLobbyPlayersUpdated;
        MatchmakingService.CurrentLobbyRefreshed += OnCurrentLobbyRefreshed;
        _startButton.SetActive(false);
        _readyButton.SetActive(false);

        _ready = false;
    }

    private void OnDisable()
    {
        LobbyOrchestrator.LobbyPlayersUpdated -= NetworkLobbyPlayersUpdated;
        MatchmakingService.CurrentLobbyRefreshed -= OnCurrentLobbyRefreshed;
    }

    public static event Action LobbyLeft;

    public void OnLeaveLobby()
    {
        LobbyLeft?.Invoke();
    }

    private void NetworkLobbyPlayersUpdated(Dictionary<ulong, bool> players)
    {
        // Remove all inactive panels
        var toDestroy = _playerPanels.Where(p => !players.Keys.Contains(p.PlayerId)).ToList();
        foreach (var panel in toDestroy)
        {
            _playerPanels.Remove(panel);
            Destroy(panel.gameObject);
        }

        foreach (var player in players)
        {
            var currentPanel = _playerPanels.FirstOrDefault(p => p.PlayerId == player.Key);

            // update ready state of joined players
            if (currentPanel != null)
            {
                if (player.Value) currentPanel.SetReady();

                continue;
            }
            
            // add new players
            var parent = transform;

            // game mode lms: all players are in a horizontal list
            if (_isGameModeLMS)
            {
                parent = _playerPanelParentLMS;
                _team += 1;
            }
            // game mode 3v3/2v2: players are added into two vertical lists
            // first add left and whenever right side has equal size
            else if (_playerPanelTeamLeft.childCount <= _playerPanelTeamRight.childCount)
            {
                parent = _playerPanelTeamLeft;
                _team = 0;
            }
            // add right if there are less players than on left
            else
            {
                parent = _playerPanelTeamRight;
                _team = 1;
            }

            var panel = Instantiate(_playerPanelPrefab, parent);
            panel.Init(player.Key);
            _playerPanels.Add(panel);
            
            if (player.Key == _lobbyData.clientId)
                UpdatePlayerServerRpc(_lobbyData.clientId, _playerData.playerName, _team);
        }

        _startButton.SetActive(NetworkManager.Singleton.IsHost && players.All(p => p.Value));
        _readyButton.SetActive(!_ready);
    }

    #region UPDATEPLAYER

    [ServerRpc(RequireOwnership = false)]
    private void UpdatePlayerServerRpc(ulong id, string name, int team)
    {
        UpdatePlayerClientRpc(id, name, team);
    }

    [ClientRpc]
    private void UpdatePlayerClientRpc(ulong id, string name, int team)
    {
        var panel = _playerPanels.Find(x => x.PlayerId == id);
        panel.UpdatePanel(name, team);
        panel.transform.SetParent(_playerPanelParentTeams.GetChild(team));

        if (_lobbyData.players.ContainsKey(name))
            _lobbyData.players[name] = team;
        else
            _lobbyData.players.Add(name, team);

        ReaderWriterJSON.SaveToJSON(_lobbyData);
    }

    public void OnTeamSwitch(int team)
    {
        // player has already declared ready
        if (_ready) return;
        
        // player is already in selected team
        if (_playerPanels.Find(x => x.PlayerId == _lobbyData.clientId).GetPlayerInfo().Item2 == team)
            return;

        // desired team is already full
        if (_playerPanelParentTeams.GetChild(team).childCount >= _lobbyData.teamSize)
            return;
        
        // player switches team
        UpdatePlayerServerRpc(_lobbyData.clientId, _playerData.playerName, team);
    }

    #endregion

    private void OnCurrentLobbyRefreshed(Lobby lobby)
    {
        _waitingText.text = $"Waiting on players... {lobby.Players.Count}/{lobby.MaxPlayers}";
    }

    public void OnReadyClicked()
    {
        _readyButton.SetActive(false);
        _ready = true;
    }

    public void OnStartClicked()
    {
        StartPressed?.Invoke();
    }

    private void ResetRoom()
    {
        foreach (Transform child in _playerPanelParentLMS) Destroy(child.gameObject);
        foreach (Transform child in _playerPanelTeamLeft) Destroy(child.gameObject);
        foreach (Transform child in _playerPanelTeamRight) Destroy(child.gameObject);
        _playerPanelParentTeams.gameObject.SetActive(false);
        _playerPanelParentLMS.gameObject.SetActive(false);
        _team = -1;
        _isGameModeLMS = false;
        _playerPanels.Clear();
        _lobbyData.players.Clear();
    }
}