using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour {

    public List<string> m_dialoguePaths;
    protected int m_activeDialogue = 0;

    public DialogueController m_dialogueController;

    void Start()
    {
        m_dialogueController = GameObject.FindGameObjectWithTag("DialogueController").GetComponent<DialogueController>();
        gameObject.tag = "NPC";
    }

    public void Interact()
    {
        m_dialogueController.LoadDialogue(m_dialoguePaths[m_activeDialogue]);
        m_dialogueController.StartDialogue();
    }
}
