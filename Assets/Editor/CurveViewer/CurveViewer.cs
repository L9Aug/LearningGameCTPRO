using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CurveViewer : EditorWindow
{
    AnimationCurve Curve;
    AnimationCurve yAxis = new AnimationCurve(new Keyframe(0, 0), new Keyframe(0, 1));
    float m, k, p, c, d, Resolution;
    float oldM, oldK, oldP, oldC, oldD, oldRes;

    string EquationText = "";

    enum CurveTypes { Linear, Polynomial, Log, Trigonometric }
    CurveTypes CurveType;
    CurveTypes OldCurveType;


    [MenuItem ("View/Curve Editor")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(CurveViewer));        
    }

    void SetDefaultValues()
    {
        switch (CurveType)
        {
            case CurveTypes.Linear:
            default:
                m = d = p = 1;
                k = c = 0;
                EquationText = "f(x) = m ((x/d) + k) + c";
                break;
            case CurveTypes.Trigonometric:
                m = d = p = 1;
                k = c = 0;
                EquationText = "f(x) = m Sin((x/d) + k)^p + c";
                break;
            case CurveTypes.Log:
                m = d = k = 1;
                p = 10;
                c = 0;
                EquationText = "f(x) = m log p ((x/d) + k) + c";
                break;
            case CurveTypes.Polynomial:
                m = d = 1;
                k = c = 0;
                p = 2;
                EquationText = "f(x) = m ((x/d) + k)^p + c";
                break;
        }
    }

    void OnGUI()
    {
        if (Curve == null)
        {
            Resolution = 100;
            SetDefaultValues();
            UpdateCurve();
        }

        float yPos = 3;

        Curve = EditorGUI.CurveField(new Rect(3, yPos, EditorGUIUtility.currentViewWidth - 6, EditorGUIUtility.currentViewWidth - 6), Curve, Color.green, new Rect(0, 0, 1, 1));
        yPos += EditorGUIUtility.currentViewWidth - 6 + EditorGUIUtility.standardVerticalSpacing;

        m = EditorGUI.FloatField(new Rect(3, yPos, EditorGUIUtility.currentViewWidth - 6, EditorGUIUtility.singleLineHeight), "m", m);

        yPos += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

        d = EditorGUI.FloatField(new Rect(3, yPos, EditorGUIUtility.currentViewWidth - 6, EditorGUIUtility.singleLineHeight), "d", d);

        yPos += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

        k = EditorGUI.FloatField(new Rect(3, yPos, EditorGUIUtility.currentViewWidth - 6, EditorGUIUtility.singleLineHeight), "k", k);

        yPos += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

        GUI.enabled = CurveType != CurveTypes.Linear;

        p = EditorGUI.FloatField(new Rect(3, yPos, EditorGUIUtility.currentViewWidth - 6, EditorGUIUtility.singleLineHeight), "p", p);

        GUI.enabled = true;

        yPos += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

        c = EditorGUI.FloatField(new Rect(3, yPos, EditorGUIUtility.currentViewWidth - 6, EditorGUIUtility.singleLineHeight), "c", c);

        yPos += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

        Resolution = EditorGUI.FloatField(new Rect(3, yPos, EditorGUIUtility.currentViewWidth - 6, EditorGUIUtility.singleLineHeight), "Resolution", Resolution);

        yPos += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

        CurveType = (CurveTypes)EditorGUI.EnumPopup(new Rect(3, yPos, EditorGUIUtility.currentViewWidth - 6, EditorGUIUtility.singleLineHeight), "Curve Type", CurveType);

        yPos += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

        EditorGUI.LabelField(new Rect(3, yPos, EditorGUIUtility.currentViewWidth - 6, EditorGUIUtility.singleLineHeight), "Equation Layout", EquationText);

        if (CurveNeedsUpdateing()) UpdateCurve();

        SetOldValues();
    }

    void SetOldValues()
    {
        oldM = m;
        oldK = k;
        oldP = p;
        oldC = c;
        oldD = d;
        oldRes = Resolution;
        OldCurveType = CurveType;
    }

    void ClearKeyframes()
    {
        for(int i = 0; i < Curve.length; ++i)
        {
            Curve.RemoveKey(0);
            --i;
        }
    }

    void UpdateCurve()
    {
        Curve = new AnimationCurve();

        ClearKeyframes();

        switch (CurveType)
        {
            case CurveTypes.Linear:
            case CurveTypes.Polynomial:
            default:
                SetupPolyCurve();
                break;
            case CurveTypes.Log:
                SetupLogCurve();
                break;
            case CurveTypes.Trigonometric:
                SetupSinCurve();
                break;
        }
        
    }

    void SetupPolyCurve()
    {
        for(int i = 0; i < Resolution + 1; ++i)
        {
            float time = (i / Resolution);
            float x = time / ((d != 0) ? d : 1);
            Curve.AddKey(time, m * Mathf.Pow((x + k), p) + c);
        }
    }

    void SetupLogCurve()
    {
        for (int i = 0; i < Resolution + 1; ++i)
        {
            float time = i / Resolution;
            float x = time / ((d != 0) ? d : 1);
            Curve.AddKey(time, m * Mathf.Log((x + k), p) + c);
        }
    }

    void SetupSinCurve()
    {
        for (int i = 0; i < Resolution + 1; ++i)
        {
            float time = i / Resolution;
            float x = time / ((d != 0) ? d : 1);
            Curve.AddKey(time, m * Mathf.Pow(Mathf.Sin(x + k), p) + c);
        }
    }

    bool CurveNeedsUpdateing()
    {
        if(m != oldM || k != oldK || p != oldP || c != oldC || Resolution != oldRes || CurveType != OldCurveType || d != oldD)
        {
            if (CurveType != OldCurveType) SetDefaultValues();
            return true;
        }
        return false;
    }

}
