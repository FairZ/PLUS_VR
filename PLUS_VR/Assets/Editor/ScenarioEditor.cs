using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityEditor.SceneManagement;

public class ScenarioEditor : EditorWindow {

    ObjectiveSystem m_objectiveSystem;
    List<Objective> m_objectives;
    List<NPC> m_scenarioNPCs;
    List<ObjectiveArea> m_areas;

    bool m_showObjectives = true;
    bool m_showScenarioNPCs = false;
    bool m_showAreas = false;

    Vector2 scrollPos;

    //place this menu in the PLUS folder at the top of the screen
    [MenuItem("PLUS/Scenario Editor")]
    public static void ShowWindow()
    {
        GetWindow<ScenarioEditor>("Scenario Editor");
    }

    private void OnInspectorUpdate()
    {
        m_objectiveSystem = null;
        m_objectiveSystem = GameObject.FindWithTag("ObjectiveSystem").GetComponent<ObjectiveSystem>();
    }

    void OnFocus()
    {
        
        if (m_scenarioNPCs != null)
            m_scenarioNPCs.Clear();
        else
            m_scenarioNPCs = new List<NPC>();

        foreach(GameObject go in GameObject.FindGameObjectsWithTag("NPC"))
        {
            NPC temp = go.GetComponent<NPC>();
            if(temp != null && temp.m_scenarioNPC)
            {
                m_scenarioNPCs.Add(temp);
            }
        }
        if (m_areas != null)
            m_areas.Clear();
        else
            m_areas = new List<ObjectiveArea>();
        
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("ObjectiveArea"))
        {
            ObjectiveArea temp = go.GetComponent<ObjectiveArea>();
            if (temp != null)
            {
                m_areas.Add(temp);
            }
        }
        if (m_objectiveSystem == null)
        {
            Debug.Log("No Objective System found, please add an object with an attached ObjectiveSystem script and set its tag to ObjectiveSystem");
        }
        
    }

    void OnGUI()
    {
        GUI.skin.textField.wordWrap = true;
        m_objectives = m_objectiveSystem.m_objectives;
        //Show number of scenario NPC
        //show all objectives
        //buttons for adding and removing both
        //buttons for moving objectives up and down

        EditorGUILayout.Space();
        m_showObjectives = EditorGUILayout.Toggle("Show Objectives: ", m_showObjectives);
        m_showScenarioNPCs = EditorGUILayout.Toggle("Show Scenario NPCs: ", m_showScenarioNPCs);
        m_showAreas = EditorGUILayout.Toggle("Show Objective Areas: ", m_showAreas);
        if(GUILayout.Button("Save"))
        {
            EditorSceneManager.MarkAllScenesDirty();
            EditorSceneManager.SaveOpenScenes();
        }
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
        if (m_showObjectives)
        {
            if(m_objectiveSystem.m_objectives.Count == 0)
            {
                EditorGUILayout.LabelField("No objectives added");
                EditorGUILayout.Space();
            }
            for(int i = 0; i < m_objectives.Count; i++)
            {

                m_objectives[i].m_type = (Objective.ObjectiveType)EditorGUILayout.Popup("Objective Type: ", (int)m_objectives[i].m_type, new string[] { "Talk To", "Go To Area" });

                m_objectives[i].m_quickTitle = EditorGUILayout.TextField("Short Title: ", m_objectives[i].m_quickTitle);
                m_objectives[i].m_description = EditorGUILayout.TextField("Description: ", m_objectives[i].m_description, GUILayout.MinHeight(30));

                EditorGUILayout.Space();
                switch(m_objectives[i].m_type)
                {
                    case Objective.ObjectiveType.TALK_TO:
                        m_objectives[i].m_NPC = (NPC)EditorGUILayout.ObjectField("NPC: ", m_objectives[i].m_NPC, typeof(NPC),true);
                        if (m_objectives[i].m_NPC != null)
                        {
                            if (m_objectives[i].m_NPC.GetNumberOfDialogues() != 0)
                            {
                                GUILayout.BeginHorizontal();
                                GUILayout.Label("Objective Dialogue Index: ");
                                m_objectives[i].m_objectiveDialogueIndex = EditorGUILayout.IntSlider(m_objectives[i].m_objectiveDialogueIndex, 0, m_objectives[i].m_NPC.GetNumberOfDialogues() - 1);
                                GUILayout.EndHorizontal();
                                GUILayout.BeginHorizontal();
                                GUILayout.Label("Post-Objective Dialogue Index: ");
                                m_objectives[i].m_postObjectiveDialogueIndex = EditorGUILayout.IntSlider( m_objectives[i].m_postObjectiveDialogueIndex, 0, m_objectives[i].m_NPC.GetNumberOfDialogues() - 1);
                                GUILayout.EndHorizontal();
                                
                            }
                            else
                            {
                                EditorGUILayout.Space();
                                EditorGUILayout.LabelField("Add dialogues to the npc to set indexes");
                            }
                                
                        }
                        break;
                    case Objective.ObjectiveType.GO_TO_AREA:
                        m_objectives[i].m_area = (ObjectiveArea)EditorGUILayout.ObjectField("Area: ", m_objectives[i].m_area, typeof(ObjectiveArea), true);
                        break;
                }
                EditorGUILayout.Space();
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Move Up"))
                {
                    MoveObjectiveUp(i);
                }

                if (GUILayout.Button("Move Down"))
                {
                    MoveObjectiveDown(i);
                }

                if (GUILayout.Button("Remove"))
                {
                    RemoveObjective(i);
                }
                GUILayout.EndHorizontal();

                if (i != m_objectives.Count - 1)
                {
                    EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
                }
            }
            if (m_objectives.Count != 0)
            {
                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            }
            if (GUILayout.Button("Add Objective"))
            {
                AddObjective();
            }
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        }
        if(m_showScenarioNPCs)
        {
            if(m_scenarioNPCs.Count == 0)
            {
                EditorGUILayout.LabelField("No Scenario NPCs added");
            }
            for(int i = 0; i < m_scenarioNPCs.Count; i++)
            {
                m_scenarioNPCs[i].gameObject.name = EditorGUILayout.TextField("Name: ", m_scenarioNPCs[i].gameObject.name);
                EditorGUILayout.LabelField("Dialogue File Names:");
                EditorGUI.indentLevel++;
                
                for (int j = 0; j < m_scenarioNPCs[i].m_dialoguePaths.Count; j++)
                {
                    m_scenarioNPCs[i].m_dialoguePaths[j] = EditorGUILayout.TextField("Element " + j + ":", m_scenarioNPCs[i].m_dialoguePaths[j]);
                }
                GUILayout.BeginHorizontal();
                GUILayout.Space(EditorGUI.indentLevel * 20);
                if (GUILayout.Button("Add Dialogue", GUILayout.MaxWidth(150)))
                {
                    AddDialoguePath(i);
                }
                if (m_scenarioNPCs[i].m_dialoguePaths.Count != 0)
                {
                    if (GUILayout.Button("Remove Dialogue", GUILayout.MaxWidth(150)))
                    {
                        RemoveDialoguePath(i);
                    }
                }
                GUILayout.EndHorizontal();
                EditorGUI.indentLevel--;
                EditorGUILayout.Space();
                if (GUILayout.Button("Remove NPC (will clear from level)", GUILayout.MaxWidth(250)))
                {
                    RemoveScenarioNPC(i);
                }
                if (i != m_scenarioNPCs.Count - 1)
                {
                    EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
                }
            }
            if (m_scenarioNPCs.Count != 0)
            {
                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            }
            if (GUILayout.Button("Add Scenario NPC"))
            {
                AddScenarioNPC();
            }
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        }
        if(m_showAreas)
        {
            if (m_areas.Count == 0)
            {
                EditorGUILayout.LabelField("No Objective Areas added");
            }
            for (int i = 0; i < m_areas.Count; i++)
            {
                m_areas[i].gameObject.name = EditorGUILayout.TextField("Name: ", m_areas[i].gameObject.name);
                
                if (GUILayout.Button("Remove Area (will clear from level)", GUILayout.MaxWidth(250)))
                {
                    RemoveArea(i);
                }
                if (i != m_areas.Count - 1)
                {
                    EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
                }
            }
            if (m_areas.Count != 0)
            {
                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            }
            if (GUILayout.Button("Add Objective Area"))
            {
                AddArea();
            }
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        }
        EditorGUILayout.EndScrollView();

    }

    private void RemoveArea(int _index)
    {
        GameObject.DestroyImmediate(m_areas[_index].gameObject);
        m_areas.RemoveAt(_index);
    }

    private void AddArea()
    {
        if (m_objectiveSystem.m_objectiveAreaPrefab != null)
        {
            GameObject.Instantiate(m_objectiveSystem.m_objectiveAreaPrefab);
            EditorWindow.FocusWindowIfItsOpen<SceneView>();
            EditorWindow.FocusWindowIfItsOpen<ScenarioEditor>();
        }
        else
        {
            Debug.LogError("No Objective Area prefab set, please add the prefab from the Editor folder to the objective system");
        }
    }

    private void AddScenarioNPC()
    {
        if(m_objectiveSystem.m_scenarioNPCPrefab != null)
        {
            GameObject.Instantiate(m_objectiveSystem.m_scenarioNPCPrefab);
            EditorWindow.FocusWindowIfItsOpen<SceneView>();
            EditorWindow.FocusWindowIfItsOpen<ScenarioEditor>();
        }
        else
        {
            Debug.LogError("No ScenarioNPC prefab set, please add the prefab from the Editor folder to the objective system");
        }
    }

    private void RemoveScenarioNPC(int _index)
    {
        GameObject.DestroyImmediate(m_scenarioNPCs[_index].gameObject);
        m_scenarioNPCs.RemoveAt(_index);
    }

    private void AddDialoguePath(int _NPCindex)
    {
        m_scenarioNPCs[_NPCindex].m_dialoguePaths.Add("");
    }

    private void RemoveDialoguePath(int _NPCindex)
    {
        m_scenarioNPCs[_NPCindex].m_dialoguePaths.RemoveAt(m_scenarioNPCs[_NPCindex].m_dialoguePaths.Count -1);
    }

    private void AddObjective()
    {
        m_objectives.Add(new Objective());
    }

    private void RemoveObjective(int _index)
    {
        m_objectives.RemoveAt(_index);
    }

    private void MoveObjectiveUp(int _index)
    {
        if (_index > 0)
        {
            Objective temp = m_objectives[_index];
            m_objectives.RemoveAt(_index);
            m_objectives.Insert(--_index, temp);
        }
    }

    private void MoveObjectiveDown(int _index)
    {
        if (_index < m_objectives.Count - 1)
        {
            Objective temp = m_objectives[_index];
            m_objectives.RemoveAt(_index);
            m_objectives.Insert(++_index, temp);
        }
    }
}
