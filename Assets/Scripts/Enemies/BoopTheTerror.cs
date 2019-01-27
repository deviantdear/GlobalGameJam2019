using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BoopTheTerror : MonoBehaviour
{
    public float debounce = 1f;
    public float knockBackDistance = 5f;
    public float knockBackSpeed = 1f;
    private bool knockedBack = false;
    private float knockBackTime = -1f;
    private Vector3 knockToLocation;
    private Vector3 knockFromLocation;
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            Knockback();
        }
    }

    public void Knockback()
    {
        if (knockBackTime + debounce > Time.time)
            return;

        knockedBack = true;
        knockFromLocation = transform.position;
        knockToLocation = knockFromLocation.normalized * knockBackDistance;
        knockBackTime = Time.time;
    }

    private void Update()
    {
        if (knockedBack)
        {
            float flightPosition = (Time.time - knockBackTime) * knockBackSpeed;
            float percentFlight = flightPosition / knockBackDistance;
            transform.position = Vector3.Lerp(knockFromLocation, knockToLocation, percentFlight);
            if (percentFlight >= 0.95)
            {
                knockedBack = false;
            }
        }
    }
    
}
