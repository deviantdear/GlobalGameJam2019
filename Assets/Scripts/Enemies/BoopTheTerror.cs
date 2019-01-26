using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BoopTheTerror : MonoBehaviour
{
    public int boopsToDie = 2;
    public float debounce = 1f;
    public float knockBackDistance = 5f;
    public float knockBackSpeed = 1f;
    
    private int boops = 0;
    private bool knockedBack = false;
    private float knockBackTime = -1f;
    private Vector3 knockToLocation;
    private Vector3 knockFromLocation;

    public UnityEvent literallyFuckingDead = new UnityEvent();
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            if (knockBackTime + debounce > Time.time)
                return;

            boops += 1;
            if (boops >= boopsToDie)
            {
                LiterallyFuckingDie();
            }

            knockedBack = true;
            knockFromLocation = transform.position;
            knockToLocation = knockFromLocation.normalized * knockBackDistance;
            knockBackTime = Time.time;
        }
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

    void LiterallyFuckingDie()
    {
        literallyFuckingDead.Invoke();
    }
}
