using System.Collections;
using System.Collections.Generic;
using DIC;
using GameCores;
using UnityEngine;

namespace JR
{
    public class SettingsCompositionRoot :BaseCompRootGO
    {
        [SerializeField] PlayerSettingsSO _playerSettings;
        [SerializeField] GameSettingsSO _gameSettings;

        public override void RegisterToContainer()
        {
            RegisterSettings(_playerSettings);
            RegisterSettings(_gameSettings);
        }

        private void RegisterSettings<T>(T settingsObject)
        {
            // Debug.Log("RegisteringSettings");

            var t = typeof(T);
            // Debug.Log("Type Of settings: " + t.Name);

            var fieldsInfo = t.GetFields(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.GetProperty);

            // Debug.Log("Settings FieldsInfo Count: " + fieldsInfo.Length);

            foreach (var fieldInfo in fieldsInfo)
            {
                // Debug.Log("Setttings field info: " + fieldInfo.FieldType);
                DIContainer.Instance.RegisterSingle(fieldInfo.GetValue(settingsObject), false, fieldInfo.FieldType);
            }
        }
    }
}