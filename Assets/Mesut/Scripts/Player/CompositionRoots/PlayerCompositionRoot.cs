using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

namespace JR
{
    public class PlayerCompositionRoot : MonoBehaviour
    {
        [SerializeField] PlayerSettingsSO _playerSettings;

        public void Awake()
        {
            Init(new InitParameters());
        }

        public void Init(InitParameters initParameters)
        {
            // CompositionRoot
            DollyCartCompositionRoot dollyCartCompositionRoot = GetComponent<DollyCartCompositionRoot>();

            // Self
            var dollyCart = GetComponent<CinemachineDollyCart>();

            // Child
            var dollyCartController = GetComponentInChildren<DollyCartController>();

            // Creation of InitParameters
            var dollyCartCompositionRootInitParameters = new DollyCartCompositionRoot.InitParameters();
            dollyCartCompositionRootInitParameters.DollyCart = dollyCart;
            dollyCartCompositionRootInitParameters.DollyCartController = dollyCartController;
            dollyCartCompositionRootInitParameters.DollyCartSettings = _playerSettings.DollyCartSettings;

            dollyCartCompositionRoot.Init(dollyCartCompositionRootInitParameters);

        }

        public class InitParameters
        {

        }
    }
}