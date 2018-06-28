using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReticuleController : MonoBehaviour {

    public GameObject m_surface;
    public GameObject m_joints;

	public void ShowBody(bool _show)
    {
        m_surface.SetActive(_show);
        m_joints.SetActive(_show);
    }
	
}
