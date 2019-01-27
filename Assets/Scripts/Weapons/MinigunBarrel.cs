using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigunBarrel : MonoBehaviour 
{
	[SerializeField]
	[Tooltip("The number of revolutions per second.")]
	private float maxSpinRate = 5f;

	[SerializeField]
	[Tooltip("The rate the revolutions per second increases when spinning.")]
	private float spinAcceleration = 5f;

	[SerializeField]
	[Tooltip("The rate the revolutions per second decreases when not spinning.")]
	private float spinDecceleration = 5f;

	[SerializeField]
	[Tooltip("The axis the object spins around.")]
	private Vector3 axis = Vector3.forward;

	private bool spinning;
	private float spinRate = 0f;

	public void SetSpin(bool spinning)
	{
		this.spinning = spinning;
	}

	private void Update()
	{
		if (spinning)
		{
			spinRate = Mathf.MoveTowards(spinRate, maxSpinRate, spinAcceleration * Time.deltaTime);
		}
		else
		{
			spinRate = Mathf.MoveTowards(spinRate, 0f, spinDecceleration * Time.deltaTime);
		}
		transform.Rotate(axis.normalized, spinRate * 360f * Time.deltaTime, Space.Self);
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.blue;
		Gizmos.DrawRay(transform.position, transform.TransformDirection(axis));
	}
}
