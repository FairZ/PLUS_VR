using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueController : MonoBehaviour {

    //stores the current dialogue
    Dialogue m_currentDialogue;
    //stores the position of the current node
    int m_currentNode;

    void Start()
    {
        m_currentDialogue = null;
        m_currentNode = 0;
    }

    public void LoadDialogue(string _path)
    {
        //load in the dialogue from a given path
    }

    public void StartDialogue()
    {
        //initialise the dialogue and show GUI
    }

    public void EndDialogue()
    {
        //end the dialogue and close the GUI
    }
}
