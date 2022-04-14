using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIToggle : MonoBehaviour
{
    private Button toggleButton;
    [SerializeField] private GameObject toggleGameObject;
    [SerializeField] private Button closeButton;

    void Awake()
    {
        toggleButton = GetComponent<Button>();
    }

    private void OnEnable()
    {
        toggleButton.onClick.AddListener(ToogleButton);
        closeButton.onClick.AddListener(CloseObject);
    }

    private void OnDisable()
    {
        toggleButton.onClick.RemoveListener(ToogleButton);
        closeButton.onClick.RemoveListener(CloseObject);
    }

    void ToogleButton()
    {
        RoosterHaptic.Selection();
        RoosterSound.PlayButtonSound();
        
        toggleGameObject.SetActive(true);
    }

    void CloseObject()
    {
        RoosterHaptic.Selection();
        RoosterSound.PlayButtonSound();
        
        toggleGameObject.SetActive(false);
    }
}