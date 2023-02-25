using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class CreateLobbyScreen : MonoBehaviour {
    [SerializeField] private TMP_InputField _lobbyNameInput, _playerNameInput;
    [SerializeField] private TMP_Dropdown _modeDropdown, _typeDropdown;

    private void Start() {
        SetOptions(_modeDropdown, Constants.GameModes);
        SetOptions(_typeDropdown, Constants.ChampionTypes);

        void SetOptions(TMP_Dropdown dropdown, IEnumerable<string> values) {
            dropdown.options = values.Select(type => new TMP_Dropdown.OptionData { text = type }).ToList();
        }
    }

    public static event Action<LobbyData> LobbyCreated;

    public void OnCreateClicked() {
        var lobbyData = new LobbyData {
            lobbyName = _lobbyNameInput.text,
            playerName = _playerNameInput.text,
            gameMode = _modeDropdown.value,
            maxPlayers = _modeDropdown.value == 2 ? 4 : 6,  // if 2v2 then 4 else 6
            championType  =_typeDropdown.value
        };

        LobbyCreated?.Invoke(lobbyData);
    }
}

public struct LobbyData {
    public string lobbyName;
    public string playerName;
    public int gameMode;
    public int maxPlayers;
    public int championType;
}