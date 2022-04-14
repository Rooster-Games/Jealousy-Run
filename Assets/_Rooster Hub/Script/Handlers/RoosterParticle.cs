using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RoosterParticle 
{
    public static GameObject SpawnConfetti()
    {
        GameObject x =RoosterParticleManager.Instance.SpawnConfettiParticle();
        return x;
    }
    public static GameObject SpawnConfetti(float killTime)
    {
        GameObject x =RoosterParticleManager.Instance.SpawnConfettiParticle(killTime);
        return x;
    }
    
    public static GameObject SpawnImpact()
    {
        GameObject x =RoosterParticleManager.Instance.SpawnImpactParticle();
        return x;
    }
    public static GameObject SpawnImpact(float killTime)
    {
        GameObject x =RoosterParticleManager.Instance.SpawnImpactParticle(killTime);
        return x;
    }

    public static GameObject SpawnCustomParticle(int index)
    {
        GameObject x = RoosterParticleManager.Instance.SpawnCustomParticle(index);
        return x;
    }
    public static GameObject SpawnCustomParticle(int index,float killTime)
    {
        GameObject x = RoosterParticleManager.Instance.SpawnCustomParticle(index,killTime);
        return x;
    }
}
