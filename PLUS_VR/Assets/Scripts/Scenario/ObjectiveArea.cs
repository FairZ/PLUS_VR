using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectiveArea : MonoBehaviour {

    public bool m_objectiveTrigger = false;

    private ObjectiveSystem m_objectiveSystem;

    void Start()
    {
        gameObject.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            m_objectiveSystem.ObjectiveComplete();
        }
    }

    public void SetObjectiveSystem(ObjectiveSystem _objectiveSystem)
    {
        m_objectiveSystem = _objectiveSystem;
    }

}
