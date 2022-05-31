using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using JR;
using NaughtyAttributes;
using RootMotion.Dynamics;
using RootMotion;
using UnityEngine;

public class CharacterCreator : MonoBehaviour
{
    [SerializeField] GameObject[] _prefabs;
    [SerializeField] GameObject _ragdollPrefab;


    [Button("Create")]
    public void Create()
    {
        foreach (var prefab in _prefabs)
        {
            var animatorGO = prefab.GetComponentInChildren<Animator>();
            GameObject ragdollGO = Instantiate(_ragdollPrefab);
            ragdollGO.transform.localPosition = Vector3.zero;

            ragdollGO.transform.SetParent(animatorGO.transform.parent);
            ragdollGO.transform.localEulerAngles= Vector3.zero;
            ragdollGO.transform.localScale = Vector3.one;

            DestroyImmediate(prefab.GetComponentInChildren<RagdollMarker>(true).gameObject);
            ragdollGO.AddComponent<RagdollMarker>();

            var ragdollCreator = ragdollGO.AddComponent<BipedRagdollCreator>();

            CheckRagdollCreator(ragdollCreator);

            BipedRagdollCreator.Create(ragdollCreator.references, ragdollCreator.options);
            DestroyImmediate(ragdollCreator);

            var slapable = prefab.GetComponent<Slapable>();
            slapable._ragdoll = ragdollGO;

            var hips = FindInChilds(ragdollGO.transform, "mixamorig:Hips");
            var spine = FindInChilds(ragdollGO.transform, "mixamorig:Spine2");
            var leftArm = FindInChilds(ragdollGO.transform, "mixamorig:LeftArm");
            var rightArm = FindInChilds(ragdollGO.transform, "mixamorig:RightArm");

            var rigidBodies = new Rigidbody[4];
            rigidBodies[0] = hips.GetComponent<Rigidbody>();
            rigidBodies[1] = spine.GetComponent<Rigidbody>();
            rigidBodies[2] = leftArm.GetComponent<Rigidbody>();
            rigidBodies[3] = rightArm.GetComponent<Rigidbody>();

            slapable._ragdollBodies = rigidBodies;

            ragdollGO.gameObject.SetActive(false);

            var animatorGORenderer = animatorGO.GetComponentInChildren<SkinnedMeshRenderer>();
            var ragdolRenderer = ragdollGO.GetComponentInChildren<SkinnedMeshRenderer>();

            ragdolRenderer.sharedMaterials = new Material[animatorGORenderer.sharedMaterials.Length];
            animatorGORenderer.sharedMaterials.CopyTo(ragdolRenderer.sharedMaterials, 0);
        }
    }

    private Transform FindInChilds(Transform root, string name)
    {
        var childCount = root.childCount;
        Transform foundTransform = null;
        for(int i = 0; i < childCount; i++)
        {
            var child = root.GetChild(i);
            Debug.Log(child.name);
            if (child.name.Contains(name))
                foundTransform = child;

            if (foundTransform != null) break;

            foundTransform = FindInChilds(child, name);
        }

        return foundTransform;
    }

    private void CheckRagdollCreator(BipedRagdollCreator script)
    {
        if (script.references.IsEmpty(false))
        {
            Animator animator = script.gameObject.GetComponent<Animator>();

            if (animator == null && script.references.root != null)
            {
                animator = script.references.root.GetComponentInChildren<Animator>();
                if (animator == null) animator = GetAnimatorInParents(script.references.root);
            }

            if (animator != null)
            {
                script.references = BipedRagdollReferences.FromAvatar(animator);

            }
            else
            {
                BipedReferences r = new BipedReferences();
                BipedReferences.AutoDetectReferences(ref r, script.transform, BipedReferences.AutoDetectParams.Default);
                if (r.isFilled) script.references = BipedRagdollReferences.FromBipedReferences(r);
            }

            if (!OnRoot(script))
            {
                Debug.LogWarning("BipedRagdollCreator must be added to the root of the character. Destroying the component.");
                DestroyImmediate(script);
                return;
            }

            string msg = string.Empty;
            if (script.references.IsValid(ref msg))
            {
                script.options = BipedRagdollCreator.AutodetectOptions(script.references);
                //BipedRagdollCreator.Create(script.references, script.options);

                //if (animator != null) DestroyImmediate(animator);
                //if (script.GetComponent<Animation>() != null) DestroyImmediate(script.GetComponent<Animation>());
            }
        }
    }


    private bool OnRoot(BipedRagdollCreator script)
    {
        if (script.references.Contains(script.transform, true)) return false;
        if (script.references.root != null)
        {
            var bipedRagdollCreatorOnChild = script.references.root.GetComponentInChildren<BipedRagdollCreator>();
            if (bipedRagdollCreatorOnChild != null && bipedRagdollCreatorOnChild.transform != script.references.root) return false;
        }
        return true;
    }

    private Animator GetAnimatorInParents(Transform transform)
    {
        if (transform.GetComponent<Animator>() != null) return transform.GetComponent<Animator>();
        if (transform.parent == null) return null;
        return GetAnimatorInParents(transform.parent);
    }
}
