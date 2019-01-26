using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlargDed : MonoBehaviour
{

    public void Die()
    {
        transform.rotation = Quaternion.Inverse(transform.rotation);
    }
}
