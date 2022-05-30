using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JR
{
    public class TutorialController : MonoBehaviour
    {
        [SerializeField] GameObject[] _tutorialLevelPrefabs;

        public GameObject GetTutorial(int index)
        {
            return _tutorialLevelPrefabs[index];
        }
    }
}