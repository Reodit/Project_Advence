using System;
using UnityEngine;
using System.Collections;

public class PixelArsenalProjectileScript : MonoBehaviour
{
    public GameObject impactParticle;
    public GameObject projectileParticle;
    public GameObject muzzleParticle;
    public GameObject[] trailParticles;
    [HideInInspector]
    public Vector3 impactNormal; //Used to rotate impactparticle.

    void Start()
    {
	    if (projectileParticle)
	    {
		    projectileParticle.SetActive(true);
	    }
		
        if (muzzleParticle)
		{
			muzzleParticle.SetActive(true);
			if (gameObject.activeInHierarchy)
			{
				StartCoroutine(DisableAfterTime(muzzleParticle, 1.5f));  // Lifetime of muzzle effect.
			}
		}
    }
    
    public void OnCol()
    {
	    if (impactParticle)
	    {
		    impactParticle.SetActive(true);
		    if (gameObject.activeInHierarchy)
		    {
			    StartCoroutine(DisableAfterTime(impactParticle, 3f));   
		    }
        }

	    for (int i = 1; i < trailParticles.Length; i++)
	    {
		    ParticleSystem trail = trailParticles[i].GetComponent<ParticleSystem>();
		
		    if (trail.gameObject.name.Contains("Trail"))
		    {
			    // TODO What this code??
			    trail.transform.SetParent(null);
			    if (gameObject.activeInHierarchy)
			    {
				    StartCoroutine(DisableAfterTime(trail.gameObject, 2f));
			    }
		    }
	    }
    }
    
    private IEnumerator DisableAfterTime(GameObject obj, float delay)
    {
	    yield return new WaitForSeconds(delay);
	    obj.SetActive(false); // 오브젝트 비활성화
    }
}