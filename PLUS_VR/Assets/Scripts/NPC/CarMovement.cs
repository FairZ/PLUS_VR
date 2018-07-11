using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CarMovement : MonoBehaviour {

    public List<Transform> m_points;
    public int m_destIndex;

    private NavMeshAgent m_agent;

    void Start()
    {
        m_agent = gameObject.GetComponent<NavMeshAgent>();
        m_agent.SetDestination(m_points[m_destIndex].position);
    }

    void Update()
    {
        if(!m_agent.pathPending && m_agent.remainingDistance<0.5f)
        {
            m_destIndex = (m_destIndex + 1) % m_points.Count;
            m_agent.SetDestination(m_points[m_destIndex].position);
        }
    }
}
