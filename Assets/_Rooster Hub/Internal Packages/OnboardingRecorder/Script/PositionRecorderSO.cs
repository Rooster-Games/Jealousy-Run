using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu]
public class PositionRecorderSO : ScriptableObject
{
    public List<Vector3> points = new List<Vector3>();
#if UNITY_EDITOR
    public void SavePositions()
    {
        EditorUtility.SetDirty(this);
    }
#endif
    public void AddNodeInPositions(List<Vector3> pos)
    {
        points.AddRange(pos);
    }

    public void ClearNodes()
    {
        points.Clear();
    }
}