using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NPC : MonoBehaviour {

    public List<string> m_dialoguePaths;
    public int m_activeDialogue = 0;

    public bool m_scenarioNPC = false;
    public bool m_objectiveTrigger = false;

    protected DialogueController m_dialogueController;

    void Start()
    {
        m_dialogueController = GameObject.FindGameObjectWithTag("DialogueController").GetComponent<DialogueController>();
        gameObject.tag = "NPC";
    }

    public virtual void Interact()
    {
        if (m_objectiveTrigger)
            m_dialogueController.m_objectiveDialogue = true;

        m_dialogueController.LoadDialogue(m_dialoguePaths[m_activeDialogue]);
    }

    public int GetNumberOfDialogues()
    {
        return m_dialoguePaths.Count;
    }
}
