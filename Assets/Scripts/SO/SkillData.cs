[System.Serializable]
[UnityEngine.CreateAssetMenu]
public class SkillData : SaveData
{    
    // primary
    public SerializableDictStringBool data = new SerializableDictStringBool() {
        // primary
        {"strength", false},
        {"defense", false},
        {"utility", false},

        // minor
        {"ad", false},
        {"ap", false},
        {"critRate", false},
        {"aoe", false},
        {"range", false},
        {"health", false},
        {"tenacity", false},
        {"armor", false},
        {"resistance", false},
        {"oocSpeed", false},
        {"selfHeal", false},
        {"allyHeal", false},
        {"speed", false},
        {"abilityCD", false},

        // major
        {"armorPen", false},
        {"critDmg", false},
        {"resistancePen", false},
        {"dot", false},
        {"oocRegen", false},
        {"open1", false},
        {"shield", false},
        {"shieldCD", false},
        {"open2", false},
        {"slowImmun", false},
        {"open3", false},
        {"open4", false},
        {"open5", false},
        {"abilityCost", false},
        {"attackCD", false},
        {"dashRange", false},
        {"projectileSpeed", false},
        {"open6", false},
        {"homing", false},

        // epic
        {"attackSlow", false},
        {"shieldDmg", false},
        {"attackHeal", false},
        {"extraDash", false},

        // legendary
        {"percHealthDmg", false},
        {"autoCleanse", false},
        {"flurryShot", false}
    };

    public SkillData()
    {
        base.filename = "SkillData.json";
    }
}
