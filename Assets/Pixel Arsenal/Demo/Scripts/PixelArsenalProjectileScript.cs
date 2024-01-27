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
        projectileParticle = Instantiate(projectileParticle, transform.position, Quaternion.identity, transform) as GameObject;
		
        if (muzzleParticle)
		{
			muzzleParticle = Instantiate(muzzleParticle, transform.position, Quaternion.identity, transform) as GameObject;
			Destroy(muzzleParticle, 1.5f); // Lifetime of muzzle effect.
		}
    }
    
    public void OnCol()
    {
	    try
	    {
		    impactParticle = Instantiate(impactParticle, transform.position, Quaternion.FromToRotation(Vector3.up, impactNormal)) as GameObject;

		    foreach (GameObject trail in trailParticles)
		    {
			    if (trail != null)
			    {
				    try
				    {
					    GameObject curTrail = transform.Find(projectileParticle.name + "/" + trail.name).gameObject;
					    curTrail.transform.parent = null;
					    Destroy(curTrail, 3f);
				    }
				    catch (Exception e)
				    {
				    }
			    }
		    }
		    Destroy(projectileParticle, 3f);
		    Destroy(impactParticle, 5f);
		    Destroy(gameObject);
		
		    ParticleSystem[] trails = GetComponentsInChildren<ParticleSystem>();
		    //Component at [0] is that of the parent i.e. this object (if there is any)
		    for (int i = 1; i < trails.Length; i++)
		    {
			
			    ParticleSystem trail = trails[i];
			
			    if (trail.gameObject.name.Contains("Trail"))
			    {
				    trail.transform.SetParent(null);
				    Destroy(trail.gameObject, 2f);
			    }
		    }
	    }
	    catch (Exception e)
	    {
	    }
        
        
    }
}