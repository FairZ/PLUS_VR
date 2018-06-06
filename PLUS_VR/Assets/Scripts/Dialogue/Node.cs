using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//each node represents a new section of conversation
[System.Serializable]
public class Node{
    //id refers to its position in the dialogue container (used to set destinations in options)
    public int m_id;
    //name is the name of the person who is talking
    public string m_name;
    //text is the section of dialogue to be displayed
    public string m_text;
    //list of possible options
    public List<Option> m_options;

    public Node()
    {
        m_options = new List<Option>();
    }

    public void AddOption()
    {
        m_options.Add(new Option());
    }

}
