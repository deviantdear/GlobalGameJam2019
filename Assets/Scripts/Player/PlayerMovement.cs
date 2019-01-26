using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using NDream.AirConsole;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class PlayerMovement : PlayerComponent
{
	const float InputThreshold = 0.1f;

	[SerializeField, Required]
	[Tooltip("The CharacterController used to move this player.")]
	private CharacterController characterController;

	[SerializeField]
	[Tooltip("The speed of the player.")]
	[MinValue(0f)]
	private float moveSpeed = 2.5f;

	[SerializeField]
	[Tooltip("The rate at which the player can achieve their move speed.")]
	[MinValue(0f)]
	private float acceleration = 5f;

	[SerializeField]
	[Tooltip("The turn speed of the player.")]
	[MinValue(0f)]
	private float turnSpeed = 180f;

	private Vector3 velocity = Vector3.zero;
	private Vector3 desiredForward;

	private void Awake()
	{
		desiredForward = transform.forward;
	}

	private void Update()
	{
		if (owner.HasAirConsolePlayer && Toolbox.Input.HasController(owner.DeviceId))
		{
			// AirConsole movement
			var controller = Toolbox.Input.GetController(owner.DeviceId);
			Move(controller.Move.X, controller.Move.Y, controller.Aim.X, controller.Aim.Y);
		}
		#if UNITY_EDITOR
		else
		{
			// Editor movement
			Move(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), Input.GetAxis("Aim X"), Input.GetAxis("Aim Y"));
		}
		#endif
	}

	// From Skyfall: Judgement Day
	private void Move(float moveX, float moveY, float aimX, float aimY)
	{
		// Get input
		var moveInput = new Vector3(moveX, 0f, moveY);
		moveInput = Vector3.ClampMagnitude(moveInput, 1f);

		// Update velocity
		var movementVelocity = Vector3.ProjectOnPlane(velocity, Vector3.up);
		var desiredMovementVelocity = moveInput * moveSpeed;
		var newMovementVelocity = Vector3.MoveTowards(movementVelocity, desiredMovementVelocity, acceleration * Time.deltaTime);
		velocity = newMovementVelocity + Vector3.up * velocity.y; // reset x/z, preserve y

//		// Jump
//		if (Input.GetButtonDown("Jump") && characterController.isGrounded)
//		{
//			velocity.y = jumpVelocity;
//			animator.SetTrigger("Jump");
//		}

		// Gravity (if airborne)
		if (!characterController.isGrounded)
		{
			velocity += Physics.gravity * Time.deltaTime;
		}

		// Move
		characterController.Move(velocity * Time.deltaTime);

		// Rotate to face velocity or second stick
		var aimInput = new Vector3(aimX, 0, aimY);
		aimInput = Vector3.ClampMagnitude(aimInput, 1f);

		var newDesiredForward = aimInput;
		if (newDesiredForward.magnitude < InputThreshold)
		{
			newDesiredForward = moveInput;
		}
		if (newDesiredForward.magnitude >= InputThreshold)
		{
			desiredForward = newDesiredForward;
		}
		transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(desiredForward), Time.deltaTime * turnSpeed);

		// Apply animations
		var localVelocity = transform.InverseTransformDirection(velocity); // TODO: Limit speed to range
		owner.Animator.SetFloat("Move X", localVelocity.x);
		owner.Animator.SetFloat("Move Y", localVelocity.z);
	}
}
