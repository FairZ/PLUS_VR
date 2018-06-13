using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class DialogueController : MonoBehaviour {

    //stores the current dialogue
    Dialogue m_currentDialogue;
    //stores the position of the currently active node
    int m_currentNode;
    //stores a reference to the dialogue wheel on the right hand of the user
    public DialogueWheel m_dialogueWheel;
    //canvas for displaying dialogue
    public GameObject m_dialogueDisplayObject;
    //where dialogue text is shown
    private DialogueDisplay m_dialogueDisplay;

    void Start()
    {
        m_dialogueDisplay = m_dialogueDisplayObject.GetComponent<DialogueDisplay>();
        m_currentDialogue = null;
        m_currentNode = 0;
        LoadDialogue("Test");
        StartDialogue();
    }

    public void LoadDialogue(string _JSONFileName)
    {
        //load in the dialogue from a given path
        //set the path to the file
        string path = Application.dataPath + "/Resources/DialogueJson/" + _JSONFileName + ".json";
        //if the file exists read from it and load in the dialogue otherwise show error
        if (File.Exists(path))
        {
            string jsonData = File.ReadAllText(path);
            m_currentDialogue = JsonUtility.FromJson<Dialogue>(jsonData);
        }
        else
        {
            Debug.Log("invalid file name");
        }
    }

    public void StartDialogue()
    {
        //initialise the dialogue and show GUI
        m_currentNode = 0;
        m_dialogueDisplayObject.SetActive(true);
        m_dialogueDisplay.SetNameText(m_currentDialogue.GetNode(m_currentNode).m_name);
        m_dialogueDisplay.SetDialogueText(m_currentDialogue.GetNode(m_currentNode).m_text);
        m_dialogueWheel.SetOptions(m_currentDialogue.GetNode(m_currentNode));
    }

    private void GoToNode(int _ID)
    {
        m_currentNode = _ID;
        m_dialogueDisplay.SetNameText(m_currentDialogue.GetNode(m_currentNode).m_name);
        m_dialogueDisplay.SetDialogueText(m_currentDialogue.GetNode(m_currentNode).m_text);
        m_dialogueWheel.SetOptions(m_currentDialogue.GetNode(m_currentNode));
    }

    public void EndDialogue()
    {
        //end the dialogue and close the GUI
        m_currentNode = 0;
        m_dialogueWheel.HideWheel();
        m_dialogueDisplayObject.SetActive(false);
    }

    public void Choose(int _choice)
    {
        Option chosenOption = m_currentDialogue.GetNode(m_currentNode).m_options[_choice];
        switch (chosenOption.m_action)
        {
            case (int)Actions.CLOSE_DIALOGUE:
                //close the UI
                EndDialogue();
                break;
            case (int)Actions.GO_TO_NODE:
                //move to next node in dialogue
                GoToNode(chosenOption.m_destination);
                break;
            case (int)Actions.STOP_AND_SEARCH:
                ///TODO: start stop and search
                break;

        }
    }
}
