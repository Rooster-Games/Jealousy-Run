using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParrticleTester : MonoBehaviour
{
    [SerializeField]
    string particle1;

    [SerializeField]
    string particle2;

    private void Update()
    {

        if (Input.GetMouseButtonDown(1))
        {
            ParticleManager.Instance?.PlayParticle(particle1, transform.position, transform.rotation);
        }
        if (Input.GetMouseButtonDown(0))
        {
            ParticleManager.Instance?.PlayParticle(particle2, transform.position, transform.rotation);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        ParticleManager.Instance?.PlayParticle(particle1, transform.position, transform.rotation);
    }
}
