using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeFlowCharacter : Character
{

    FreeFlowMovement freeFlowMovement;

    protected override void OnEnable()
    {
        base.OnEnable();

        freeFlowMovement = GetComponentInParent<FreeFlowMovement>();

        if (freeFlowMovement)
        {
            freeFlowMovement.OnRun += IsRunning;
        }

    }

    protected override void OnDisable()
    {
        base.OnDisable();

        if (freeFlowMovement)
        {
            freeFlowMovement.OnRun -= IsRunning;
        }
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void IsRunning(bool isRun)
    {
        base.IsRunning(isRun);
    }
}
