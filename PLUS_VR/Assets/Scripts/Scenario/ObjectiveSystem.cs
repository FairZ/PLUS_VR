using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveSystem : MonoBehaviour {

    
    public List<Objective> m_objectives;
    public GameObject m_scenarioNPCPrefab;
    public GameObject m_objectiveAreaPrefab;

    Objective m_currentObjective;
    int m_currentObjectiveIndex = 0;

    void Start()
    {
        //load scenario, spawn NPCs??
        m_currentObjective = m_objectives[m_currentObjectiveIndex];
        m_currentObjective.OnBegin();
    }

    public void ObjectiveComplete()
    {
        m_currentObjective.OnComplete();
        m_currentObjectiveIndex++;
        if(m_currentObjectiveIndex < m_objectives.Count)
        {
            m_currentObjective = m_objectives[m_currentObjectiveIndex];
            m_currentObjective.OnBegin();
        }
        else
        {
            //End the scenario
        }

    }
}
