using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectiveSystem : MonoBehaviour {

    [HideInInspector]
    public List<Objective> m_objectives;
    public GameObject m_scenarioNPCPrefab;
    public GameObject m_objectiveAreaPrefab;

    public Text m_TitleText;
    public Text m_descriptionText;

    public CompassRotation m_compass;

    Objective m_currentObjective;
    int m_currentObjectiveIndex = 0;

    void Start()
    {
        //load scenario, spawn NPCs??
        m_currentObjective = m_objectives[m_currentObjectiveIndex];
        m_currentObjective.OnBegin();
        m_TitleText.text = m_currentObjective.m_quickTitle;
        m_descriptionText.text = m_currentObjective.m_description;
        
        switch(m_currentObjective.m_type)
        {
            case Objective.ObjectiveType.TALK_TO:
                m_compass.m_aimPoint = m_currentObjective.m_NPC.transform.position;
                break;
            case Objective.ObjectiveType.GO_TO_AREA:
                m_compass.m_aimPoint = m_currentObjective.m_area.transform.position;
                break;
        }
        
    }

    public void ObjectiveComplete()
    {
        PerformanceTracker.AddEvent(PLUSEventType.ObjectiveComplete);
        m_currentObjective.OnComplete();
        m_currentObjectiveIndex++;
        if(m_currentObjectiveIndex < m_objectives.Count)
        {
            m_currentObjective = m_objectives[m_currentObjectiveIndex];
            m_currentObjective.OnBegin();
            m_TitleText.text = m_currentObjective.m_quickTitle;
            m_descriptionText.text = m_currentObjective.m_description;

            switch (m_currentObjective.m_type)
            {
                case Objective.ObjectiveType.TALK_TO:
                    m_compass.m_aimPoint = m_currentObjective.m_NPC.transform.position;
                    break;
                case Objective.ObjectiveType.GO_TO_AREA:
                    m_compass.m_aimPoint = m_currentObjective.m_area.transform.position;
                    break;
            }
        }
        else
        {
            m_TitleText.text = "Scenario Complete";
            m_descriptionText.text = "Look at your score and then take off the Headset";
            //End the scenario
        }

    }
}
