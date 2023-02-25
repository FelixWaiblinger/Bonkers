using TMPro;
using UnityEngine;

public class LobbyPlayerPanel : MonoBehaviour {
    [SerializeField] private TMP_Text _nameText, _statusText;

    public ulong PlayerId { get; private set; }

    public void Init(ulong playerId, string playerName) {
        PlayerId = playerId;
        _nameText.text = $"{playerName}";
    }

    public void SetReady() {
        _statusText.text = "Ready";
        _statusText.color = Color.green;
    }
}