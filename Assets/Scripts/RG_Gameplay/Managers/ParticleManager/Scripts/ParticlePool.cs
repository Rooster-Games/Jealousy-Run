using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ParticlePool : MonoBehaviour
{
    [SerializeField]
    List<Particle> particles = new List<Particle>();

    private void Awake()
    {
        Create();
    }

    void Create()
    {
        for(int i = 0; i < particles.Count; i++)
        {
            particles[i].FillPool(this.transform);
        }
    }

    public bool SearchPool(string particleName,out ParticleSystem particleSystem)
    {
        if(particles.Any((x)=> x.particleName.Equals(particleName)))
        {
            Particle particle = particles.First((x) => x.particleName.Equals(particleName));

            if (particle != null)
            {
                particleSystem = particle.GetParticle();
                return particleSystem;
            }
            else
            {
                Debug.LogError("There is no " + particleName.ToUpper() + " at pool! Please assign at ParticleManager!");
                particleSystem = null;
                return false;
            }
        }

        else
        {
            Debug.LogError("There is no " + particleName.ToUpper() + " at pool! Please assign at ParticleManager!");
            particleSystem = null;
            return false;
        }
    }

    [System.Serializable]
    public class Particle
    {
        public string particleName;
        [SerializeField]
        ParticleSystem particleSystem;
        [SerializeField]
        int count;
        List<ParticleSystem> particlePool = new List<ParticleSystem>();
        Transform parentTransform;
        public void FillPool(Transform parent)
        {
            parentTransform = parent;
            for (int i = 0; i < count; i++)
            {
                ParticleSystem partSystem = Instantiate(particleSystem, parentTransform);
                particlePool.Add(partSystem);
            }
        }

        public ParticleSystem GetParticle()
        {
            foreach (ParticleSystem particle in particlePool)
            {
                if (!particle.gameObject.activeInHierarchy)
                {
                    particle.gameObject.SetActive(true);
                    return particle;
                }
            }

            ParticleSystem newPatrticleSystem = Instantiate(particleSystem, parentTransform);
            newPatrticleSystem.gameObject.SetActive(true);
            particlePool.Add(newPatrticleSystem);

            return newPatrticleSystem;
        }

    }
}

