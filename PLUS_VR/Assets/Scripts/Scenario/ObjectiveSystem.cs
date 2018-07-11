using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ObjectiveSystem : MonoBehaviour {

    
    public List<Objective> m_objectives;
    public GameObject m_scenarioNPCPrefab;
    public GameObject m_objectiveAreaPrefab;

    public Text m_TitleText;
    public Text m_descriptionText;

    public CompassRotation m_compass;

    public SteamVR_TrackedObject m_leftHand;
    private SteamVR_Controller.Device m_device;
    public float m_buzzLength = 0.2f;
    private float m_buzzTime = 0;

    Objective m_currentObjective;
    int m_currentObjectiveIndex = 0;

    void Start()
    {
        m_currentObjective = m_objectives[m_currentObjectiveIndex];
        m_currentObjective.OnBegin();
        m_TitleText.text = m_currentObjective.m_quickTitle;
        m_descriptionText.text = m_currentObjective.m_description;
        PerformanceTracker.StartScenario();
        
        switch(m_currentObjective.m_type)
        {
            case Objective.ObjectiveType.TALK_TO:
                m_compass.m_aimPoint = m_currentObjective.m_NPC.transform.position;
                m_currentObjective.m_NPC.SetObjectiveSystem(this);
                break;
            case Objective.ObjectiveType.GO_TO_AREA:
                m_compass.m_aimPoint = m_currentObjective.m_area.transform.position;
                m_currentObjective.m_area.SetObjectiveSystem(this);
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
            m_device = SteamVR_Controller.Input((int)m_leftHand.index);
            m_buzzTime = 0;
            StartCoroutine(Buzz());
            switch (m_currentObjective.m_type)
            {
                case Objective.ObjectiveType.TALK_TO:
                    m_compass.m_aimPoint = m_currentObjective.m_NPC.transform.position;
                    m_currentObjective.m_NPC.SetObjectiveSystem(this);
                    break;
                case Objective.ObjectiveType.GO_TO_AREA:
                    m_compass.m_aimPoint = m_currentObjective.m_area.transform.position;
                    m_currentObjective.m_area.SetObjectiveSystem(this);
                    break;
            }
        }
        else
        {
            m_TitleText.text = "Scenario Complete";
            PerformanceTracker.EndScenario();
            gameObject.GetComponent<SteamVR_LoadLevel>().Trigger();
            //End the scenario
        }

    }

    IEnumerator Buzz()
    {
        while(m_buzzTime < m_buzzLength)
        {
            m_device.TriggerHapticPulse(1000);
            m_buzzTime += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator LoadNextScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1, LoadSceneMode.Single);
        
        while(!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
