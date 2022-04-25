using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using JetBrains.Annotations;
using NaughtyAttributes;
using RootMotion.Dynamics;
using UnityEngine;

public class PeopleCollisionDetector : MonoBehaviour
{
    PuppetMaster _myPuppet;
    [SerializeField] float _pinWeight = 0f;

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision");
        _myPuppet.pinWeight = _pinWeight;
    }
}