public class RigidbodyPropCopier
{
    public UnityEngine.Vector3 velocity { get; set; }
    public UnityEngine.Vector3 angularVelocity { get; set; }
    public System.Single drag { get; set; }
    public System.Single angularDrag { get; set; }
    public System.Single mass { get; set; }
    public System.Boolean useGravity { get; set; }
    public System.Single maxDepenetrationVelocity { get; set; }
    public System.Boolean isKinematic { get; set; }
    public System.Boolean freezeRotation { get; set; }
    public UnityEngine.RigidbodyConstraints constraints { get; set; }
    public UnityEngine.CollisionDetectionMode collisionDetectionMode { get; set; }
    public UnityEngine.Vector3 centerOfMass { get; set; }
    public UnityEngine.Quaternion inertiaTensorRotation { get; set; }
    public UnityEngine.Vector3 inertiaTensor { get; set; }
    public System.Boolean detectCollisions { get; set; }
    public UnityEngine.Vector3 position { get; set; }
    public UnityEngine.Quaternion rotation { get; set; }
    public UnityEngine.RigidbodyInterpolation interpolation { get; set; }
    public System.Int32 solverIterations { get; set; }
    public System.Single sleepThreshold { get; set; }
    public System.Single maxAngularVelocity { get; set; }
    public System.Int32 solverVelocityIterations { get; set; }
    public System.Single sleepVelocity { get; set; }
    public System.Single sleepAngularVelocity { get; set; }
    public System.Boolean useConeFriction { get; set; }
    public System.Int32 solverIterationCount { get; set; }
    public System.Int32 solverVelocityIterationCount { get; set; }
    public System.String tag { get; set; }
    public System.String name { get; set; }
    public UnityEngine.HideFlags hideFlags { get; set; }

    public RigidbodyPropCopier(UnityEngine.Rigidbody instance)
   {
        this.velocity = instance.velocity;
        this.angularVelocity = instance.angularVelocity;
        this.drag = instance.drag;
        this.angularDrag = instance.angularDrag;
        this.mass = instance.mass;
        this.useGravity = instance.useGravity;
        this.maxDepenetrationVelocity = instance.maxDepenetrationVelocity;
        this.isKinematic = instance.isKinematic;
        this.freezeRotation = instance.freezeRotation;
        this.constraints = instance.constraints;
        this.collisionDetectionMode = instance.collisionDetectionMode;
        this.centerOfMass = instance.centerOfMass;
        this.inertiaTensorRotation = instance.inertiaTensorRotation;
        this.inertiaTensor = instance.inertiaTensor;
        this.detectCollisions = instance.detectCollisions;
        this.position = instance.position;
        this.rotation = instance.rotation;
        this.interpolation = instance.interpolation;
        this.solverIterations = instance.solverIterations;
        this.sleepThreshold = instance.sleepThreshold;
        this.maxAngularVelocity = instance.maxAngularVelocity;
        this.solverVelocityIterations = instance.solverVelocityIterations;
        this.sleepVelocity = instance.sleepVelocity;
        this.sleepAngularVelocity = instance.sleepAngularVelocity;
        this.useConeFriction = instance.useConeFriction;
        this.solverIterationCount = instance.solverIterations;
        this.solverVelocityIterationCount = instance.solverVelocityIterations;
        this.tag = instance.tag;
        this.name = instance.name;
        this.hideFlags = instance.hideFlags;
    }


    public void CopyTo(ref UnityEngine.Rigidbody other)
   {
       other.velocity = this.velocity;
       other.angularVelocity = this.angularVelocity;
       other.drag = this.drag;
       other.angularDrag = this.angularDrag;
       other.mass = this.mass;
       other.useGravity = this.useGravity;
       other.maxDepenetrationVelocity = this.maxDepenetrationVelocity;
       other.isKinematic = this.isKinematic;
       other.freezeRotation = this.freezeRotation;
       other.constraints = this.constraints;
       other.collisionDetectionMode = this.collisionDetectionMode;
       other.centerOfMass = this.centerOfMass;
       other.inertiaTensorRotation = this.inertiaTensorRotation;
       other.inertiaTensor = this.inertiaTensor;
       other.detectCollisions = this.detectCollisions;
       other.position = this.position;
       other.rotation = this.rotation;
       other.interpolation = this.interpolation;
       other.solverIterations = this.solverIterations;
       other.sleepThreshold = this.sleepThreshold;
       other.maxAngularVelocity = this.maxAngularVelocity;
       other.solverVelocityIterations = this.solverVelocityIterations;
       other.sleepVelocity = this.sleepVelocity;
       other.sleepAngularVelocity = this.sleepAngularVelocity;
       other.useConeFriction = this.useConeFriction;
       other.solverIterations = this.solverIterationCount;
       other.solverVelocityIterations = this.solverVelocityIterationCount;
       other.tag = this.tag;
       other.name = this.name;
       other.hideFlags = this.hideFlags;
 }
}