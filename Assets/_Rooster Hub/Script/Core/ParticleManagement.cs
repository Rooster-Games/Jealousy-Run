using System.Collections.Generic;
using UnityEngine;

public class ParticleManagement : MonoBehaviour
{
    public static ParticleManagement Instance;
    
    [SerializeField] private GameObject confettiParticle;
    [SerializeField] private GameObject impactParticle;
    [SerializeField] private List<GameObject> customParticles = new List<GameObject>();

    private void Awake()
    {
        Instance = this;
    }

    public GameObject SpawnConfettiParticle()
    {
        GameObject obj =Instantiate(confettiParticle);
        return obj;
    }
    public GameObject SpawnConfettiParticle(float killTime)
    {
        GameObject obj =Instantiate(confettiParticle);
        Destroy(obj,killTime);
        return obj;
    }

    public GameObject SpawnImpactParticle()
    {
        GameObject obj = Instantiate(impactParticle);
        return obj;
    }
    public GameObject SpawnImpactParticle(float killTime)
    {
        GameObject obj = Instantiate(impactParticle);
        Destroy(obj,killTime);
        return obj;
    }

    public GameObject SpawnCustomParticle(int index)
    {
        GameObject obj = Instantiate(customParticles[index]);
        return obj;
    }
    public GameObject SpawnCustomParticle(int index,float killTime)
    {
        GameObject obj = Instantiate(customParticles[index]);
        Destroy(obj,killTime);
        return obj;
    }

}
