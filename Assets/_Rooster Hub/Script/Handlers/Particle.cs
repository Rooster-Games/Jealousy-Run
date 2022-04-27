using UnityEngine;

namespace RoosterHub
{
    public static class Particle 
    {
        public static GameObject SpawnConfetti()
        {
            GameObject x = ParticleManagement.Instance.SpawnConfettiParticle();
            return x;
        }

        public static GameObject SpawnConfetti(float killTime)
        {
            GameObject x = ParticleManagement.Instance.SpawnConfettiParticle(killTime);
            return x;
        }

        public static GameObject SpawnImpact()
        {
            GameObject x = ParticleManagement.Instance.SpawnImpactParticle();
            return x;
        }

        public static GameObject SpawnImpact(float killTime)
        {
            GameObject x = ParticleManagement.Instance.SpawnImpactParticle(killTime);
            return x;
        }

        public static GameObject SpawnCustomParticle(int index)
        {
            GameObject x = ParticleManagement.Instance.SpawnCustomParticle(index);
            return x;
        }

        public static GameObject SpawnCustomParticle(int index, float killTime)
        {
            GameObject x = ParticleManagement.Instance.SpawnCustomParticle(index, killTime);
            return x;
        }
    }
}