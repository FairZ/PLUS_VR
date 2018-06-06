using UnityEngine;
using UnityEditor;
using System.IO;

public class DialogueEditor : EditorWindow {

    string m_dialogueName = null;

    Vector2 scrollPos;

    string[] actionOptions = new string[] { "Close Dialogue", "Go to Node", "Stop and Search" };

    Dialogue m_dialogue;

    [MenuItem("PLUS/Dialogue Editor")]
	public static void ShowWindow()
    {
        GetWindow<DialogueEditor>("Dialogue Editor");
    }

    void Awake()
    {
        m_dialogue = new Dialogue();
    }

    void OnGUI()
    {
        //code for window display
        EditorGUILayout.Space();
        m_dialogueName = EditorGUILayout.TextField("Name of Dialogue", m_dialogueName);
        EditorGUILayout.Space();
        Rect r = EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Load"))
        {
            LoadDialogue();
        }
        if (GUILayout.Button("Clear"))
        {
            ClearDialogue();
        }
        if(GUILayout.Button("Save"))
        {
            SaveDialogue();
        }
        EditorGUILayout.EndHorizontal();
        if (GUILayout.Button("Add new Node"))
        {
            m_dialogue.AddNode();
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
        

        if (m_dialogue.m_nodes != null)
        {
            for(int i = 0; i < m_dialogue.m_nodes.Count; i++)
            {
                EditorGUI.indentLevel++;
                Node n = m_dialogue.m_nodes[i];

                EditorGUILayout.LabelField("ID: ", n.m_id.ToString());

                n.m_name = EditorGUILayout.TextField("Character Name", n.m_name);

                EditorGUILayout.LabelField("Text:");
                n.m_text = EditorGUILayout.TextArea(n.m_text,GUILayout.Height(50));
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider,GUILayout.MaxWidth(50));
                for (int j = 0; j < n.m_options.Count; j++)
                {
                    Option o = n.m_options[j];
                    EditorGUILayout.LabelField("Option:", j.ToString());
                    o.m_text = EditorGUILayout.TextField("Text:", o.m_text);
                    o.m_action = EditorGUILayout.Popup("Action:",o.m_action, actionOptions);
                    if (o.m_action == 1)
                    {
                        o.m_destination = EditorGUILayout.IntField("Destination ID:", o.m_destination);
                    }

                    if (GUILayout.Button("Delete", GUILayout.MaxWidth(75)))
                    {
                        n.m_options.RemoveAt(j);
                    }

                    EditorGUILayout.Space();
                }
                if(n.m_options.Count < 4)
                {
                    if (GUILayout.Button("Add Option", GUILayout.MaxWidth(75)))
                    {
                        n.AddOption();
                    }
                }
                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider, GUILayout.MaxWidth(50));

                EditorGUILayout.Space();
                if (GUILayout.Button("Delete"))
                {
                    m_dialogue.DeleteNode(n.m_id);
                    for(int k = 0; k < m_dialogue.m_nodes.Count; k++)
                    {
                        m_dialogue.m_nodes[k].m_id = k;
                        for(int o = 0; o < m_dialogue.m_nodes[k].m_options.Count; o++)
                        {
                            if (m_dialogue.m_nodes[k].m_options[o].m_destination > i)
                            {
                                m_dialogue.m_nodes[k].m_options[o].m_destination--;
                            }
                            if(m_dialogue.m_nodes[k].m_options[o].m_destination == i)
                            {
                                m_dialogue.m_nodes[k].m_options.RemoveAt(o);
                            }
                        }
                    }
                }
                EditorGUI.indentLevel--;
                EditorGUILayout.Space();
                if (i < (m_dialogue.m_nodes.Count-1))
                {
                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
                    EditorGUILayout.Space();
                }
            }
        }

        EditorGUILayout.EndScrollView();        
    }

    void SaveDialogue()
    {
        //convert the data to Json format
        string jsonData = JsonUtility.ToJson(m_dialogue,true);
        //set the path to the file
        string path = Application.dataPath+"/Resources/DialogueJson/" + m_dialogueName + ".json";

        //create a new file or clear the existing file
        FileStream fs = new FileStream(path, FileMode.Create);
        //close stream to avoid access conflicts
        fs.Close();
        //send Json data to file
        File.WriteAllText(path, jsonData);
        //refresh the asset database to show the new version
        AssetDatabase.Refresh();
    }

    void LoadDialogue()
    {
        string path = Application.dataPath + "/Resources/DialogueJson/" + m_dialogueName + ".json";
        if (File.Exists(path))
        {
            string jsonData = File.ReadAllText(path);
            m_dialogue = JsonUtility.FromJson<Dialogue>(jsonData);
        }
        else
        {
            Debug.Log("invalid file name");
        }
    }

    void ClearDialogue()
    {
        m_dialogue = new Dialogue();
    }
}
