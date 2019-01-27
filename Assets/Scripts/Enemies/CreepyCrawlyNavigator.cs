using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CreepyCrawlyNavigator : MonoBehaviour
{
    [SerializeField]
    NavMeshAgent navMeshAgent;
    Animator animator;

    public Vector3 target;

	// Use this for initialization
	void Start () {
        if (!navMeshAgent)
            navMeshAgent = GetComponent<NavMeshAgent>();
        if (!animator)
            animator = GetComponent<Animator>();
        //navMeshAgent.updatePosition = false;

        navMeshAgent.SetDestination(target);

    }
	
	// Update is called once per frame
	void Update () {

        if (animator != null && navMeshAgent.enabled)
            NavAgentAnimation();
	}

    // Smash this dead function to stop the creeper from crawling
    public void Dead()
    {
        //navMeshAgent.isStopped = true;
        navMeshAgent.enabled = false;
        animator.SetBool("dead", true);
    }

    Vector2 smoothDeltaPosition = Vector2.zero;
    Vector2 velocity = Vector2.zero;
    void NavAgentAnimation()
    {

        bool shouldMove = navMeshAgent.velocity.sqrMagnitude > 0.01f;

        // Update animation parameters
        animator.SetBool("move", shouldMove);
    }

    public void Enable()
    {
        navMeshAgent.enabled = true;
        navMeshAgent.SetDestination(target);
    }

    public void Climb()
    {
        navMeshAgent.enabled = false;
        animator.SetBool("move", true);
    }
}
