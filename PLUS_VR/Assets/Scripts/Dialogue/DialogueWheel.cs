using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueWheel : MonoBehaviour {

    //options: 0 = top, 1 = left, 2 = right, 3 = bottom
    public List<GameObject> m_options;

    public DialogueController m_dialogueController;

    public float m_delayTime = 1.0f;
    private float m_delay;

    public SteamVR_TrackedObject m_controller;
    private SteamVR_Controller.Device m_device;

    private int m_optionHover = -1;

    private bool m_active = false;

    private int m_optionCount = -1;

    void Start()
    {
        m_device = SteamVR_Controller.Input((int)m_controller.index);
    }

    void Update()
    {
        if (m_active)
        {
            m_delay -= Time.deltaTime;
                           
            //if the user's thumb is a little away from the center of the trackpad scale up the correct segment
            if (m_device.GetAxis().magnitude > 0.2 && m_delay < 0)
            {
                m_delay = -1;

                int prevOption = m_optionHover;
                m_optionHover = CalculateSegment();

                if (prevOption != m_optionHover)
                {
                    if (prevOption != -1)
                    {
                        m_options[prevOption].transform.localScale = new Vector3(1, 1, 1);
                        m_options[prevOption].GetComponentInChildren<Image>().color = new Color(1, 1, 1, 0.6f);
                    }
                    m_options[m_optionHover].transform.localScale = new Vector3(2, 2, 2);
                    m_options[m_optionHover].GetComponentInChildren<Image>().color = Color.white;
                    //when the trackpad is pressed send the choice if the option was valid
                }
                if (m_device.GetPressDown(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad) && m_optionHover != -1 && m_optionHover <= m_optionCount)
                {
                    m_dialogueController.Choose(m_optionHover);
                }
            }
            //otherwise reset the scaled segment and set option to void
            else
            {
                if (m_optionHover != -1)
                {
                    m_options[m_optionHover].transform.localScale = new Vector3(1, 1, 1);
                    m_options[m_optionHover].GetComponentInChildren<Image>().color = new Color(1, 1, 1, 0.6f);
                }
                m_optionHover = -1;
            }
            
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

    public void SetOptions(Node _dialogueNode)
    {
        //activate the wheel and reset the option count
        m_active = true;
        m_delay = m_delayTime;
        m_optionCount = -1;
        //activate each option and set its text
        for (int i = 0; i < _dialogueNode.m_options.Count; i++)
        {
            m_options[i].SetActive(true);
            m_options[i].GetComponentInChildren<Text>().text = _dialogueNode.m_options[i].m_text;
            //increment the option count for each available option
            m_optionCount++;
        }
        //hide any unneccesary options
        for (int j = m_optionCount+1; j < 4; j++)
        {
            m_options[j].SetActive(false);
        }
    }

    public void HideWheel()
    {
        //deactivate the wheel and hide all segments
        m_active = false;
        for (int i = 0; i < 4; i++)
        {
            m_options[i].SetActive(false);
        }
    }
}
