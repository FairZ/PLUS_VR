using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenarioNPC : NPC {

	public void ChangeActiveDialogue(int _index)
    {
        if(_index >= 0 && _index < m_dialoguePaths.Count)
            m_activeDialogue = _index;
    }
}
