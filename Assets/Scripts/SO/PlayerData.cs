[System.Serializable]
[UnityEngine.CreateAssetMenu]
public class PlayerData : SaveData
{
    // general information
    public string playerName = "";
    public string prefab = "";

    // defensive stats
    public int speed;
    public int health;
    public int armor;
    public int resistance;
    public int shield;

    // offensive stats
    public int ad;
    public int ap;
    public float armorPen;
    public float magicPen;
    public float range;
    public float cooldown;
    
    // other

    public PlayerData()
    {
        base.filename = "PlayerData.json";
    }
}
