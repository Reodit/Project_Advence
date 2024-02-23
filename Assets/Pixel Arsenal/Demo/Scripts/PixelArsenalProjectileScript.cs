using System;
using UnityEngine;
using System.Collections;

public class PixelArsenalProjectileScript : MonoBehaviour
{
    public ParticlePoolObject impactParticle;
    public ParticlePoolObject projectileParticle;
    public ParticlePoolObject muzzleParticle;
    public ParticlePoolObject[] trailParticles;
    [HideInInspector]
    public Vector3 impactNormal; //Used to rotate impactparticle.

    void Start()
    {
        projectileParticle = ObjectPooler.Instance.Particle.GetFromPool(projectileParticle, transform.position, Quaternion.identity, transform);
		
        if (muzzleParticle)
		{
			muzzleParticle = ObjectPooler.Instance.Particle.GetFromPool(muzzleParticle, transform.position, Quaternion.identity, transform);
			WaitForDestroy(muzzleParticle, 1.5f); // Lifetime of muzzle effect.
		}
    }
    
    public void OnCol()
    {
	    try
	    {
		    impactParticle = ObjectPooler.Instance.Particle.GetFromPool(impactParticle, transform.position, Quaternion.FromToRotation(Vector3.up, impactNormal));

		    WaitForDestroy(projectileParticle, 3f);
            WaitForDestroy(impactParticle, 5f);

            ParticlePoolObject[] trails = GetComponentsInChildren<ParticlePoolObject>();
		    //Component at [0] is that of the parent i.e. this object (if there is any)
		    for (int i = 1; i < trails.Length; i++)
		    {
			    ParticlePoolObject trail = trails[i];
			
			    if (trail.gameObject.name.Contains("Trail"))
			    {
				    trail.transform.SetParent(null);
				    WaitForDestroy(trail, 2f);
			    }
		    }
	    }
	    catch (Exception e)
	    {
	    }
    }

	private void WaitForDestroy(ParticlePoolObject particle, float waitTime)
	{
		StartCoroutine(CoWaitForDestroy(particle, waitTime));
	}

	private IEnumerator CoWaitForDestroy(ParticlePoolObject particle, float waitTime)
	{
		yield return new WaitForSeconds(waitTime);
		ObjectPooler.Instance.Particle.ReturnToPool(particle);
	}

	
}