using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class ComboMakerEditor : EditorWindow
{
    private bool initialized;
    private List<string> characters;
    private List<string> damagetypes;
    private float[] damagevalues;
    private List<string> abilitylist;
    private List<List<ComboClass>> combolist;

    private int selectedPopupIndex_character = 0;
    private int selectedPopupIndex_damage = 0;
    private int selectedPopupIndex_damagevalues = 0;
    private int selectedPopupIndex_ability = 0;
    private int selectedPopupIndex_combo = 0;
    private bool IsComboPlaying;

    [MenuItem("Personal Tools/Combo Maker")]
    public static void f()
    {
        // Get existing open window or if none, make a new one:
        ComboMakerEditor window = (ComboMakerEditor)EditorWindow.GetWindow(typeof(ComboMakerEditor));
        window.Show();
    }

    public void OnEnable()
    {
        #region Folder Check
        string path = Application.dataPath + "/StreamingAssets/data/cb";

        if (!AssetDatabase.IsValidFolder(path))
        {
            path = Application.dataPath + "/StreamingAssets/data";

            if (!AssetDatabase.IsValidFolder(path))
            {
                path = Application.dataPath + "/StreamingAssets";

                AssetDatabase.CreateFolder(path, "data");

            }
            else
            {
                AssetDatabase.CreateFolder(path, "cb");
            }
        }

        path = Application.dataPath + "/StreamingAssets/data/cb";
        #endregion

        #region Data Initialization
        characters = new List<string>();
        Array characterArray = Enum.GetValues(typeof(Common.Character));

        foreach (Common.Character cc in characterArray)
        {
            characters.Add(cc.ToString());
        }
        characterArray = null;

        abilitylist = new List<string>();
        abilitylist.Add("Beacon of Light");
        abilitylist.Add("Beacon of Dark");
        abilitylist.Add("Beacon of Fire");
        abilitylist.Add("Beacon of Rock");


        damagetypes = new List<string>();
        Array damageTypesArray = Enum.GetValues(typeof(Common.DamageType));

        foreach (Common.DamageType dt in damageTypesArray)
        {
            damagetypes.Add(dt.ToString());
        }
        damageTypesArray = null;


        //Retrieve curves from txt file
        damagevalues = new float[damagetypes.Count];

        combolist = new List<List<ComboClass>>();
        combolist.Add(new List<ComboClass>());



        #endregion

        initialized = true;
    }

    public void OnGUI()
    {

        if (!initialized)
        {
            OnEnable();
        }


        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Combo Maker");
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Separator();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Character");
        selectedPopupIndex_character = EditorGUILayout.Popup(selectedPopupIndex_character, characters.ToArray());
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Ability");
        selectedPopupIndex_ability = EditorGUILayout.Popup(selectedPopupIndex_ability, abilitylist.ToArray());
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Separator();

        #region DMG Control Panel
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("DMG Control Panel");
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.BeginVertical();
        selectedPopupIndex_damage = EditorGUILayout.Popup(selectedPopupIndex_damage, damagetypes.ToArray());

        EditorGUILayout.EndVertical();
        EditorGUILayout.BeginVertical();
        damagevalues[selectedPopupIndex_damagevalues] = EditorGUILayout.Slider(damagevalues[selectedPopupIndex_damagevalues], 0, 1);
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();
        #endregion

        EditorGUILayout.Separator();

        #region recorder
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.BeginVertical();
        if (!IsComboPlaying)
        {
            if (GUILayout.Button(((Texture)(Texture)Resources.Load("Textures/stop.png"))))
            {
                Debug.Log("IsComboPlaying: " + IsComboPlaying);
                IsComboPlaying = true;
            }
        }
        else
        {
            if (GUILayout.Button((Texture)Resources.Load("Textures/play.png")))
            {
                Debug.Log("IsComboPlaying: " + IsComboPlaying);
                IsComboPlaying = false;
            }
            //if (GUILayout.Button((Texture)Resources.Load("Textures/record.png"))) {
            //    IsComboRecording ;
            //}
        }
        //GUILayout.Button("");
        EditorGUILayout.EndVertical();
        EditorGUILayout.BeginHorizontal();
        for (int i = 0; i < combolist[selectedPopupIndex_ability].Count; i++)
        {
            //if (combolist[selectedPopupIndex_ability][selectedPopupIndex_combo] == null)
            combolist[selectedPopupIndex_ability][selectedPopupIndex_combo] = new ComboClass(accuracy: Common.Accuracy.GOOD, timewindow: 500, keycode: KeyCode.A, damage: 2, ability: new Ability());

            Color c = new Color();
            switch (combolist[selectedPopupIndex_ability][selectedPopupIndex_combo].accuracy)
            {
                case Common.Accuracy.WEAK:
                    c = new Color(255, 240, 240);
                    break;
                case Common.Accuracy.GOOD:
                    c = new Color(240, 240, 255);
                    break;
                case Common.Accuracy.PERFECT:
                    c = new Color(240, 240, 255);
                    break;
                default: break;
            }

            //use GUI.Box because editorgui unusable in editorwindow
            EditorGUI.DrawRect(new Rect(100, 100, combolist[selectedPopupIndex_ability][selectedPopupIndex_combo].timewindow, 100), c);
        }

        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndHorizontal();

        #endregion

        EditorGUILayout.BeginHorizontal();



        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.EndHorizontal();


        EditorGUILayout.BeginHorizontal();
        GUILayout.Button("SAVE");
        EditorGUILayout.EndHorizontal();


        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.EndHorizontal();


    }

}
