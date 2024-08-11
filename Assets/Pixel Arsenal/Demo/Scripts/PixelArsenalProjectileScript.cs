using System;
using UnityEngine;
using System.Collections;

public class PixelArsenalProjectileScript : MonoBehaviour
{
    public ParticleSystem impactParticle;
    public ParticleSystem projectileParticle;
    public ParticleSystem muzzleParticle;
    public ParticleSystem[] trailParticles;
    [HideInInspector]
    public Vector3 impactNormal; //Used to rotate impactparticle.

    void Start()
    {
        projectileParticle.gameObject.SetActive(true);
		
        if (muzzleParticle)
		{
			muzzleParticle.gameObject.SetActive(true);
			Destroy(muzzleParticle.gameObject, 1.5f); // Lifetime of muzzle effect.
		}
    }
    
    public void OnCol()
    {
        impactParticle.gameObject.SetActive(true);
        Destroy(impactParticle, 3f);

	    for (int i = 1; i < trailParticles.Length; i++)
	    {
		    ParticleSystem trail = trailParticles[i];
		
		    if (trail.gameObject.name.Contains("Trail"))
		    {
			    // TODO What this code??
			    trail.transform.SetParent(null);
                Destroy(trail, 2f);
		    }
	    }
    }
}