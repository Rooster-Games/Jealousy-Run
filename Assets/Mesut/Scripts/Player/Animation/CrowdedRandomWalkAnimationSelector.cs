using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdedRandomWalkAnimationSelector : MonoBehaviour
{
    [SerializeField] int _walkAnimationIndexCount = 6;
    [SerializeField] Vector2 _animSpeedMultiplier = new Vector2(0.9f, 1.1f);
    Animator []_animators;

    List<int> _animationIndexList = new List<int>();
    private void Awake()
    {
        _animators = GetComponentsInChildren<Animator>();


        foreach (var animator in _animators)
        {
            if (_animationIndexList.Count == 0)
                FillList();

            int randomIndex = Random.Range(0, _animationIndexList.Count);
            int index = _animationIndexList[randomIndex];
            _animationIndexList.RemoveAt(randomIndex);

            animator.SetFloat("walkASMultiplier", Random.Range(_animSpeedMultiplier.x, _animSpeedMultiplier.y));
            animator.SetFloat("walkIndex", index);
            animator.SetTrigger("walk");
        }
    }

    private void FillList()
    {
        for (int i = 0; i < _walkAnimationIndexCount; i++)
        {
            _animationIndexList.Add(i);
        }

    }
}
