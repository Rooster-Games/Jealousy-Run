using UnityEngine;
using System.Collections;
using DIC;
using GameCores;
using GameCores.CoreEvents;

namespace JR
{
    public class RoleSelector : MonoBehaviour, ICompRoot<Gender>
    {
        [SerializeField] SingleController[] _coupleControllerCollection;
        [SerializeField] BarCompositionSettings _barCompositionSettings;

        Transform _protectorTransform;
        Transform _guardedTransform;
        Gender _protectorGender;

        public void RegisterToContainer(Gender protectorGender)
        {
            _protectorGender = protectorGender;

            for(int i = 0; i < _coupleControllerCollection.Length; i++)
            {
                var singleController = _coupleControllerCollection[i];

                if(singleController.GenderInfo.Gender == protectorGender)
                {
                    IProtector protector = singleController;
                    DIContainer.Instance.RegisterSingle(protector);
                    DIContainer.Instance.RegisterWhenInjectTo<ISwapper>(singleController.transform);
                    _protectorTransform = singleController.transform;
                }
                else
                {
                    IGuarded guarded = singleController;
                    DIContainer.Instance.RegisterSingle(guarded);
                    _guardedTransform = singleController.transform;
                }

                singleController.GetComponentInChildren<SkinnedMeshRenderer>().enabled = true;
            }

            DIContainer.Instance.RegisterSingle(_barCompositionSettings.BarController);
            DIContainer.Instance.RegisterSingle(_barCompositionSettings.LoveData);
            DIContainer.Instance.RegisterWhenInjectTo(_barCompositionSettings.LoveData, _barCompositionSettings.StartingPercent);

            foreach (var bar in _barCompositionSettings.Bars)
            {
                bar.gameObject.SetActive(false);

                if (protectorGender == bar.BoundedGender)
                {
                    DIContainer.Instance.RegisterSingle<BaseBar>(bar);
                    DIContainer.Instance.RegisterWhenInjectTo(bar, _barCompositionSettings.StartingPercent);
                }
            }
        }

        public void Init(InitParameters initParameters)
        {
            var coupleTransformSettings = initParameters.CoupleTransformSettings;

            _protectorTransform.localPosition = coupleTransformSettings.ProtectorStartingPosition;
            _guardedTransform.localPosition = coupleTransformSettings.DefendedStartingPosition;

            foreach (var animationGenderSetting in initParameters.AnimationGenderSettings)
            {
                if (animationGenderSetting.Gender == _protectorGender)
                {
                    _protectorTransform.GetComponentInChildren<Animator>().runtimeAnimatorController = animationGenderSetting.ProtectorRunTimeAnimatorController;
                    initParameters.EventBus.Register<OnGameStarted>( (e) => _protectorTransform.GetComponentInChildren<ItemTriggerDetector>(true).gameObject.SetActive(true));
                }
                else
                {

                    _guardedTransform.GetComponentInChildren<Animator>().runtimeAnimatorController = animationGenderSetting.GuardedAnimatorController;
                    // _guardedTransform.GetComponentInChildren<ItemTriggerDetector>(true).gameObject.SetActive(false);
                }
            }
        }

        public class InitParameters
        {
            public CoupleTransformSettings CoupleTransformSettings { get; set; }
            public SingleController.AnimatorGenderSettings[] AnimationGenderSettings { get; set; }
            public IEventBus EventBus { get; set; }
        }
    }
}
