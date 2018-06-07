using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Serializable to be able to load and save from Json
//NB: all variables which you wish to save to Json MUST be PUBLIC or use the [SerializeField] tag
[System.Serializable]
public class Dialogue {
    //contains all speech Nodes for a specific piece of dialogue
    public List<Node> m_nodes;

    //default constructor to intialise list
    public Dialogue()
    {
        m_nodes = new List<Node>();
    }

    //add a new Node with its ID set to its position in the list
    public void AddNode()
    {
        Node newNode = new Node();
        newNode.m_id = m_nodes.Count;
        m_nodes.Add(newNode);        
    }

    public void DeleteNode(int pos)
    {
        //remove a node if the given position is within bounds
        if(pos > -1 && pos < m_nodes.Count)
        {
            m_nodes.RemoveAt(pos);
        }
    }

}
