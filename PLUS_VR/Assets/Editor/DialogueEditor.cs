using UnityEngine;
using UnityEditor;
using System.IO;

public class DialogueEditor : EditorWindow {

    //name of dialogue to be opened/saved
    string m_dialogueName = null;

    //position of Node view scroll
    Vector2 scrollPos;

    //possible actions for Options
    string[] actionOptions = new string[] { "Close Dialogue", "Go to Node", "Stop and Search", "Correct Answer","Incorrect Answer" };

    //Dialogue holding the currently worked on piece of dialogue
    Dialogue m_dialogue;

    //place this menu in the PLUS folder at the top of the screen
    [MenuItem("PLUS/Dialogue Editor")]
	public static void ShowWindow()
    {
        GetWindow<DialogueEditor>("Dialogue Editor");
    }

    //initialise a new dialogue on awake
    void Awake()
    {
        m_dialogue = new Dialogue();
    }

    //code for layout of editor window
    void OnGUI()
    {

        GUI.skin.textField.wordWrap = true;
        
        //begin by showing the name of the dialogue currently being edited or to be loaded
        EditorGUILayout.Space();
        m_dialogueName = EditorGUILayout.TextField("Name of Dialogue", m_dialogueName);
        EditorGUILayout.Space();

        //show load clear and save buttons in a horizontal line and give them correct functions
        EditorGUILayout.BeginHorizontal();
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

        //add new node function underneath
        if (GUILayout.Button("Add new Node"))
        {
            m_dialogue.AddNode();
        }
        //horizontal dividing bar
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        //begin Node view
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
        if (m_dialogue.m_nodes != null)
        {
            //create one set of UI elements for each node
            for(int i = 0; i < m_dialogue.m_nodes.Count; i++)
            {
                //add an indent to move the following UI elements in slightly
                EditorGUI.indentLevel++;
                //initialise a node variable for easier access
                Node n = m_dialogue.m_nodes[i];
                //display id number of node
                EditorGUILayout.LabelField("ID: ", n.m_id.ToString());
                //display and set character name in a text field
                n.m_name = EditorGUILayout.TextField("Character Name:", n.m_name);
                //display and set dialogue text in a text field
                n.m_text = EditorGUILayout.TextField("Text:",n.m_text,GUILayout.Height(50));
                //add a space and small horizontal line to seperate options section of node
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider,GUILayout.MaxWidth(75));

                //create one set of UI elements for each option
                for (int j = 0; j < n.m_options.Count; j++)
                {
                    //initialise option variable for easier access
                    Option o = n.m_options[j];
                    //display option number
                    EditorGUILayout.LabelField("Option:", j.ToString());
                    //display and set text for this option
                    o.m_text = EditorGUILayout.TextField("Text:", o.m_text);
                    //display and set the action of this option 
                    o.m_action = EditorGUILayout.Popup("Action:",o.m_action, actionOptions);
                    //if the action is to go to another Node, display and set the destination with an integer field
                    if (o.m_action == 1 || o.m_action == 3 || o.m_action == 4)
                    {
                        o.m_destination = EditorGUILayout.IntField("Destination ID:", o.m_destination);
                    }
                    //show a small button to remove the option
                    GUILayout.BeginHorizontal();
                    GUILayout.Space(EditorGUI.indentLevel * 20);
                    if (GUILayout.Button("Delete Option", GUILayout.MaxWidth(100)))
                    {
                        n.m_options.RemoveAt(j);
                    }
                    GUILayout.EndHorizontal();

                    EditorGUILayout.Space();
                }
                //if there can be more options, display an add option button
                GUILayout.BeginHorizontal();
                GUILayout.Space(EditorGUI.indentLevel * 20);
                if (n.m_options.Count < 4)
                {
                    if (GUILayout.Button("Add Option", GUILayout.MaxWidth(100)))
                    {
                        n.AddOption();
                    }
                }
                GUILayout.EndHorizontal();
                //horizontal seperating bar to signal end of option region
                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider, GUILayout.MaxWidth(75));

                EditorGUILayout.Space();
                //button to remove a node
                if (GUILayout.Button("Delete Node"))
                {
                    //delete the correct node
                    m_dialogue.DeleteNode(n.m_id);
                    //loop throuhg all nodes to reset ID and alter destination options to match new IDs
                    for(int k = 0; k < m_dialogue.m_nodes.Count; k++)
                    {
                        m_dialogue.m_nodes[k].m_id = k;
                        for(int o = 0; o < m_dialogue.m_nodes[k].m_options.Count; o++)
                        {
                            if (m_dialogue.m_nodes[k].m_options[o].m_destination > i)
                            {
                                m_dialogue.m_nodes[k].m_options[o].m_destination--;
                            }
                            //if the target Node has been removed the option will also be removed
                            if(m_dialogue.m_nodes[k].m_options[o].m_destination == i)
                            {
                                m_dialogue.m_nodes[k].m_options.RemoveAt(o);
                            }
                        }
                    }
                }
                EditorGUI.indentLevel--;
                EditorGUILayout.Space();
                //add horizontal dividers between nodes
                if (i < (m_dialogue.m_nodes.Count-1))
                {
                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
                    EditorGUILayout.Space();
                }
            }
        }
        //end of Node view
        EditorGUILayout.EndScrollView();

        //horizontal divider andsecondary add node button
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        if (GUILayout.Button("Add new Node"))
        {
            m_dialogue.AddNode();
        }
        EditorGUILayout.Space();
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
        //set the path to the file
        string path = Application.dataPath + "/Resources/DialogueJson/" + m_dialogueName + ".json";
        //if the file exists read from it and load in the dialogue otherwise show error
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
        //reset the current dialogue to new
        m_dialogue = new Dialogue();
    }
}
