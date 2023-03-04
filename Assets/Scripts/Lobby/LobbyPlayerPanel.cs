using System;
using UnityEngine;
using TMPro;

public class LobbyPlayerPanel : MonoBehaviour {
    [SerializeField] private TMP_Text _nameText, _statusText;

    private string _playerName = "";
    private int _team = -1;

    public ulong PlayerId { get; private set; }

    public void Init(ulong playerId)
    {
        PlayerId = playerId;
    }

    public void UpdatePanel(string playerName, int team)
    {
        _playerName = playerName;
        _team = team;
        _nameText.text = _playerName;
    }

    public Tuple<string, int> GetPlayerInfo()
    {
        return new Tuple<string, int>(_playerName, _team);
    }

    public void SetReady()
    {
        _statusText.text = "Ready";
        _statusText.color = Color.green;
    }
}