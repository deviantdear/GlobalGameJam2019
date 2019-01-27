using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightmareishChomp : MonoBehaviour {

    [SerializeField]
    private LayerMask damageMask = (LayerMask)(-1);
    [SerializeField]
    private float damage = 1f;
    [SerializeField]
    private float attackRate = 1f;

    private float lastAttack = 0f;
    private bool attacking = false;

    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnCollisionStay(Collision collision)
    {
        HandleCollision(collision.gameObject);
    }

    private void OnTriggerStay(Collider other)
    {
        HandleCollision(other.gameObject);
    }

    private void HandleCollision(GameObject other)
    {
        if (attacking)
            return;

        // Deal damage
        if (damageMask.Contains(other.layer))
        {
            var health = other.GetComponentInParent<Health>();
            if (!health)
            {
                health = other.GetComponentInChildren<Health>();
            }
            if (health)
            {
                attacking = true;
                lastAttack = Time.time;
                health.Damage(damage);
                animator.SetBool("attack", true);
            }
        }
    }

    private void Update()
    {
        if (attacking && lastAttack + attackRate < Time.time)
        {
            attacking = false;
            animator.SetBool("attack", false);
        }
    }
}
