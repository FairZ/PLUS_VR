using System.Collections.Generic;
using UnityEngine;

public enum PLUSEventType
{
    ObjectiveComplete,
    CorrectAnswer,
    IncorrectAnswer,
    StopAndSearchStart,
    StopAndSearchEnd
}

public struct PLUSEvent
{
    float m_time;
    PLUSEventType m_type;

    public PLUSEvent(PLUSEventType _eventType)
    {
        m_type = _eventType;
        m_time = Time.realtimeSinceStartup;
    }
}

public static class PerformanceTracker {
    private static List<PLUSEvent> m_events;

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
