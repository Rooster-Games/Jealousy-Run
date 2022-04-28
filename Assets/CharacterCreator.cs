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
    [SerializeField] GameObject _baseObject;
    [SerializeField] GameObject[] _prefabCol;
    [SerializeField] DynamicBone _dynamicBone;
    [SerializeField] RuntimeAnimatorController _runtimeAnimatorController; 

    [Button("Create")]
    public void Create()
    {
        foreach (var prefab in _prefabCol)
        {
            GameObject root = Instantiate(_baseObject);
            root.transform.position = Vector3.zero;
            root.name = prefab.name;

            var obj = Instantiate(prefab);
            obj.transform.SetParent(root.transform);
            obj.transform.position = Vector3.zero;
            obj.transform.eulerAngles = Vector3.zero;
            obj.transform.localScale = Vector3.one;
            obj.GetComponent<Animator>().runtimeAnimatorController = _runtimeAnimatorController;

            var dynamicbone = obj.AddComponent<DynamicBone>();
            dynamicbone.m_Damping = _dynamicBone.m_Damping;
            dynamicbone.m_Elasticity = _dynamicBone.m_Elasticity;
            dynamicbone.m_Stiffness = _dynamicBone.m_Stiffness;

            var dynamicBoneTransform = FindInChilds(obj.transform, "mixamorig:Hips");
            dynamicbone.m_Root = dynamicBoneTransform;

            var ragdoll = Instantiate(prefab);
            ragdoll.transform.SetParent(root.transform);
            ragdoll.transform.position = Vector3.zero;
            ragdoll.transform.localEulerAngles = Vector3.zero;
            ragdoll.transform.localScale = Vector3.one;
            DestroyImmediate(ragdoll.GetComponent<Animator>());
            var ragdollCreator = ragdoll.AddComponent<BipedRagdollCreator>();

            CheckRagdollCreator(ragdollCreator);

            BipedRagdollCreator.Create(ragdollCreator.references, ragdollCreator.options);
            DestroyImmediate(ragdollCreator);

            var slapable = root.GetComponent<Slapable>();
            slapable._closeObject = obj;
            slapable._ragdoll = ragdoll;

            var hips = FindInChilds(ragdoll.transform, "mixamorig:Hips");
            var spine = FindInChilds(ragdoll.transform, "mixamorig:Spine2");
            var leftArm = FindInChilds(ragdoll.transform, "mixamorig:LeftArm");
            var rightArm = FindInChilds(ragdoll.transform, "mixamorig:RightArm");

            var rigidBodies = new Rigidbody[4];
            rigidBodies[0] = hips.GetComponent<Rigidbody>();
            rigidBodies[1] = spine.GetComponent<Rigidbody>();
            rigidBodies[2] = leftArm.GetComponent<Rigidbody>();
            rigidBodies[3] = rightArm.GetComponent<Rigidbody>();

            slapable._ragdollBodies = rigidBodies;

            ragdoll.gameObject.SetActive(false);
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
