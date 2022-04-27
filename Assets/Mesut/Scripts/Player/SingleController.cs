
using System.Collections;
using DG.Tweening;
using DI;
using UnityEngine;

namespace JR
{
    public enum Gender { Male, Female }
    public class SingleController : MonoBehaviour, IProtector
    {
        [SerializeField] GenderInfo _genderInfo;
        [SerializeField] int _slapAnimationCount = 8;
        IAnimatorController _animatorController;

        public GenderInfo GenderInfo => _genderInfo;

        bool _isSlapping;
        public bool IsSlapping => _isSlapping;

        DoTweenSwapper _transformSwapper;
        ExhaustChecker _exhaustChecker;

        public void Init(InitParameters initParameters)
        {
            _animatorController = initParameters.AnimatorController;

            var swapperInitParameters = new DoTweenSwapper.InitParameters();
            swapperInitParameters.TransformToSwap = transform;
            swapperInitParameters.MoveSettings = initParameters.MoveSettings;
            _transformSwapper = new DoTweenSwapper(swapperInitParameters);

            var exhaustCheckerInitParameters = new ExhaustChecker.InitParameters();
            exhaustCheckerInitParameters.ProtectorController = initParameters.AnimatorController;
            exhaustCheckerInitParameters.Settings = initParameters.ExhaustCheckerSettings;

            _exhaustChecker = new ExhaustChecker();
            _exhaustChecker.Init(exhaustCheckerInitParameters);
        }

        public void Slap()
        {
            Debug.Log("Slap");
            if (_isSlapping) return;
            _isSlapping = true;
            _animatorController.SetLayerWeight(1, 1f);
            _animatorController.SetTrigger("slap");
            _animatorController.SetFloat("tokatIndex", Random.Range(0, _slapAnimationCount));
            StartCoroutine(RestartAnimatorWeight());
        }

        IEnumerator RestartAnimatorWeight()
        {
            yield return new WaitForSeconds(0.25f);
            float timer = 1f;
            DOTween.To(() => timer, (x) => { timer = x; _animatorController.SetLayerWeight(1, x); }, 0f, 0.25f)
                .OnComplete(() => _isSlapping = false);
        }

        public void Protect()
        {
            _transformSwapper.Swap();
            _exhaustChecker.StartTimer();
            _animatorController.SetTrigger("protectRun");
        }

        public void ReturnBack()
        {
            _transformSwapper.ReturnBack();
            _exhaustChecker.CheckForExhaust();
        }

        public class InitParameters
        {
            public IAnimatorController AnimatorController { get; set; }
            public DoTweenSwapper.MoveSettings MoveSettings { get; set; }
            public ExhaustChecker.Settings ExhaustCheckerSettings { get; set; }
            public bool IsProtector { get; set; }
        }
    }

    public interface IProtector
    {
        void Protect();
        void ReturnBack();
    }
}