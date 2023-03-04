[System.Serializable]
[UnityEngine.CreateAssetMenu]
public class LobbyData : SaveData
{
    public ulong clientId = 0;
    public string lobbyName = "";
    public string gameMode = "";
    public int teamSize = -1;

    // public SerializableDictUlongTuple players = new SerializableDictUlongTuple();
    public SerializableDictStringInt players = new SerializableDictStringInt();

    public LobbyData()
    {
        filename = "LobbyData.json";
    }
}
