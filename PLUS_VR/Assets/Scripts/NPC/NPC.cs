using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NPC : MonoBehaviour {

    public List<string> m_dialoguePaths;
    public int m_activeDialogue = 0;

    public bool m_scenarioNPC = false;
    public bool m_objectiveTrigger = false;

    private bool m_lookAtPlayer;
    public GameObject m_playerHead;
    public float m_headTurnSpeed;
    private Animator m_anim;
    private float m_lookAtWeight = 0;

    public DialogueController m_dialogueController;

    private ObjectiveSystem m_objectiveSystem;

    void Start()
    {
        Debug.Log(GameObject.FindGameObjectsWithTag("DialogueController").Length);
        m_dialogueController = GameObject.FindGameObjectWithTag("DialogueController").GetComponent<DialogueController>();
        gameObject.tag = "NPC";
        m_anim = gameObject.GetComponent<Animator>();
    }

    public virtual void Interact()
    {
        if (m_objectiveTrigger)
        {
            m_dialogueController.m_objectiveDialogue = true;
            m_dialogueController.m_objectiveSystem = m_objectiveSystem;
        }
        m_lookAtPlayer = true;
        StartCoroutine(LerpToOne());
        m_dialogueController.LoadDialogue(m_dialoguePaths[m_activeDialogue]);
    }

    void OnAnimatorIK()
    {
        
        if (m_lookAtPlayer)
        {
            m_anim.SetBool("Talking", true);
            m_anim.SetLookAtWeight(m_lookAtWeight);
            m_anim.SetLookAtPosition(m_playerHead.transform.position);
            if((m_playerHead.transform.position - transform.position).magnitude > 3)
            {
                m_lookAtPlayer = false;
            }
        }
        else
        {
            m_anim.SetBool("Talking", false);
            m_lookAtWeight = 0;
            m_anim.SetLookAtWeight(0);
        }
    }

    IEnumerator LerpToOne()
    {
        while (m_lookAtWeight < 0.9999f)
        {
            m_lookAtWeight = Mathf.Lerp(m_lookAtWeight, 1, Time.deltaTime * m_headTurnSpeed);
            yield return null;
        }
    }

    public int GetNumberOfDialogues()
    {
        return m_dialoguePaths.Count;
    }

    public void SetObjectiveSystem(ObjectiveSystem _objectiveSystem)
    {
        m_objectiveSystem = _objectiveSystem;
    }
}
