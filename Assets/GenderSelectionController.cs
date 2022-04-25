using System;
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

        public event Action OnGenderSelected;

        public Gender GameTypeGender => _gameTypeGender;
        public bool IsGenderSelected => _isGenderSelected;

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
                _showPanelButton.gameObject.SetActive(true);
            }
            ActivateSelectionButtons(!hasKey);
        }

        public void ShowPanel()
        {
            _isPanelShowed = !_isPanelShowed;
            ActivateSelectionButtons(_isPanelShowed);
        }

        private void ActivateSelectionButtons(bool state)
        {
            foreach (var button in _genderSelectionButtonCollection)
            {
                button.gameObject.SetActive(state);
            }
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
            OnGenderSelected?.Invoke();
        }
    }
}