using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportMovement : MonoBehaviour {

    public GameObject m_player;
    public GameObject m_head;

    public GameObject m_reticulePrefab;
    private GameObject m_reticule;
    private Transform m_reticuleTransform;
    public Vector3 m_reticuleOffsetFromRayHit;

    public GameObject m_teleportButton;
    bool m_teleportButtonAvailable = false;

    [ColorUsageAttribute(true, true)] public Color m_validColour;
    [ColorUsageAttribute(true, true)] public Color m_invalidColour;

    private LineRenderer m_line;

    private SteamVR_TrackedObject m_controller;
    private SteamVR_Controller.Device m_device;

    private LaserInteraction m_laser;

    void Start()
    {
        m_line = gameObject.GetComponent<LineRenderer>();
        m_controller = gameObject.GetComponent<SteamVR_TrackedObject>();
        m_device = SteamVR_Controller.Input((int)m_controller.index);
        m_laser = gameObject.GetComponent<LaserInteraction>();
        m_reticule = Instantiate(m_reticulePrefab);
        m_reticule.SetActive(false);
        m_reticuleTransform = m_reticule.transform;
    }

    void Update()
    {
        if(m_device.GetPress(Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger))
        {
            m_laser.m_laserActive = false;
            //m_teleportButton.SetActive(true);
            m_teleportButtonAvailable = false;
            m_line.SetPosition(1, new Vector3(0, 0, 10));
            m_line.material.SetColor("_EmissionColor", m_invalidColour);
            m_reticule.SetActive(false);
            RaycastHit hit;
            if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 10))
            {
                m_line.SetPosition(1, new Vector3(0, 0, hit.distance));
                if (hit.transform.gameObject.tag == "TeleportSurface")
                {
                    m_teleportButtonAvailable = true;
                    m_line.material.SetColor("_EmissionColor", m_validColour);
                    m_reticule.SetActive(true);
                    m_reticuleTransform.position = hit.point +m_reticuleOffsetFromRayHit;
                    m_reticuleTransform.rotation = Quaternion.Euler(0, m_head.transform.rotation.eulerAngles.y, 0);
                }
            }
        }
        else
        {
            m_laser.m_laserActive = true;
            //m_teleportButton.SetActive(false);
            m_reticule.SetActive(false);
        }
    }
}
