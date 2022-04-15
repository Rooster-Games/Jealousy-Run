using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class StandartRunnerCharacter : Character
{
    InputManager inputManager;

    StandartRunnerMovement standartRunnerMovement;

    [SerializeField] private float _rotateAmount = 30;

    protected override void OnEnable()
    {
        base.OnEnable();

        standartRunnerMovement = GetComponentInParent<StandartRunnerMovement>();

        if (standartRunnerMovement)
        {
            standartRunnerMovement.OnRun += IsRunning;
            inputManager = standartRunnerMovement.GetComponent<InputManager>();
        }
        else
        {
            Debug.LogError("please add Standart Runner Movement on parent");
        }

    }

    protected override void OnDisable()
    {
        base.OnDisable();

        if (standartRunnerMovement)
        {
            standartRunnerMovement.OnRun -= IsRunning;
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

        transform.DOLocalRotate(new Vector3(0,sign * _rotateAmount,0),0.4f);
    }

    private void Update()
    {
        Rotate();
    }

}
