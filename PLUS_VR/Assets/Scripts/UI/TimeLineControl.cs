using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeLineControl : MonoBehaviour {

    public Text m_midLeft;
    public Text m_middle;
    public Text m_midRight;
    public Text m_end;
    public GameObject m_eventPrefab;

    void Start()
    {
        List<PLUSEvent> m_events = PerformanceTracker.GetEvents();
        float duration = PerformanceTracker.m_endTime - PerformanceTracker.m_startTime;
        m_midLeft.text = ParseTimeFromFloat(duration / 4);
        m_middle.text = ParseTimeFromFloat(duration / 2);
        m_midRight.text = ParseTimeFromFloat((duration / 4) * 3);
        m_end.text = ParseTimeFromFloat(duration);
        foreach(PLUSEvent e in m_events)
        {
            float fraction = (e.m_time - PerformanceTracker.m_startTime) / duration;
            GameObject eventdisplay = GameObject.Instantiate(m_eventPrefab,transform) ;
            eventdisplay.transform.localPosition += new Vector3(fraction* 1.8f, 0, 0);
            eventdisplay.GetComponentInChildren<Image>().color = new Color(0, 155.0f/255.0f, 191.0f / 255.0f);
            switch (e.m_type)
            {
                case PLUSEventType.CorrectAnswer:
                    eventdisplay.GetComponent<Text>().text = "Correct Answer";
                    eventdisplay.GetComponentInChildren<Image>().color = new Color(5.0f / 255.0f, 191.0f / 255.0f, 0);
                    break;
                case PLUSEventType.IncorrectAnswer:
                    eventdisplay.GetComponent<Text>().text = "Incorrect Answer";
                    eventdisplay.GetComponentInChildren<Image>().color = new Color(191.0f / 255.0f, 2.0f / 255.0f, 0);
                    break;
                case PLUSEventType.ObjectiveComplete:
                    eventdisplay.GetComponentInChildren<Text>().text = "Objective Complete";
                    break;
                case PLUSEventType.StopAndSearchStart:
                    eventdisplay.GetComponentInChildren<Text>().text = "S&S Start";
                    break;
            }
            
            
        }
    }

    string ParseTimeFromFloat(float _time)
    {
        string retVal = null;
        int minutes = (int)((_time-(_time%60)) / 60);
        int seconds = (int)(_time%60);
        string minPad = "";
        if(minutes <10)
        {
            minPad = "0";
        }
        string secPad = "";
        if(seconds <10)
        {
            secPad = "0";
        }

        retVal = minPad + minutes.ToString() + ":" + secPad + seconds.ToString();

        return retVal;
    }

}
