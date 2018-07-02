using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompassRotation : MonoBehaviour {

    public Vector3 m_aimPoint;
	
	
	void Update () {
        
        transform.LookAt(m_aimPoint, transform.up);
        transform.localRotation = Quaternion.Euler(0,transform.localRotation.eulerAngles.y + 180, 0);
	}
}
