using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotepadAnimationControl : MonoBehaviour {

    Animator m_animator;
    public Transform m_camera;

    void Start () {
        m_animator = gameObject.GetComponent<Animator>();
	}
	
	void Update () {
        Vector3 toNotepad = transform.position - m_camera.position;

        if(Vector3.Dot(toNotepad.normalized,m_camera.forward) > 0.9f)
        {
            m_animator.SetBool("Open", true);
        }
        else
        {
            m_animator.SetBool("Open", false);
        }
	}
}
