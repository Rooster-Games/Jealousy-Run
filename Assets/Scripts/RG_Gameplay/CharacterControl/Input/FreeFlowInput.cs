//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using System;

//public class FreeFlowInput : InputManager
//{
//    [Space]
//    [Header("Free Flow")]
//    [SerializeField]
//    float _sensivity = 1;
//    [SerializeField]
//    float _maxDeltaXClamp = 1;
//    [SerializeField]
//    float _maxDeltaYClamp = 1;

//    [SerializeField] private float _deltaX;
//    [SerializeField] private float _deltaY;

//    [SerializeField] private float _totaldeltaX;
//    [SerializeField] private float _totaldeltaY;

//    [Space]
//    [Header("Runner")]
//    [SerializeField] private Vector3 _delta;
//    [SerializeField] private Vector3 _smoothDelta;
//    [SerializeField] private float xSensivity = 6;
//    [Range(0, 5)] [SerializeField] private float smoothingFactor = 0.6f;

//    public event Action<bool> OnClickDown;
//    public event Action<bool> OnClickUp;
//    public event Action<Vector2> OnClickTotalDirection;
//    public event Action<Vector2> OnClickDeltaDirection;

//    protected override void Start()
//    {
//        base.Start();
//    }

//    private void Update()
//    {
//        if (_started && !_ended)
//            GetInput();
//    }

//    protected override void GetInput()
//    {
//        if (Input.GetMouseButtonDown(0))
//        {
//            _currentPoint = Input.mousePosition;
//            _followingPoint = Input.mousePosition;
//            Clicked = true;
//            OnClickDown?.Invoke(Clicked);

//        }
//        if (Input.GetMouseButton(0))
//        {
//            if (Clicked == false)
//            {

//                _currentPoint = Input.mousePosition;
//                _followingPoint = Input.mousePosition;
//                Clicked = true;
//            }

//            _currentPoint = Input.mousePosition;

//            // SmoothDelta


//            _delta = _delta + xSensivity * (_currentPoint - _followingPoint);

//            _delta.x = Mathf.Clamp(_delta.x, -1, 1);

//            //


//            // Free Flow
//            _deltaX = _currentPoint.x - _followingPoint.x;
//            _deltaY = _currentPoint.y - _followingPoint.y;
//            _deltaX = ((_deltaX * 1080) / _screenWidth) * Time.deltaTime * _sensivity;
//            _deltaY = ((_deltaY * 1920) / _screenHeight) * Time.deltaTime * _sensivity;

//            _deltaX = Mathf.Clamp(_deltaX, -_maxDeltaXClamp, _maxDeltaXClamp);
//            _deltaY = Mathf.Clamp(_deltaY, -_maxDeltaYClamp, _maxDeltaYClamp);

//            _totaldeltaX += (_deltaX);

//            _totaldeltaY += (_deltaY);
//            ///


//            // Events
//            OnClickTotalDirection?.Invoke(new Vector2(_totaldeltaX,_totaldeltaY));
//            OnClickDeltaDirection?.Invoke(new Vector2(_deltaX, _deltaY));
//            //

//            _followingPoint = _currentPoint;
//            Clicked = true;
//        }
//        else
//        {
//            _followingPoint = Vector3.zero;
//            _currentPoint = Vector3.zero;
//            _deltaY = 0;
//            _deltaX = 0;
//            _totaldeltaX = 0;
//            _totaldeltaY = 0;
//            Clicked = false;
//            OnClickUp?.Invoke(Clicked);
//            OnClickTotalDirection?.Invoke(new Vector2(_totaldeltaX, _totaldeltaY));
//            OnClickDeltaDirection?.Invoke(new Vector2(_deltaX, _deltaY));
//        }

//        _smoothDelta = Vector3.Lerp(_smoothDelta, _delta, Time.deltaTime * 20f * smoothingFactor);
//    }


//    void MoveSample()
//    {
//        // Sample from before games
//        float sideValue = Mathf.Clamp(Mathf.Lerp(0, 3.5f * Mathf.Sign(_smoothDelta.x), Mathf.Abs(_smoothDelta.x)), -2.5f, 2.5f);

//        transform.localPosition = new Vector3(sideValue, 0, 0);

//    }
//}
