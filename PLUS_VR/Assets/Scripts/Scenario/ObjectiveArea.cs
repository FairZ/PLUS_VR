using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectiveArea : MonoBehaviour {

    public bool m_objectiveTrigger = false;

    private ObjectiveSystem m_objectiveSystem;

    void Start()
    {
        m_objectiveSystem = GameObject.FindGameObjectWithTag("ObjectiveSystem").GetComponent<ObjectiveSystem>();
        gameObject.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            m_objectiveSystem.ObjectiveComplete();
        }
    }

}
