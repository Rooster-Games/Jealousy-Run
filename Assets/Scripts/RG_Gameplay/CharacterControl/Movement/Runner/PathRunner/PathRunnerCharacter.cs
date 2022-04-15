using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PathRunnerCharacter : Character
{
    InputManager inputManager;

    PathRunnerMovement pathRunnerMovement;

    [SerializeField] private float _rotateAmount = 30;

    protected override void OnEnable()
    {
        base.OnEnable();

        pathRunnerMovement = GetComponentInParent<PathRunnerMovement>();

        if (pathRunnerMovement)
        {
            pathRunnerMovement.OnRun += IsRunning;
            inputManager = pathRunnerMovement.GetComponent<InputManager>();
        }
        else
        {
            Debug.LogError("please add Standart Runner Movement on parent");
        }

    }

    protected override void OnDisable()
    {
        base.OnDisable();

        if (pathRunnerMovement)
        {
            pathRunnerMovement.OnRun -= IsRunning;
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

    void Rotate()
    {
        float sign = Mathf.Approximately(inputManager.DeltaX, 0) ? 0 : Mathf.Sign(inputManager.DeltaX);

        transform.DOLocalRotate(new Vector3(0, sign * _rotateAmount, 0), 0.4f);
    }

    private void Update()
    {
        Rotate();
    }
}
