[System.Serializable]
public class Objective {

    public enum ObjectiveType
    {
        TALK_TO,
        GO_TO_AREA
    }

    //common variables
    public string m_quickTitle;
    public string m_description;
    public bool m_complete = false;
    public ObjectiveType m_type;

    //Talk-To variables
    public NPC m_NPC;
    public int m_objectiveDialogueIndex;
    public int m_postObjectiveDialogueIndex;

    //Go-To-Area variables

    public virtual void OnBegin()
    {
        switch(m_type)
        {
            case ObjectiveType.TALK_TO:
                m_NPC.m_activeDialogue = m_objectiveDialogueIndex;
                m_NPC.m_objectiveTrigger = true;
                break;
            case ObjectiveType.GO_TO_AREA:
                //add code here
                break;
        }
    }

    public virtual void OnComplete()
    {
        switch (m_type)
        {
            case ObjectiveType.TALK_TO:
                m_NPC.m_activeDialogue = m_postObjectiveDialogueIndex;
                m_NPC.m_objectiveTrigger = false;
                break;
            case ObjectiveType.GO_TO_AREA:
                //add code here
                break;
        }
    }

}
