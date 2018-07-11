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
    //reference to the laser pointer script to correctly hide and show the laser
    public LaserInteraction m_laserPointer;

    public bool m_objectiveDialogue = false;

    public ObjectiveSystem m_objectiveSystem;

    public StopAndSearch m_stopAndSearch;

    private bool m_shouldStart = false;

    void Start()
    {
        m_dialogueDisplay = m_dialogueDisplayObject.GetComponent<DialogueDisplay>();
        m_currentDialogue = null;
        m_currentNode = 0;
        m_objectiveSystem = GameObject.FindWithTag("ObjectiveSystem").GetComponent<ObjectiveSystem>();
    }

    void Update()
    {
        if (m_shouldStart)
            StartDialogue();
    }

    public void LoadDialogue(string _JSONFileName)
    {
        //load in the dialogue from a given path
        //set the path to the file
        
        string jsonData = Resources.Load<TextAsset>("DialogueJson/" + _JSONFileName).text ;
        m_currentDialogue = JsonUtility.FromJson<Dialogue>(jsonData);
        m_shouldStart = true;
        
    }

    private void StartDialogue()
    {
        //initialise the dialogue and show GUI
        m_currentNode = 0;
        m_laserPointer.SetTalking(true);
        m_dialogueDisplayObject.SetActive(true);
        m_dialogueDisplay.SetNameText(m_currentDialogue.GetNode(m_currentNode).m_name);
        m_dialogueDisplay.SetDialogueText(m_currentDialogue.GetNode(m_currentNode).m_text);
        m_dialogueWheel.SetOptions(m_currentDialogue.GetNode(m_currentNode));
        m_shouldStart = false;
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
        m_laserPointer.SetTalking(false);
        m_dialogueWheel.HideWheel();
        m_dialogueDisplayObject.SetActive(false);
        if(m_objectiveDialogue)
        {
            m_objectiveSystem.ObjectiveComplete();
            m_objectiveDialogue = false;
        }
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
                //Stop and search will always be the final objective so to resolve overdrawing on the results board
                //the objective is deactivated and will be completed once the stop and search is done
                m_stopAndSearch.BeginStopAndSearch();
                m_objectiveDialogue = false;
                EndDialogue();
                PerformanceTracker.AddEvent(PLUSEventType.StopAndSearchStart);
                break;
            case (int)Actions.CORRECT_ANSWER:
                PerformanceTracker.AddEvent(PLUSEventType.CorrectAnswer);
                GoToNode(chosenOption.m_destination);
                break;
            case (int)Actions.INCORRECT_ANSWER:
                PerformanceTracker.AddEvent(PLUSEventType.IncorrectAnswer);
                GoToNode(chosenOption.m_destination);
                break;
        }
    }
}
