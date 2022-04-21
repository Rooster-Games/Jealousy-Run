public class ConfigurableJointPropCopier
{
    public UnityEngine.Vector3 secondaryAxis { get; set; }
    public UnityEngine.ConfigurableJointMotion xMotion { get; set; }
    public UnityEngine.ConfigurableJointMotion yMotion { get; set; }
    public UnityEngine.ConfigurableJointMotion zMotion { get; set; }
    public UnityEngine.ConfigurableJointMotion angularXMotion { get; set; }
    public UnityEngine.ConfigurableJointMotion angularYMotion { get; set; }
    public UnityEngine.ConfigurableJointMotion angularZMotion { get; set; }
    public UnityEngine.SoftJointLimitSpring linearLimitSpring { get; set; }
    public UnityEngine.SoftJointLimitSpring angularXLimitSpring { get; set; }
    public UnityEngine.SoftJointLimitSpring angularYZLimitSpring { get; set; }
    public UnityEngine.SoftJointLimit linearLimit { get; set; }
    public UnityEngine.SoftJointLimit lowAngularXLimit { get; set; }
    public UnityEngine.SoftJointLimit highAngularXLimit { get; set; }
    public UnityEngine.SoftJointLimit angularYLimit { get; set; }
    public UnityEngine.SoftJointLimit angularZLimit { get; set; }
    public UnityEngine.Vector3 targetPosition { get; set; }
    public UnityEngine.Vector3 targetVelocity { get; set; }
    public UnityEngine.JointDrive xDrive { get; set; }
    public UnityEngine.JointDrive yDrive { get; set; }
    public UnityEngine.JointDrive zDrive { get; set; }
    public UnityEngine.Quaternion targetRotation { get; set; }
    public UnityEngine.Vector3 targetAngularVelocity { get; set; }
    public UnityEngine.RotationDriveMode rotationDriveMode { get; set; }
    public UnityEngine.JointDrive angularXDrive { get; set; }
    public UnityEngine.JointDrive angularYZDrive { get; set; }
    public UnityEngine.JointDrive slerpDrive { get; set; }
    public UnityEngine.JointProjectionMode projectionMode { get; set; }
    public System.Single projectionDistance { get; set; }
    public System.Single projectionAngle { get; set; }
    public System.Boolean configuredInWorldSpace { get; set; }
    public System.Boolean swapBodies { get; set; }
    public UnityEngine.Rigidbody connectedBody { get; set; }
    public UnityEngine.ArticulationBody connectedArticulationBody { get; set; }
    public UnityEngine.Vector3 axis { get; set; }
    public UnityEngine.Vector3 anchor { get; set; }
    public UnityEngine.Vector3 connectedAnchor { get; set; }
    public System.Boolean autoConfigureConnectedAnchor { get; set; }
    public System.Single breakForce { get; set; }
    public System.Single breakTorque { get; set; }
    public System.Boolean enableCollision { get; set; }
    public System.Boolean enablePreprocessing { get; set; }
    public System.Single massScale { get; set; }
    public System.Single connectedMassScale { get; set; }
    public System.String tag { get; set; }
    public System.String name { get; set; }
    public UnityEngine.HideFlags hideFlags { get; set; }
    public ConfigurableJointPropCopier(UnityEngine.ConfigurableJoint instance)
   {
        this.secondaryAxis = instance.secondaryAxis;
        this.xMotion = instance.xMotion;
        this.yMotion = instance.yMotion;
        this.zMotion = instance.zMotion;
        this.angularXMotion = instance.angularXMotion;
        this.angularYMotion = instance.angularYMotion;
        this.angularZMotion = instance.angularZMotion;
        this.linearLimitSpring = instance.linearLimitSpring;
        this.angularXLimitSpring = instance.angularXLimitSpring;
        this.angularYZLimitSpring = instance.angularYZLimitSpring;
        this.linearLimit = instance.linearLimit;
        this.lowAngularXLimit = instance.lowAngularXLimit;
        this.highAngularXLimit = instance.highAngularXLimit;
        this.angularYLimit = instance.angularYLimit;
        this.angularZLimit = instance.angularZLimit;
        this.targetPosition = instance.targetPosition;
        this.targetVelocity = instance.targetVelocity;
        this.xDrive = instance.xDrive;
        this.yDrive = instance.yDrive;
        this.zDrive = instance.zDrive;
        this.targetRotation = instance.targetRotation;
        this.targetAngularVelocity = instance.targetAngularVelocity;
        this.rotationDriveMode = instance.rotationDriveMode;
        this.angularXDrive = instance.angularXDrive;
        this.angularYZDrive = instance.angularYZDrive;
        this.slerpDrive = instance.slerpDrive;
        this.projectionMode = instance.projectionMode;
        this.projectionDistance = instance.projectionDistance;
        this.projectionAngle = instance.projectionAngle;
        this.configuredInWorldSpace = instance.configuredInWorldSpace;
        this.swapBodies = instance.swapBodies;
        this.connectedBody = instance.connectedBody;
        this.connectedArticulationBody = instance.connectedArticulationBody;
        this.axis = instance.axis;
        this.anchor = instance.anchor;
        this.connectedAnchor = instance.connectedAnchor;
        this.autoConfigureConnectedAnchor = instance.autoConfigureConnectedAnchor;
        this.breakForce = instance.breakForce;
        this.breakTorque = instance.breakTorque;
        this.enableCollision = instance.enableCollision;
        this.enablePreprocessing = instance.enablePreprocessing;
        this.massScale = instance.massScale;
        this.connectedMassScale = instance.connectedMassScale;
        this.tag = instance.tag;
        this.name = instance.name;
        this.hideFlags = instance.hideFlags;
    }


    public void CopyTo(ref UnityEngine.ConfigurableJoint other)
   {
       other.secondaryAxis = this.secondaryAxis;
       other.xMotion = this.xMotion;
       other.yMotion = this.yMotion;
       other.zMotion = this.zMotion;
       other.angularXMotion = this.angularXMotion;
       other.angularYMotion = this.angularYMotion;
       other.angularZMotion = this.angularZMotion;
       other.linearLimitSpring = this.linearLimitSpring;
       other.angularXLimitSpring = this.angularXLimitSpring;
       other.angularYZLimitSpring = this.angularYZLimitSpring;
       other.linearLimit = this.linearLimit;
       other.lowAngularXLimit = this.lowAngularXLimit;
       other.highAngularXLimit = this.highAngularXLimit;
       other.angularYLimit = this.angularYLimit;
       other.angularZLimit = this.angularZLimit;
       other.targetPosition = this.targetPosition;
       other.targetVelocity = this.targetVelocity;
       other.xDrive = this.xDrive;
       other.yDrive = this.yDrive;
       other.zDrive = this.zDrive;
       other.targetRotation = this.targetRotation;
       other.targetAngularVelocity = this.targetAngularVelocity;
       other.rotationDriveMode = this.rotationDriveMode;
       other.angularXDrive = this.angularXDrive;
       other.angularYZDrive = this.angularYZDrive;
       other.slerpDrive = this.slerpDrive;
       other.projectionMode = this.projectionMode;
       other.projectionDistance = this.projectionDistance;
       other.projectionAngle = this.projectionAngle;
       other.configuredInWorldSpace = this.configuredInWorldSpace;
       other.swapBodies = this.swapBodies;
       other.connectedBody = this.connectedBody;
       other.connectedArticulationBody = this.connectedArticulationBody;
       other.axis = this.axis;
       other.anchor = this.anchor;
       other.connectedAnchor = this.connectedAnchor;
       other.autoConfigureConnectedAnchor = this.autoConfigureConnectedAnchor;
       other.breakForce = this.breakForce;
       other.breakTorque = this.breakTorque;
       other.enableCollision = this.enableCollision;
       other.enablePreprocessing = this.enablePreprocessing;
       other.massScale = this.massScale;
       other.connectedMassScale = this.connectedMassScale;
       other.tag = this.tag;
       other.name = this.name;
       other.hideFlags = this.hideFlags;
 }
}