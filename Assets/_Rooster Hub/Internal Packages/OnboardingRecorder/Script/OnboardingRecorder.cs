using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using NaughtyAttributes;

#if UNITY_EDITOR
public class OnboardingRecorder : MonoBehaviour
{
    [Expandable] public PositionRecorderSO record;
    public Transform onboardingObject;


    private List<Vector3> positions = new List<Vector3>();
    public float pointCollectTime;
    public float objectMovementSpeed;
    private float _tempTime;
    private bool _startedRecord;
    int index = 0;

    [Button("Start Recording")]
    public void ToggleRecord()
    {
        _startedRecord = !_startedRecord;
        Debug.LogError(_startedRecord);
    }

    [Button("Clear Records")]
    public void Clear()
    {
        positions.Clear();
        record.ClearNodes();
        record.SavePositions();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            StartToMoveOnboarding();
        }

        if (!_startedRecord) return;

        if (Input.GetMouseButtonDown(0))
        {
            _tempTime = pointCollectTime;
            positions.Add(Input.mousePosition);
        }

        if (Input.GetMouseButton(0))
        {
            _tempTime -= Time.deltaTime;
            if (_tempTime < 0)
            {
                positions.Add(Input.mousePosition);
                _tempTime = pointCollectTime;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            _tempTime = 0;
            ToggleRecord();
            record.AddNodeInPositions(positions);
            record.SavePositions();
        }
    }


    public void StartToMoveOnboarding()
    {
        onboardingObject.DOMove(record.points[index], objectMovementSpeed).OnComplete(() =>
        {
            index++;
            if (index > positions.Count)
            {
                index = 0;
                onboardingObject.transform.position = positions[index];
            }

            StartToMoveOnboarding();
        }).SetEase(Ease.Linear).SetSpeedBased();
    }
}
#endif