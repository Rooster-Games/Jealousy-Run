//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class RunnerInput : InputManager
//{
//    public float leftClamp = -2.5f;
//    public float rightClamp = 2.5f;

//    private Vector3 _delta;
//    private Vector3 _smoothDelta;
//    [SerializeField] private float xSensivity = 6;
//    [Range(0, 5)] [SerializeField] private float smoothingFactor = 0.6f;


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
//            _currentPoint = new Vector3(Input.mousePosition.x / Screen.width, Input.mousePosition.y / Screen.height, 0);
//            _followingPoint = _currentPoint;
//        }
//        if (Input.GetMouseButton(0))
//        {
//            Vector3 mousePosition = new Vector3(Input.mousePosition.x / Screen.width, Input.mousePosition.y / Screen.height, 0);

//            _delta = _delta + xSensivity * (mousePosition - _followingPoint);

//            _delta.x = Mathf.Clamp(_delta.x, -1, 1);

//            _followingPoint = mousePosition;
//        }

//        _smoothDelta = Vector3.Lerp(_smoothDelta, _delta, Time.deltaTime * 20f * smoothingFactor);

//    }

//    void MoveSample()
//    {
//        // Sample from before games
//        float sideValue = Mathf.Clamp(Mathf.Lerp(0, 3.5f * Mathf.Sign(_smoothDelta.x), Mathf.Abs(_smoothDelta.x)), leftClamp, rightClamp);
 
//        transform.localPosition = new Vector3(sideValue, 0, 0);

//    }
//}
