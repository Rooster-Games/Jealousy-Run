using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JR
{
    // local position swapper
    public class CoroutineSwapper : MonoBehaviour, ISwapper
    {
        Transform _transformToSwap;
        DoTweenSwapper.MoveSettings _moveSettings;

        Vector3 _initialPosition;

        float totalSideDistance;
        float totalForwardDistance;

        Vector3 _targetPosition;
        float _percent;
        float _timer;


        public void Init(ISwapper.InitParameters initParameters)
        {
            _transformToSwap = initParameters.TransformToSwap;
            _moveSettings = initParameters.MoveSettings;

            _initialPosition = initParameters.CoupleTransformSettings.ProtectorStartingPosition;

            _targetPosition = new Vector3(_moveSettings.MoveSideDistance, _transformToSwap.localPosition.y, _moveSettings.MoveForwardDistance);
        }

        Coroutine _swap;

        public float WayPercent => _percent;

        public void Swap()
        {
            if (_swap != null)
                StopCoroutine(_swap);

            _swap = StartCoroutine(SwapCoroutine(1, () => _percent < 1f));
        }

        public void ReturnBack()
        {
            if (_swap != null)
                StopCoroutine(_swap);

            _swap = StartCoroutine(SwapCoroutine(-1, () => _percent > 0f));
        }

        IEnumerator SwapCoroutine(int timeDirection, System.Func<bool> condition)
        {
            Vector3 startingPosition = _initialPosition;
            while (condition())
            {
                _timer += Time.deltaTime * timeDirection;
                _percent = _timer / _moveSettings.MoveDuration;

                ChangePosition(_percent);
                yield return null;
            }

            void ChangePosition(float percent)
            {
                float sideCurvePercent = _moveSettings.SideMoveCurve.Evaluate(percent);
                float xPos = Mathf.Lerp(startingPosition.x, _targetPosition.x, sideCurvePercent);

                float forwardCurvePercent = _moveSettings.ForwardMoveCurve.Evaluate(percent);
                float zPos = Mathf.Lerp(startingPosition.z, _targetPosition.z, forwardCurvePercent);

                _transformToSwap.localPosition = new Vector3(xPos, _transformToSwap.localPosition.y, zPos);
            }
        }
    }
}
