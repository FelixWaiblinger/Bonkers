[System.Serializable]
[UnityEngine.CreateAssetMenu]
public class PlayerData : SaveData
{
    // general information
    public string playerName = "";
    public string prefab = "";

    public PlayerData()
    {
        filename = "PlayerData.json";
    }
}
