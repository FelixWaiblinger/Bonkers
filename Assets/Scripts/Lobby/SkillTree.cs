using System.Collections.Generic;
using UnityEngine;

public class SkillTree : MonoBehaviour
{
    [SerializeField] private SkillData _chosenSkills;
    [SerializeField] private Dictionary<string, float> _skillValues = new Dictionary<string, float> {
        // minor
        {"ad", 1.1f}, {"ap", 1.1f}, {"critRate", 1.1f}, {"aoe", 1.1f}, {"range", 1.1f}, {"health", 1.1f},
        {"tenacity", 0.9f}, {"armor", 1.1f}, {"resistance", 1.1f}, {"oocSpeed", 1.1f}, {"selfHeal", 1.1f},
        {"allyHeal", 1.1f}, {"speed", 1.1f}, {"abilityCD", 0.9f},

        // major
        {"armorPen", 1.1f}, {"critDmg", 1.1f}, {"resistancePen", 1.1f}, {"dot", 1.1f}, {"oocRegen", 1.1f},
        {"open1", 0}, {"shield", 1.1f}, {"shieldCD", 0.9f}, {"open2", 0}, {"slowImmun", 1.1f}, {"open3", 0},
        {"open4", 0}, {"open5", 0}, {"abilityCost", 0.9f}, {"attackCD", 0.9f}, {"dashRange", 1.1f},
        {"projectileSpeed", 1.1f}, {"open6", 0}, {"homing", 1},

        // epic
        {"attackSlow", 1}, {"shieldDmg", 1}, {"attackHeal", 1}, {"extraDash", 1},

        // legendary
        {"percHealthDmg", 1}, {"autoCleanse", 1}, {"flurryShot", 1}
    };
    [SerializeField] private int _skillPoints = 17;
    
    private Dictionary<string, int> _skillCosts = new Dictionary<string, int> {
        // primary
        {"strength", 1}, {"defense", 1}, {"utility", 1},

        // minor
        {"ad", 1}, {"ap", 1}, {"critRate", 1}, {"aoe", 1}, {"range", 1}, {"health", 1}, {"tenacity", 1},
        {"armor", 1}, {"resistance", 1}, {"oocSpeed", 1}, {"selfHeal", 1}, {"allyHeal", 1}, {"speed", 1},
        {"abilityCD", 1},

        // major
        {"armorPen", 2}, {"critDmg", 2}, {"resistancePen", 2}, {"dot", 2}, {"oocRegen", 2}, {"open1", 2},
        {"shield", 2}, {"shieldCD", 2}, {"open2", 2}, {"slowImmun", 2}, {"open3", 2}, {"open4", 2},
        {"open5", 2}, {"abilityCost", 2}, {"attackCD", 2}, {"dashRange", 2}, {"projectileSpeed", 2},
        {"open6", 2}, {"homing", 2},

        // epic
        {"attackSlow", 3}, {"shieldDmg", 3}, {"attackHeal", 3}, {"extraDash", 3},

        // legendary
        {"percHealthDmg", 3}, {"autoCleanse", 3}, {"flurryShot", 3}
    };

    // function for each skill button
    public void ToggleSkill(string name)
    {
        // TODO check if skill is enabled already
        _chosenSkills.data[name] = !_chosenSkills.data[name];
    }

    // save choices in persistentDataPath upon closing the skill tree editor
    public void SaveSkillTree()
    {
        ReaderWriterJSON.SaveToJSON(_chosenSkills);
    }

    // load choices from persistentDataPath upon opening the skill tree editor
    public void LoadSkillTree()
    {
        ReaderWriterJSON.LoadFromJSON<SkillData>(ref _chosenSkills);

        int necessaryPoints = 0;
        foreach (var (skillName, isChosen) in _chosenSkills.data)
        {
            if (isChosen) necessaryPoints += _skillCosts[skillName];
        }

        // reset skill tree if too many chosen skills were found (GRR CHEATER DETECTED)
        if (necessaryPoints > _skillPoints) _chosenSkills = new SkillData();

        // TODO also check whether chosen skills make sense (connection-wise)
    }
}
