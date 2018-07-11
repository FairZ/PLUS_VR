using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TireSpin : MonoBehaviour {

	public float m_speed;

    public bool m_reverse;

    // Update is called once per frame
    void Update () {
        int reverser = 1;
        if (m_reverse)
            reverser = -1;
        transform.Rotate(m_speed*reverser, 0, 0, Space.Self);
	}
}
