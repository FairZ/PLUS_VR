using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueDisplay : MonoBehaviour {

    public Text m_dialogueText;
    public Text m_nameText;

    public void SetDialogueText(string _text)
    {
        m_dialogueText.text = _text;
    }

    public void SetNameText(string _name)
    {
        m_nameText.text = _name;
    }
}
