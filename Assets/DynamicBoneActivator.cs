using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class DynamicBoneActivator : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        FindAndEnableDB(other, true);
    }

    private void OnTriggerExit(Collider other)
    {
        FindAndEnableDB(other, false);
        FindAndEnableRagDool(other, false);
    }

    private void FindAndEnableDB(Collider other, bool state)
    {
        var db = other.GetComponentInChildren<DynamicBone>();
        if (db != null)
            db.enabled = state;
    }

    private void FindAndEnableRagDool(Collider other, bool state)
    {
        var ragdollMarker = other.GetComponentInChildren<RagdollMarker>();
        if (ragdollMarker != null)
            ragdollMarker.gameObject.SetActive(state);
    }
}