using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlargDed : MonoBehaviour
{
    public Transform materialContainer;
    Material dissolveMaterial;
    private bool dead = false;
    private float deadTime = 0f;
    public float deathEffectTime = 3f;

    private void Start()
    {
        dissolveMaterial = materialContainer.GetComponent<Renderer>().material;
    }

    private void Update()
    {
        if (dead)
        {
            float effect = (Time.time - deadTime) / deathEffectTime;
            dissolveMaterial.SetFloat("_DissolveAmount", effect);
            if (effect >= 1f)
            {
                Destroy(gameObject);
            }
        }
            
    }


    public void Die()
    {
        dead = true;
        transform.rotation = Quaternion.Inverse(transform.rotation);
        deadTime = Time.time;
    }
}
