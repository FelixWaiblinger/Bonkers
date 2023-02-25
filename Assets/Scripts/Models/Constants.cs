using System.Collections.Generic;

public class Constants {
    public const string JoinKey = "j";
    public const string ChampionTypeKey = "d";
    public const string GameModeKey = "t";

    public static readonly List<string> ChampionTypes = new() { "Melee", "Ranged", "Support" };
    public static readonly List<string> GameModes = new() { "LMS", "3v3", "2v2" };
}