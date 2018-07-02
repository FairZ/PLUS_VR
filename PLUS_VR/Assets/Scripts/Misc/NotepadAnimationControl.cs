using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotepadAnimationControl : MonoBehaviour {

    Animator m_animator;
    public Transform m_camera;
    float m_closeCountdown = 2.0f;

    void Start () {
        m_animator = gameObject.GetComponent<Animator>();
	}
	
	void Update () {
        Vector3 toNotepad = transform.position - m_camera.position;
        m_closeCountdown -= Time.deltaTime;
        if(Vector3.Dot(toNotepad.normalized,m_camera.forward) > 0.9f)
        {
            m_animator.SetBool("Open", true);
            m_closeCountdown = 2.0f;
        }
        else
        {
            if(m_closeCountdown < 0)
                m_animator.SetBool("Open", false);
        }
	}
}
