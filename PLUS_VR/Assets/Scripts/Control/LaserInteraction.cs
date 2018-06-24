using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LaserInteraction : MonoBehaviour {

    public GameObject m_chatButton;
    bool m_chatButtonActive = false;

    [ColorUsageAttribute(true, true)] public Color m_standardColour;
    [ColorUsageAttribute(true, true)] public Color m_hoverColour;

    public DialogueController m_dialogueController;

    private bool m_talking = false;

    private LineRenderer m_line;

    private SteamVR_TrackedObject m_controller;
    private SteamVR_Controller.Device m_device;

    public bool m_laserActive = true;

    void Start()
    {
        m_line = gameObject.GetComponent<LineRenderer>();
        m_controller = gameObject.GetComponent<SteamVR_TrackedObject>();
        m_device = SteamVR_Controller.Input((int)m_controller.index);

    }

    void FixedUpdate()
    {
        m_chatButtonActive = false;
        if (!m_talking&&m_laserActive)
        {
            m_line.material.SetColor("_EmissionColor", m_standardColour);
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 1))
            {
                m_line.SetPosition(1, new Vector3(0, 0, hit.distance));
                if (hit.transform.tag == "NPC")
                {
                    m_chatButtonActive = true;
                    m_line.material.SetColor("_EmissionColor", m_hoverColour);
                    if(m_device.GetTouch(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad))
                    {
                        m_chatButton.GetComponent<Image>().color = Color.white;
                    }
                    else
                    {
                        m_chatButton.GetComponent<Image>().color = new Color(1, 1, 1, 0.6f);
                    }
                    if(m_device.GetPressDown(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad))
                    {
                        hit.transform.gameObject.GetComponent<NPC>().Interact();
                    }
                }
            }
            else
            {
                m_line.SetPosition(1, new Vector3(0, 0, 1));
            }
        }
        m_chatButton.SetActive(m_chatButtonActive);
    }

    public void SetTalking(bool _talking)
    {
        m_talking = _talking;
        m_line.enabled = !_talking;
    }
}
