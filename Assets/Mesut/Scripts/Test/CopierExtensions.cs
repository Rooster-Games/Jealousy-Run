using System.Collections;
using System.Collections.Generic;
using RootMotion.Demos;
using UnityEngine;

public static class CopierExtensions
{
    public static void CopyTo(this Rigidbody self, ref Rigidbody other)
    {
        RigidbodyPropCopier copier = new RigidbodyPropCopier(self);
        copier.CopyTo(ref other);
    }

    public static void CopyTo(this ConfigurableJoint self, ref ConfigurableJoint other)
    {
        ConfigurableJointPropCopier copier = new ConfigurableJointPropCopier(self);
        copier.CopyTo(ref other);
    }
}