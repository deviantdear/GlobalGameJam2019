using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrifyingHop : MonoBehaviour
{
    public float upPercent = 0.8f;
    public float forwardPercent = 0.1f;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("bedframe"))
        {
            transform.position = transform.position + (transform.up * upPercent) + (transform.forward * forwardPercent);
        }
    }
}
