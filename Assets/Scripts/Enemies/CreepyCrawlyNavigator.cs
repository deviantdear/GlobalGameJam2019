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
        //navMeshAgent.updatePosition = false;

        navMeshAgent.SetDestination(target);

    }
	
	// Update is called once per frame
	void Update () {

        if (animator != null)
            NavAgentAnimation();
	}

    Vector2 smoothDeltaPosition = Vector2.zero;
    Vector2 velocity = Vector2.zero;
    void NavAgentAnimation()
    {
        Vector3 worldDeltaPosition = navMeshAgent.nextPosition - transform.position;

        // Map worldDeltaPosition to local space
        float dx = Vector3.Dot(transform.right, worldDeltaPosition);
        float dy = Vector3.Dot(transform.forward, worldDeltaPosition);
        Vector2 deltaPosition = new Vector2(dx, dy);

        // Low Pass Filter for deltaMove
        float smooth = Mathf.Min(1.0f, Time.deltaTime / 0.15f);
        smoothDeltaPosition = Vector2.Lerp(smoothDeltaPosition, deltaPosition, smooth);

        // Update velocity if time advances
        if (Time.deltaTime > 1e-5f)
            velocity = smoothDeltaPosition / Time.deltaTime;

        bool shouldMove = velocity.magnitude > 0.5f && navMeshAgent.remainingDistance > navMeshAgent.radius;

        // Update animation parameters
        animator.SetBool("move", shouldMove);
        animator.SetFloat("velx", velocity.x);
        animator.SetFloat("vely", velocity.y);
    }
}
