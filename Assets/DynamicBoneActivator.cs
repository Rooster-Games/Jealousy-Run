using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using GameCores;
using GameCores.CoreEvents;
using UnityEngine;

public class DynamicBoneActivator : MonoBehaviour
{
    BoxCollider _boxCol;

    public void Init(InitParameters initParameters)
    {
        //_boxCol = GetComponent<BoxCollider>();
        //_boxCol.enabled = false;
        _boxCol = initParameters.BoxCollider;
        _boxCol.enabled = false;

        initParameters.EventBus.Register<OnGameStarted>(EventBus_OnGameStarted);
    }

    private void EventBus_OnGameStarted(OnGameStarted eventData)
    {
        _boxCol.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        FindAndEnableDB(other, true);
    }

    private void OnTriggerExit(Collider other)
    {
        FindAndEnableDB(other, false);
        FindAndEnableRagDool(other, false);
        other.gameObject.SetActive(false);
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

    public class InitParameters
    {
        public IEventBus EventBus { get; set; }
        public BoxCollider BoxCollider { get; set; }
    }
}