using System.Collections.Generic;
using UnityEngine;

public enum PLUSEventType
{
    ObjectiveComplete,
    CorrectAnswer,
    IncorrectAnswer,
    StopAndSearchStart
}

public struct PLUSEvent
{
    public float m_time;
    public PLUSEventType m_type;

    public PLUSEvent(PLUSEventType _eventType)
    {
        m_type = _eventType;
        m_time = Time.realtimeSinceStartup;
    }
}

public static class PerformanceTracker {
    private static List<PLUSEvent> m_events;
    public static float m_startTime;
    public static float m_endTime;

    public static void StartScenario()
    {
        if (m_events == null)
            m_events = new List<PLUSEvent>();
        m_events.Clear();
        m_startTime = Time.realtimeSinceStartup;
    }

    public static void EndScenario()
    {
        m_endTime = Time.realtimeSinceStartup;
    }

    public static void AddEvent(PLUSEventType _eventType)
    {
        if (m_events == null)
            m_events = new List<PLUSEvent>();

        m_events.Add(new PLUSEvent(_eventType));
    }

    public static List<PLUSEvent> GetEvents()
    {
        if (m_events != null)
            return m_events;
        else
            return new List<PLUSEvent>();
    }
	
}
