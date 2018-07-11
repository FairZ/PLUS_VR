using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopAndSearchCollider : MonoBehaviour {

    public bool m_correctCollider = false;

    private CapsuleCollider m_capsuleCollider;

    public StopAndSearch m_stopAndSearch;

	void Start () {
        m_capsuleCollider = gameObject.GetComponent<CapsuleCollider>();
	}

    void OnTriggerStay(Collider other)
    {
        if (m_correctCollider == true)
        {
            SteamVR_Controller.Device device = SteamVR_Controller.Input((int)other.gameObject.GetComponent<SteamVR_TrackedObject>().index);
            device.TriggerHapticPulse(2000);
            if (device.GetPressDown(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad))
            {
                m_stopAndSearch.StopAndSearchComplete();
            }
        }
        else
        {
            SteamVR_Controller.Device device = SteamVR_Controller.Input((int)other.gameObject.GetComponent<SteamVR_TrackedObject>().index);
            device.TriggerHapticPulse(100);
        }
    }
    
}
