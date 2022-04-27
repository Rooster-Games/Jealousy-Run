using System;
using System.Collections.Generic;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;
using static RG.Handlers.RoosterEventHandler;

namespace RG.Loader
{
    public class UI_Loop : MonoBehaviour
    {
        [SerializeField] [BoxGroup("Transition Panel")]
        public GameObject transitionPanel;

        [BoxGroup("Transition Panel")] public bool useTransition;


        public List<UIObject> uILoop = new List<UIObject>();

        [HideInInspector] public int _uiIndex = -1;

        public static Action<bool> OnChangeUI;

        private void OnEnable()
        {
            RoosterHub.Central.OnGameStartedHandler += OnGameStarted;
            OnShowTransition += TransitionAction;
            OnChangeUI += ChangeUIMenu;
        }

        private void OnDisable()
        {
            RoosterHub.Central.OnGameStartedHandler -= OnGameStarted;
            if (OnChangeUI != null) OnChangeUI -= ChangeUIMenu;
            if (OnShowTransition != null) OnShowTransition -= TransitionAction;
        }

        void Start()
        {
            ChangeUIMenu(true);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.U))
            {
                ChangeUIMenu(true);
            }
        }

        private void TransitionAction(bool status)
        {
            transitionPanel.SetActive(true);
            ITransition trans = transitionPanel.GetComponent<ITransition>();
            
            if (trans == null)
            {
                Debug.LogError("TRANSITION PANEL BULUNAMADI");
            }
            else
            {
                if (status)
                {
                    trans.CloseAction();
                }
                else
                {
                    trans.OpenAction();
                }
                
            }
        }

        private void OnTrasectionComplete()
        {
            uILoop[GetCurrentUIIndex()].OpenMenu();
            ITransition trans = transitionPanel.GetComponent<ITransition>();
            trans.OpenAction();
        }

        private void ChangeUIMenu(bool closeOthers)
        {
            if (closeOthers)
            {
                foreach (var item in uILoop)
                {
                    item.CloseMenu();
                }
            }

            _uiIndex++;

            if (uILoop[GetCurrentUIIndex()].useUnlockQueue)
            {
                if (uILoop[GetCurrentUIIndex()].IsUnlockableCanvas())
                {
                    uILoop[GetCurrentUIIndex()].OpenMenu();
                }
                else
                {
                    ChangeUIMenu(closeOthers);
                }
            }
            else
            {
                uILoop[GetCurrentUIIndex()].OpenMenu();
            }
        }

        private int GetCurrentUIIndex()
        {
            var currentUiIndex = _uiIndex % uILoop.Count;
            return currentUiIndex;
        }

        private void OnGameStarted()
        {
            ChangeUIMenu(true);
        }
    }
}