using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopAndSearch : MonoBehaviour {

    public StopAndSearchCollider m_correctCollider;
    public List<GameObject> m_allSSColliders;
    public List<SphereCollider> m_handColliders;

    public List<GameObject> m_handObjectsToHide;
    public List<GameObject> m_handObjectsToShow;

    private Animator m_animator;

    public ObjectiveSystem m_objectiveSystem;

    public LaserInteraction m_laser;

    void Start()
    {
        m_animator = gameObject.GetComponent<Animator>();

    }

    public void BeginStopAndSearch()
    {
        m_animator.SetBool("Stop&Search", true);
        foreach(GameObject go in m_allSSColliders)
        {
            go.GetComponent<CapsuleCollider>().enabled = true;
        }
        m_correctCollider.m_correctCollider = true;
        m_correctCollider.m_stopAndSearch = this;

        foreach(GameObject go in m_handObjectsToHide)
        {
            go.SetActive(false);
        }
        foreach(GameObject go in m_handObjectsToShow)
        {
            go.SetActive(true);
        }
        m_objectiveSystem = GameObject.FindGameObjectWithTag("ObjectiveSystem").GetComponent<ObjectiveSystem>();
        foreach(SphereCollider s in m_handColliders)
        {
            s.enabled = true;
        }
        m_laser.SetStopAndSearch(true);
    }

    public void StopAndSearchComplete()
    {
        m_objectiveSystem.ObjectiveComplete();
    }
    
}
