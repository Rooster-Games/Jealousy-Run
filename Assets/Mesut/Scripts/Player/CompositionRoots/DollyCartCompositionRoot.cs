
using Cinemachine;
using UnityEngine;

namespace JR
{
    public class DollyCartCompositionRoot : MonoBehaviour
    {
        public void Init(InitParameters initParameters)
        {
            var dollyCart = initParameters.DollyCart;
            var dollyCartController = initParameters.DollyCartController;
            var dollyCartSettings = initParameters.DollyCartSettings;

            var dollyCartControllerInitParameters = new DollyCartController.InitParameters();
            dollyCartControllerInitParameters.Settings = dollyCartSettings;
            dollyCartControllerInitParameters.DollyCart = dollyCart;

            dollyCartController.Init(dollyCartControllerInitParameters);
        }

        public class InitParameters
        {
            public DollyCartController.Settings DollyCartSettings { get; set; }
            public DollyCartController DollyCartController { get; set; }
            public CinemachineDollyCart DollyCart { get; set; }
        }
    }
}