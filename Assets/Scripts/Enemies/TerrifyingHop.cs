using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrifyingHop : MonoBehaviour
{
    CreepyCrawlyNavigator navigator;
    public float upDistance = 0.8f;
    public float forwardDistance = 0.1f;
    public float hopSpeed = 2f;
    
    private bool hopping = false;
    private float hopUpTime = -1f;
    private Vector3 hopToLocation;
    private Vector3 hopFromLocation;

    private void Start()
    {
        navigator = GetComponent<CreepyCrawlyNavigator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!hopping && other.transform.CompareTag("bedframe"))
        {
            hopping = true;
            navigator.Climb();
            hopFromLocation = transform.position;
            hopToLocation = transform.position + (transform.up * upDistance) + (Vector3.Normalize(other.transform.position - transform.position) * forwardDistance);
            hopUpTime = Time.time;
        }
    }

    private void Update()
    {
        if (hopping)
        {
            float flightPosition = (Time.time - hopUpTime) * hopSpeed;
            float percentFlight = flightPosition / Vector3.Distance(hopFromLocation, hopToLocation);
            transform.position = Vector3.Lerp(hopFromLocation, hopToLocation, percentFlight);
            transform.LookAt(hopToLocation);
            if (percentFlight >= 0.99f)
            {
                navigator.Enable();
                hopping = false;
            }
        }
    }
}
