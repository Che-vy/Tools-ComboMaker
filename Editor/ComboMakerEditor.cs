using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class ComboMakerEditor : EditorWindow
{
    private Rect windowPos;
    private bool initialized;
    private List<string> characters;
    private List<string> damagetypes;
    private float[] damagevalues;
    private List<string> abilitylist;
    private List<List<ComboClass>> combolist;
    private Dictionary<string, Animation> animationlist;

    private int selectedPopupIndex_character = 0;
    private int selectedPopupIndex_damage = 0;
    private int selectedPopupIndex_damagevalues = 0;
    private int selectedPopupIndex_ability = 0;
    private int selectedPopupIndex_combo = 0;
    private bool IsFoldoutUnfolded = true;
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
        windowPos = GetWindow<ComboMakerEditor>("ComboMakerEditor").position;

        #region Folder Check
        string path = Application.dataPath + "/StreamingAssets/data/cb";

        if (!AssetDatabase.IsValidFolder(path))
        {
            path = Application.dataPath + "/StreamingAssets/data";

            if (!AssetDatabase.IsValidFolder(path))
            {
                path = Application.dataPath + "/StreamingAssets";

                AssetDatabase.CreateFolder("ComboMaker/Assets/StreamingAssets", "data");

            }
            else
            {
                AssetDatabase.CreateFolder("ComboMaker/Assets/StreamingAssets", "cb");
            }
        }

        // path = Application.dataPath + "/StreamingAssets/data/cb";
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
        abilitylist.Add("Beacon of Stone");


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
        combolist[0].Add(new ComboClass(KeyCode.B, new Ability(), new Animation(), 0.4f, Common.Accuracy.GOOD, 2));
        combolist[0].Add(new ComboClass(KeyCode.W, new Ability(), new Animation(), 0.2f, Common.Accuracy.PERFECT, 2));
        combolist[0].Add(new ComboClass(KeyCode.A, new Ability(), new Animation(), 1f, Common.Accuracy.WEAK, 2));


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
        IsFoldoutUnfolded = EditorGUILayout.Foldout(IsFoldoutUnfolded, "ok", true);
        if (IsFoldoutUnfolded)
        {
            EditorGUI.indentLevel++;
            foreach (ComboClass cc in combolist[0]) {
#pragma warning disable CS0618 // Type or member is obsolete
                EditorGUILayout.ObjectField(cc.animation, typeof(Animation));
#pragma warning restore CS0618 // Type or member is obsolete

            }

            EditorGUI.indentLevel--;
        }
        EditorGUI.indentLevel = 0;
        EditorGUILayout.Separator();

        #region record/play btn
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
        #endregion

        #region Timeline
        EditorGUILayout.BeginHorizontal();
        float xOffset = 4, yOffset = 140;

        for (int i = 0; i < combolist[selectedPopupIndex_ability].Count; i++)
        {
            //if (combolist[selectedPopupIndex_ability][selectedPopupIndex_combo] == null)
            ComboClass cc = combolist[selectedPopupIndex_ability][i];//new ComboClass(accuracy: Common.Accuracy.GOOD, timewindow: 30f, keycode: KeyCode.A, damage: 2, ability: new Ability());

            //use GUI.Box because editorgui unusable in editorwindow
            //EditorGUI.DrawRect(new Rect(100, 100, combolist[selectedPopupIndex_ability][selectedPopupIndex_combo].timewindow, 100), c);
            var centeredStyle = GUI.skin.GetStyle("Button");
            centeredStyle.alignment = TextAnchor.MiddleCenter;

            //GUI.Box(new Rect(windowPos.x + xOffset,
            //    windowPos.y + yOffset,
            //    cc.timewindow * 100,
            //    100),
            //    cc.keycode.ToString() + "\n " + (float)cc.timewindow + "s",
            //    centeredStyle);

            GUILayout.Box(new GUIContent(cc.keycode.ToString() + "\n " + (float)cc.timewindow + "s"), GUILayout.Width(cc.timewindow * 100), GUILayout.MaxHeight(50));
            

            xOffset = cc.timewindow;

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

    public void OnInspectorUpdate()
    {
        this.Repaint();
    }


    public Color SwitchColor(Common.Accuracy accuracy)
    {
        switch (accuracy)
        {
            case Common.Accuracy.WEAK:
                return new Color(255, 240, 240);
            case Common.Accuracy.GOOD:
                return new Color(240, 240, 255);
            case Common.Accuracy.PERFECT:
                return new Color(240, 240, 255);
            default: return Common.PINK;
        }


    }



}
