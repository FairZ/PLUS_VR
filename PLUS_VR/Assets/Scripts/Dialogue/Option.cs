using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Actions
{
    CLOSE_DIALOGUE,
    GO_TO_NODE,
    STOP_AND_SEARCH
};

//each node of dialogue contains a list of options which determine what occurs when that option is selected
[System.Serializable]
public class Option {
    //text should be displayed on a dialogue wheel
    public string m_text;
    //destination is the ID of the Node you wish to go to if using a "Go To Node" action
    public int m_destination;
    //action is the variety of action that activating the option will trigger
    //see Actions enum for possible actions and their int value
    //NB: adding additional actions to the Actions dialogue will not add them to the dialogue editor and must be added manually there
    public int m_action;
}
