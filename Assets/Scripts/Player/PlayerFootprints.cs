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

    public void leftFootstep()
    {
        // Play step audio
        leftFootAudioSource.Play();

        // Raycast out and create footprint
        RaycastHit hit;

        if(Physics.Raycast(leftFootLocation.position, leftFootLocation.forward, out hit))
        {
            Instantiate(leftFootprint, hit.point + hit.normal * footprintOffest, Quaternion.LookRotation(hit.normal, leftFootLocation.up));
        }
    }

    public void rightFootstep()
    {
        // Play step audio
        rightFootAudioSource.Play();

        // Raycast out and create footprint
        RaycastHit hit;

        if(Physics.Raycast(rightFootLocation.position, rightFootLocation.forward, out hit))
        {
            Instantiate(rightFootprint, hit.point + hit.normal * footprintOffest, Quaternion.LookRotation(hit.normal, rightFootLocation.up));
        }
    }


}
