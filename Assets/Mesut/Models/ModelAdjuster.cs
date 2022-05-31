using System.Collections;
using System.Collections.Generic;
using JR;
using NaughtyAttributes;
using UnityEngine;

public class ModelAdjuster : MonoBehaviour
{
    [SerializeField] GameObject[] _prefabs;
    [SerializeField] GameObject _newModel;

    [Button("Create")]
    public void CreateNews()
    {
        foreach (var prefab in _prefabs)
        {
            var animator = prefab.GetComponentInChildren<Animator>();
            var instantiated = Instantiate(_newModel);

            var meshRenderer = animator.GetComponentInChildren<SkinnedMeshRenderer>();
            Material[] newMaterials = new Material[meshRenderer.sharedMaterials.Length];
            meshRenderer.sharedMaterials.CopyTo(newMaterials, 0);


            var anim = instantiated.GetComponent<Animator>();
            instantiated.AddComponent<PersonAnimatorController>();

            var renderers = instantiated.GetComponentsInChildren<SkinnedMeshRenderer>();
            foreach (var renderer in renderers)
            {
                renderer.sharedMaterials = newMaterials;
            }

            anim.runtimeAnimatorController = animator.runtimeAnimatorController;
            anim.avatar = animator.avatar;

            instantiated.transform.SetParent(animator.transform.parent);

            DestroyImmediate(animator.gameObject);
        }
    }

    // 0 =
    // 1 =
    // element2 = skin
    // 3 =
    // 4 = 
}
