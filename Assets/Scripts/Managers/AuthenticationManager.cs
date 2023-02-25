using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class AuthenticationManager : MonoBehaviour {

    [SerializeField] private TMP_InputField _playerNameInput;
    [SerializeField] private PlayerData _playerData;

    void Awake()
    {
        if (_playerData.playerName == "")
        {
            // check if some player data already exists
            ReaderWriterJSON.LoadFromJSON<PlayerData>(ref _playerData);
        }
        
        // display player name if existing player data is found
        if (_playerData.playerName != "") _playerNameInput.text = _playerData.playerName;
    }

    public async void LoginAnonymously() {
        if (_playerNameInput.text == "")
        {
            // TODO make this error message appear on screen
            Debug.Log("Error - Enter a name before you log in!");
            return;
        }

        // save player name for next startup
        _playerData.playerName = _playerNameInput.text;
        ReaderWriterJSON.SaveToJSON(_playerData);

        // log in without any "account"
        using (new Load("Logging you in...")) {
            await Authentication.Login();
            SceneManager.LoadSceneAsync("Lobby");
        }
    }
}