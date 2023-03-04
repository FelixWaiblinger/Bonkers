using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class CreateLobbyScreen : MonoBehaviour {
    [SerializeField] private TMP_InputField _lobbyNameInput, _playerNameInput;
    [SerializeField] private TMP_Dropdown _modeDropdown, _typeDropdown;
    [SerializeField] private LobbyData _lobbyData;

    private void Start() {
        SetOptions(_modeDropdown, Constants.GameModes);

        void SetOptions(TMP_Dropdown dropdown, IEnumerable<string> values) {
            dropdown.options = values.Select(type => new TMP_Dropdown.OptionData { text = type }).ToList();
            int index = Constants.GameModes.IndexOf(_lobbyData.gameMode);
            dropdown.value = index >= 0 ? index : 0;
        }
    }

    public static event Action<LobbyInfo> LobbyCreated;

    public void OnCreateClicked() {
        var lobbyData = new LobbyInfo {
            lobbyName = _lobbyNameInput.text,
            gameMode = _modeDropdown.value,
            // if LMS then 8 else if 3v3 then 6 else 4 (2v2)
            maxPlayers = _modeDropdown.value == 0 ? 8 : _modeDropdown.value == 1 ? 6 : 4
        };

        // update player data for next start up
        _lobbyData.lobbyName = _lobbyNameInput.text;
        _lobbyData.gameMode = Constants.GameModes[_modeDropdown.value];
        _lobbyData.teamSize = lobbyData.maxPlayers < 8 ? (lobbyData.maxPlayers / 2) : 1;
        ReaderWriterJSON.SaveToJSON(_lobbyData);

        LobbyCreated?.Invoke(lobbyData);
    }
}

public struct LobbyInfo {
    public string lobbyName;
    public int gameMode;
    public int maxPlayers;
}