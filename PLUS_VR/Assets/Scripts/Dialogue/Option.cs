using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//each node of dialogue contains a list of options
[System.Serializable]
public class Option {
    //text should be displayed on a dialogue wheel
    public string m_text;
    //destination is the position of the next node
    public int m_destination;
    //action is the variety of action that activating the option will trigger
    //(go to destination, exit dialogue, start SaS)
    public int m_action;

}
