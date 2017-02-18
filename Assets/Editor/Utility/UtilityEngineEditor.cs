using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(UtilityEngine))]
public class UtilityEngineEditor : Editor
{
    //ReorderableList ActionList;
    //List<ReorderableList> ConsiderationLists = new List<ReorderableList>();

    void OnEnable()
    {
       // ActionList = new ReorderableList(serializedObject,
        //    serializedObject.FindProperty("Actions"),
         //   true, true, true, true);
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        serializedObject.Update();

        //DisplayScript();

        DisplayActions();

        serializedObject.ApplyModifiedProperties();
    }

    void DisplayActions()
    {
        List<UtilityAction> actions = ((UtilityEngine)target).Actions;
        foreach(UtilityAction action in actions)
        {
            foreach(UtilityConsideration cons in action.Considerations)
            {
                DisplayConsideration(cons);
            }
        }
    }

    void DisplayConsideration(UtilityConsideration cons)
    {
        EditorGUI.CurveField(new Rect(0, 0, EditorGUIUtility.currentViewWidth, 100), 
            new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1)));
    }

    void DisplayScript()
    {
        SerializedProperty iterator = serializedObject.GetIterator();
        iterator.NextVisible(true);
        GUI.enabled = false;
        EditorGUILayout.PropertyField(iterator, true);
        GUI.enabled = true;
    }

}
