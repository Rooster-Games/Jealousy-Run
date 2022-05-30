using System.Collections;
using System.Collections.Generic;
using GameCores;
using GameCores.CoreEvents;
using TMPro;
using UIThings;
using UnityEngine;
using UnityEngine.EventSystems;

public class TutorialTrigger_1 : MonoBehaviour
{
    [SerializeField] GameObject _tutorialCanvas;
    [SerializeField] CursorController _cursorController;
    [SerializeField] TextMeshProUGUI _textMeshPro;
    [SerializeField] string _message;
    [SerializeField] bool _isCursorInverse;
    [SerializeField] EventTrigger _eventTrigger;
    [SerializeField] bool _isDown;
    [SerializeField] float _timeScale = 0.15f;

    [SerializeField] BoxCollider _boxCollider;

    bool _isTriggered;

    bool _isClicked;
    bool _skipTrigger;

    private void Awake()
    {
        _boxCollider = GetComponent<BoxCollider>();
        _cursorController.InverseScale = _isCursorInverse;
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0) && !_isDown)
        {
            _isClicked = true;
            _skipTrigger = false;
        }

        if(Input.GetMouseButtonUp(0) && !_isTriggered && _isClicked && !_isDown)
        {
            _skipTrigger = true;
        }

        if(_isTriggered && (Input.GetMouseButtonDown(0) || Input.GetMouseButtonUp(0)))
        {
            EndTutorial();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_skipTrigger) return;
        _textMeshPro.text = _message;
        _isTriggered = true;
        _eventTrigger.gameObject.SetActive(true);
        _boxCollider.enabled = false;
        Time.timeScale = _timeScale;
        _tutorialCanvas.SetActive(true);
    }

    public void EndTutorial()
    {
        Debug.Log("EndTutorial");
        _tutorialCanvas.SetActive(false);
        Time.timeScale = 1f;
        enabled = false;
    }

    public class InitParameters
    {
        public IEventBus EventBus { get; set; }
    }
}