using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using NaughtyAttributes;

public class InputManager : MonoBehaviour
{
    [Foldout("Info")] [ReadOnly] [SerializeField] protected bool _started = false;
    [Foldout("Info")] [ReadOnly] [SerializeField] protected bool _ended = false;
    [Foldout("Info")] [ReadOnly] [SerializeField] protected bool _clicked = false;

    public bool Clicked { get => _clicked; set => _clicked = value; }

    protected Vector3 _currentPoint;
    protected Vector3 _followingPoint;

    protected float _screenWidth;
    protected float _screenHeight;


    [Space]
    [Header("Free Flow")]
    [SerializeField]
    float _sensivity = 1;
    [SerializeField]
    float _maxDeltaXClamp = 1;
    [SerializeField]
    float _maxDeltaYClamp = 1;

    [SerializeField] private float _deltaX;
    [SerializeField] private float _deltaY;

    [SerializeField] private float _totaldeltaX;
    [SerializeField] private float _totaldeltaY;

    public float DeltaX { get => _deltaX; }
    public float DeltaY { get => _deltaY; }
    public float TotalDeltaX { get => _totaldeltaX; }
    public float TotalDeltaY { get => _totaldeltaY; }

    [Space]
    [Header("Runner")]
    [SerializeField] private Vector3 _delta;
    [SerializeField] private Vector3 _smoothDelta;
    [SerializeField] private float xSensivity = 6;
    [Range(0, 5)] [SerializeField] private float smoothingFactor = 0.6f;
    Vector3 _oldMousePosition;
    Vector3 _firstMousePosition;

    public Vector3 SmoothDelta { get => _smoothDelta; }

    public event Action<bool> OnClickDown;
    public event Action<bool> OnClickUp;
    public event Action<Vector2> OnClickTotalDirection;
    public event Action<Vector2> OnClickDeltaDirection;

    protected virtual void Start()
    {
        _screenHeight = Screen.height;
        _screenWidth = Screen.width;
    }

    private void Update()
    {
            GetInput();
    }

    protected virtual void GetInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _currentPoint = Input.mousePosition;
            _followingPoint = Input.mousePosition;

            _firstMousePosition = new Vector3(Input.mousePosition.x / Screen.width, Input.mousePosition.y / Screen.height, 0);
            _oldMousePosition = _firstMousePosition;

            Clicked = true;
            OnClickDown?.Invoke(Clicked);

        }
        if (Input.GetMouseButton(0))
        {
            if (Clicked == false)
            {



                _currentPoint = Input.mousePosition;
                _followingPoint = Input.mousePosition;
                Clicked = true;
            }

            _currentPoint = Input.mousePosition;

            // SmoothDelta


            Vector3 mousePosition = new Vector3(Input.mousePosition.x / _screenWidth, Input.mousePosition.y / _screenHeight, 0);

            _delta = _delta + xSensivity * (mousePosition - _oldMousePosition);

            _delta.x = Mathf.Clamp(_delta.x, -1, 1);

            //


            // Free Flow
            _deltaX = _currentPoint.x - _followingPoint.x;
            _deltaY = _currentPoint.y - _followingPoint.y;
            _deltaX = ((_deltaX * 1080) / _screenWidth) * Time.deltaTime * _sensivity;
            _deltaY = ((_deltaY * 1920) / _screenHeight) * Time.deltaTime * _sensivity;

            _deltaX = Mathf.Clamp(_deltaX, -_maxDeltaXClamp, _maxDeltaXClamp);
            _deltaY = Mathf.Clamp(_deltaY, -_maxDeltaYClamp, _maxDeltaYClamp);

            _totaldeltaX += (_deltaX);

            _totaldeltaY += (_deltaY);
            ///


            // Events
            OnClickTotalDirection?.Invoke(new Vector2(_totaldeltaX, _totaldeltaY));
            OnClickDeltaDirection?.Invoke(new Vector2(_deltaX, _deltaY));
            //

            _oldMousePosition = mousePosition;
            _followingPoint = _currentPoint;
            Clicked = true;
        }
        else
        {
            _followingPoint = Vector3.zero;
            _currentPoint = Vector3.zero;
            _deltaY = 0;
            _deltaX = 0;
            _totaldeltaX = 0;
            _totaldeltaY = 0;
            Clicked = false;
            OnClickTotalDirection?.Invoke(new Vector2(_totaldeltaX, _totaldeltaY));
            OnClickDeltaDirection?.Invoke(new Vector2(_deltaX, _deltaY));
        }

        if (Input.GetMouseButtonUp(0))
        {
            Clicked = false;
            OnClickUp?.Invoke(Clicked);
        }

        _smoothDelta = Vector3.Lerp(_smoothDelta, _delta, Time.deltaTime * 20f * smoothingFactor);
    }


    void MoveSample()
    {
        // Sample from before games
        float sideValue = Mathf.Clamp(Mathf.Lerp(0, 3.5f * Mathf.Sign(_smoothDelta.x), Mathf.Abs(_smoothDelta.x)), -2.5f, 2.5f);

        transform.localPosition = new Vector3(sideValue, 0, 0);

    }

}
