using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace JR
{
    public class GenderSelectionController : MonoBehaviour
    {
        private const string GENDER_SELECTION_STR = "GameTypeGenderSelection";
        [SerializeField] Gender _gameTypeGender;

        bool _isGenderSelected;
        int _selectedGenderINT;
        bool _isPanelShowed;

        [SerializeField] Button _showPanelButton;
        [SerializeField] Button[] _genderSelectionButtonCollection;
        [SerializeField] GameObject[] _buttonGraphics;
        [SerializeField] GameObject _xButton;
        [SerializeField] GameObject _xOutLine;
        [SerializeField] GameObject _xImageObject;
        [SerializeField] float _waitBeforeShowPanelDuration = 1f;

        public event Action OnGenderSelected;

        public Gender GameTypeGender => _gameTypeGender;
        public bool IsGenderSelected => _isGenderSelected;

        Tween _openTween;

        public void Init()
        {
            ControlPlayerPrefs();
            RoosterHub.Central.OnGameStartedHandler += CloseShowPanelButton;
        }

        private void OnDestroy()
        {
            RoosterHub.Central.OnGameStartedHandler -= CloseShowPanelButton;
        }

        private void CloseShowPanelButton()
        {
            if(_openTween != null)
            {
                _openTween.Kill();
                _openTween = null;
            }
            _showPanelButton.gameObject.SetActive(false);
            ActivateSelectionButtons(false);
        }

        private void ControlPlayerPrefs()
        {
            bool hasKey = PlayerPrefs.HasKey(GENDER_SELECTION_STR);
            if(hasKey)
            {
                _selectedGenderINT = PlayerPrefs.GetInt(GENDER_SELECTION_STR);
                _gameTypeGender = (Gender)_selectedGenderINT;
                _isGenderSelected = true;
            }

            if (hasKey)
            {
                float timer = 0f;
                _openTween = DOTween.To(() => timer, (x) => timer = x, 1f, _waitBeforeShowPanelDuration)
                    .OnComplete(() => _showPanelButton.gameObject.SetActive(hasKey));
            }
            else
                _showPanelButton.gameObject.SetActive(hasKey);


            ActivateSelectionButtons(!hasKey);

            if(!hasKey)
            {
                _xButton.gameObject.SetActive(hasKey);
                _xImageObject.gameObject.SetActive(hasKey);
                _xOutLine.gameObject.SetActive(hasKey);
            }
        }

        public void ShowPanel()
        {
            _isPanelShowed = !_isPanelShowed;
            _showPanelButton.gameObject.SetActive(!_isPanelShowed);
            ActivateSelectionButtons(_isPanelShowed);
        }

        private void ActivateSelectionButtons(bool state)
        {
            foreach (var button in _genderSelectionButtonCollection)
            {
                button.gameObject.SetActive(state);
            }
            ActivateGraphics(state);
        }

        public void SelectGender(int genderIndex)
        {
            if (_isGenderSelected && genderIndex != _selectedGenderINT)
            {
                int loadedSceneIndex = SceneManager.GetActiveScene().buildIndex;
                SceneManager.LoadScene(loadedSceneIndex);
            }

            _selectedGenderINT = genderIndex;
            PlayerPrefs.SetInt(GENDER_SELECTION_STR, _selectedGenderINT);

            _gameTypeGender = (Gender)genderIndex;

            ActivateSelectionButtons(false);
            _isPanelShowed = false;

            _isGenderSelected = true;
            try
            {

                OnGenderSelected?.Invoke();
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }

        private void ActivateGraphics(bool state)
        {
            foreach (var go in _buttonGraphics)
            {
                go.SetActive(state);
            }
        }
    }
}