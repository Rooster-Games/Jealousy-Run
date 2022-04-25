using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using NaughtyAttributes;
using UnityEngine;

public class DynamicBonesetter : MonoBehaviour
{
    [SerializeField] string dbRootName = "spine";
    [Button("Set")]
    public void SetDynamicBone()
    {
        var dbs = FindObjectsOfType<DynamicBone>();

        Debug.Log(dbs.Length);
        foreach (var db in dbs)
        {
            var parent = db.transform.GetChild(0);
            var foundTransform = TraverseTransfomAndFindChildByName(parent, dbRootName);

            db.m_Root = foundTransform;
        }
    }

    private Transform TraverseTransfomAndFindChildByName(Transform parent, string name)
    {
        Transform foundTransform = null;
        var childCount = parent.childCount;
        for(int i = 0; i < childCount; i++)
        {

            var child = parent.GetChild(i);

            if (child.name == name)
                foundTransform = child;

            if (foundTransform != null)
                return foundTransform;
            foundTransform = TraverseTransfomAndFindChildByName(child, name);
        }

        return foundTransform;
    }
}

