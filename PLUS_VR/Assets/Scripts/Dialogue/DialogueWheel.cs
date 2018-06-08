using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueWheel : MonoBehaviour {

    public List<GameObject> m_options;

    public DialogueController m_dialogueController;

    public SteamVR_TrackedObject m_controller;
    private SteamVR_Controller.Device m_device;

    private int m_optionHover = -1;

    void Start()
    {
        m_device = SteamVR_Controller.Input((int)m_controller.index);
    }

    void Update()
    {
        //if the user's thumb is a little away from the center of the trackpad
        if(m_device.GetAxis().magnitude > 0.2)
        {
            int prevOption = m_optionHover;
            m_optionHover = CalculateSegment();

            if(prevOption != m_optionHover)
            {
                if(prevOption != -1)
                {
                    m_options[prevOption].transform.localScale = new Vector3(1,1,1);
                }
                m_options[m_optionHover].transform.localScale = new Vector3(1.7f, 1.7f, 1.7f);
            }
        }
        else
        {
            if (m_optionHover != -1)
            {
                m_options[m_optionHover].transform.localScale = new Vector3(1, 1, 1);
            }
            m_optionHover = -1;
        }
    }

    private int CalculateSegment()
    {
        //function to determine the segment that the user's thumb is in
        //this could most likely be done better using vector maths or by having the options
        //in a square pattern as opposed to their current diamond
        Vector2 axis = m_device.GetAxis();
        int segment = -1;
        if (axis.y > 0)
        {
            if (axis.x > 0)
            {
                if (axis.y > axis.x)
                {
                    segment = 0;
                }
                else
                {
                    segment = 2;
                }
            }
            else
            {
                if (axis.y > -axis.x)
                {
                    segment = 0;
                }
                else
                {
                    segment = 1;
                }
            }
        }
        else
        {
            if (axis.x > 0)
            {
                if (axis.y < -axis.x)
                {
                    segment = 3;
                }
                else
                {
                    segment = 2;
                }
            }
            else
            {
                if (axis.y < axis.x)
                {
                    segment = 3;
                }
                else
                {
                    segment = 1;
                }
            }
        }
        return segment;
    }

}
