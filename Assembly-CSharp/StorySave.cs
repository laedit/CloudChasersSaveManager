using System;

[Serializable]
public class StorySave
{
    public bool SaveQuest
    {
        get;
        set;
    }

    public bool SaveDecision
    {
        get;
        set;
    }

    public int QuestID
    {
        get;
        set;
    }

    public int DecisionID
    {
        get;
        set;
    }

    public StorySave(bool saveQuest, bool saveDecision, int saveId, int decisionNumber)
    {
        this.SaveQuest = saveQuest;
        this.SaveDecision = saveDecision;
        this.QuestID = saveId;
        this.DecisionID = decisionNumber;
    }

    public StorySave()
    {

    }
}