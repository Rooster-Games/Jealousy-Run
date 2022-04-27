using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace JR
{
    public class CharacterSelector : MonoBehaviour
    {
        [SerializeField] GenderAnimatorSettings[] _animatorSettings;
        [SerializeField] Animator _animator;

        public void Init(InitParameters initParameters)
        {
            var infos = GetComponentsInChildren<GenderInfo>(true).Where(x => x.Gender == initParameters.Gender).ToList();
            var randomIndex = Random.Range(0, infos.Count);
            var info = infos[randomIndex];
            info.gameObject.SetActive(true);

            foreach (var settings in _animatorSettings)
            {
                if(settings.Gender == initParameters.Gender)
                {
                    _animator.runtimeAnimatorController = settings.RuntimeAnimatorController;
                }
            }
        }

        public class InitParameters
        {
            public Gender Gender { get; set; }
        }

        [System.Serializable]
        public class GenderAnimatorSettings
        {
            [SerializeField] RuntimeAnimatorController _animator;
            [SerializeField] Gender _gender;

            public RuntimeAnimatorController RuntimeAnimatorController => _animator;
            public Gender Gender => _gender;
        }
    }
}