using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue {

    public List<Node> m_nodes;

    public Dialogue()
    {
        m_nodes = new List<Node>();
    }

    public void AddNode()
    {
        Node newNode = new Node();
        newNode.m_id = m_nodes.Count;
        m_nodes.Add(newNode);        
    }

    public void DeleteNode(int pos)
    {
        if(pos > -1 && pos < m_nodes.Count)
        {
            m_nodes.RemoveAt(pos);
        }
    }

}
