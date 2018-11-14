using System;
using System.Collections.Generic;

[Serializable]
public class SaveFile
{
    public static SaveFile current;

    public bool canLoad;

    public float HealthFrancisco
    {
        get;
        set;
    }

    public float HealthAmelia
    {
        get;
        set;
    }

    public float HealthGlider
    {
        get;
        set;
    }

    public KeyValuePair<bool, bool> FractureSickFrancisco
    {
        get;
        set;
    }

    public KeyValuePair<bool, bool> FractureSickAmelia
    {
        get;
        set;
    }

    public bool AmeliaImmunity
    {
        get;
        set;
    }

    public bool FranciscoImmunity
    {
        get;
        set;
    }

    public float CurrentWater
    {
        get;
        set;
    }

    public List<KeyValuePair<int, int>> MainInventory
    {
        get;
        set;
    }

    public List<KeyValuePair<int, int>> AmeliaInventory
    {
        get;
        set;
    }

    public List<KeyValuePair<int, int>> FranciscoInventory
    {
        get;
        set;
    }

    public List<KeyValuePair<int, int>> GliderWingInventory
    {
        get;
        set;
    }

    public List<KeyValuePair<int, int>> GliderBodyInventory
    {
        get;
        set;
    }

    public List<KeyValuePair<int, int>> GliderNetInventory
    {
        get;
        set;
    }

    public int seed
    {
        get;
        set;
    }

    public List<KeyValuePair<int, SerializedVector3>> QuestList
    {
        get;
        set;
    }

    public List<int> UsedQuests
    {
        get;
        set;
    }

    public List<int> GlobalUsedQuests
    {
        get;
        set;
    }

    public List<int> StoredSequels
    {
        get;
        set;
    }

    public List<KeyValuePair<int, SerializedVector3>> CloudList
    {
        get;
        set;
    }

    public SerializedVector3 windDirection
    {
        get;
        set;
    }

    public KeyValuePair<SerializedVector3, SerializedVector3> playerPosRot
    {
        get;
        set;
    }

    public List<SerializedVector3> MapHarvesters
    {
        get;
        set;
    }

    public List<SerializedVector3> PatrolList
    {
        get;
        set;
    }

    public List<KeyValuePair<int[], SerializedVector3>> HiddenItemsList
    {
        get;
        set;
    }

    public List<KeyValuePair<int, SerializedVector3>> AHRelictsList
    {
        get;
        set;
    }

    public List<KeyValuePair<int, SerializedVector3>> PlantsList
    {
        get;
        set;
    }

    public List<StorySave> QuestLog
    {
        get;
        set;
    }

    public int CurrentDesertNumber
    {
        get;
        set;
    }

    public GenerationManager.DesertType CurrentDesertType
    {
        get;
        set;
    }

    public float CurrentPlayTime
    {
        get;
        set;
    }

    public List<KeyValuePair<int, int>> TraderInventory
    {
        get;
        set;
    }
}
