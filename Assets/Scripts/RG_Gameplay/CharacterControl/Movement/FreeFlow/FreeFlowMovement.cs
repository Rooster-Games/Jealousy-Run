using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(InputManager))]
public class FreeFlowMovement : CharacterMovement
{
    private bool _clicked = false;
    [SerializeField] private Vector2 _direction;

    public bool Clicked { get => _clicked; set => _clicked = value; }
    public Vector2 Direction { get => _direction; set => _direction = value; }

    public event Action<bool> OnRun;


    protected override void OnEnable()
    {
        base.OnEnable();

        ConnectEvents();
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        LeaveEvents();
    }

    protected override void Start()
    {
        base.Start();
    }

    void ConnectEvents()
    {
        GetComponent<PlayerController>().OnStart += StartMovement;

        if (TryGetComponent(out InputManager inputManager))
        {
            inputManager.OnClickDown += ClickedInput;
            inputManager.OnClickUp += ClickedInput;
            inputManager.OnClickTotalDirection += GetDirection;
        }
        else
        {
            Debug.LogError("No InputManager on Player");
        }
    }

    void LeaveEvents()
    {
        GetComponent<PlayerController>().OnStart -= StartMovement;

        if (TryGetComponent(out InputManager inputManager))
        {
            inputManager.OnClickDown -= ClickedInput;
            inputManager.OnClickUp -= ClickedInput;
            inputManager.OnClickTotalDirection -= GetDirection;
        }
    }

    void ClickedInput(bool clicked)
    {
        Clicked = clicked;
    }

    void GetDirection(Vector2 direction)
    {
        if (_isStarted && !IsStopped)
        {
            Direction = direction;

            Move();

            Rotate();
        }

    }

    public void Jump()
    {
        _characterRB.AddForce((Vector3.up + transform.forward) * 10,ForceMode.VelocityChange);
    }

    protected override void Move()
    {

        Vector2 dir = Direction.normalized;

        float velY = _characterRB.velocity.y;
        _characterRB.velocity = new Vector3(dir.x, 0, dir.y) * Speed + Vector3.up * velY;

        float horizontalVelocity = new Vector3(_characterRB.velocity.x, 0, _characterRB.velocity.z).magnitude;
        OnRun?.Invoke(horizontalVelocity > 0.1f);
        
    }

    protected override void Rotate()
    {
        Vector2 dir = Direction.normalized;
        Quaternion look;
        if (dir != Vector2.zero)
        {
            look = Quaternion.LookRotation(new Vector3(dir.x, 0, dir.y));
            transform.rotation = Quaternion.Slerp(transform.rotation, look, Time.deltaTime * 17.5f);
        }

        //Vector3 relative = transform.InverseTransformPoint(transform.position + new Vector3(dir.x, 0, dir.y));
        //float angle = Mathf.Atan2(relative.x, relative.z) * Mathf.Rad2Deg;
        //transform.Rotate(0, angle, 0);

    }

}
