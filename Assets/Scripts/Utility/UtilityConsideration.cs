using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UtilityConsideration
{
    public enum CurveTypes { Linear, Polynomial, Log, Trigonometric };

    /// <summary>
    /// The type of curve that is used to get the score.
    /// </summary>
    public CurveTypes CurveType;

    /// <summary>
    /// Curve Parameter:
    /// y = m ((x/d) + k)^p + c | 
    /// y = m Sin((x/d) + k)^p + c | 
    /// y = m log p ((x/d) + k) + c
    /// </summary>
    public float m, d, k, p, c;

    /// <summary>
    /// The bookends of the input data. 
    /// (min, max) values.
    /// </summary>
    public Vector2 Bookends;

    public delegate float InputFunction();
    public InputFunction GetInput;

    public float GetScore()
    {
        float x = GetNormalisedInput() / ((d != 0) ? d : 1);
        switch (CurveType)
        {
            case CurveTypes.Linear:
            case CurveTypes.Polynomial:
                return m * Mathf.Pow(x + k, p) + c;

            case CurveTypes.Log:
                return m * Mathf.Log(x + k, p) + c;

            case CurveTypes.Trigonometric:
                return m * Mathf.Pow(Mathf.Sin(x + k), p) + c;

            default:
                return GetNormalisedInput();

        }
    }

    float GetNormalisedInput()
    {
        return Mathf.Clamp((GetInput() - Bookends.x) / (Bookends.y - Bookends.x), 0f, 1f);
    }
    
}
