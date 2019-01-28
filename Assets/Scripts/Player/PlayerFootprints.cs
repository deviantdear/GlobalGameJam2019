using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFootprints : MonoBehaviour
{
    public GameObject leftFootprint;
    public GameObject rightFootprint;

    public Transform leftFootLocation;
    public Transform rightFootLocation;

    public float footprintOffest = 0.05f;

    public AudioSource leftFootAudioSource;
    public AudioSource rightFootAudioSource;

	[SerializeField]
	private float footstepDelay = 0.1f;

	private float nextLeftFootstepTime;
	private float nextRightFootstepTime;

    public void leftFootstep()
    {
		if (Time.realtimeSinceStartup < nextLeftFootstepTime)
			return;

        // Play step audio
        leftFootAudioSource.Play();

        // Raycast out and create footprint
        RaycastHit hit;

        if(Physics.Raycast(leftFootLocation.position, leftFootLocation.forward, out hit))
        {
            Instantiate(leftFootprint, hit.point + hit.normal * footprintOffest, Quaternion.LookRotation(hit.normal, leftFootLocation.up));
        }

		nextLeftFootstepTime = Time.realtimeSinceStartup;
    }

    public void rightFootstep()
	{
		if (Time.realtimeSinceStartup < nextRightFootstepTime)
			return;
		
        // Play step audio
        rightFootAudioSource.Play();

        // Raycast out and create footprint
        RaycastHit hit;

        if(Physics.Raycast(rightFootLocation.position, rightFootLocation.forward, out hit))
        {
            Instantiate(rightFootprint, hit.point + hit.normal * footprintOffest, Quaternion.LookRotation(hit.normal, rightFootLocation.up));
        }

		nextRightFootstepTime = Time.realtimeSinceStartup;
    }


}
