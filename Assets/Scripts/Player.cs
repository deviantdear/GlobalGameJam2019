using System.Collections;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

[SelectionBase]
public class Player : MonoBehaviour
{
	[SerializeField, Required]
	[Tooltip("The Animator used to animate this player.")]
	private Animator animator;

	[SerializeField, Required]
	[Tooltip("The PlayerMovement used to move this player.")]
	private PlayerMovement movement;

	[Header("Events")]
	public UnityEvent onInitialize;
	public UnityEvent onDie;

	private int deviceId = -1;

	/// <summary>
	/// Gets the animator.
	/// </summary>
	/// <value>The animator.</value>
	public Animator Animator
	{
		get
		{
			return animator;
		}
	}

	/// <summary>
	/// Gets the device identifier for the controlling AirController player, or -1 if there is none.
	/// </summary>
	/// <value>The device identifier.</value>
	public int DeviceId
	{
		get
		{
			return deviceId;
		}
	}

	/// <summary>
	/// Gets a value indicating whether this instance is controlled by an AirController player.
	/// </summary>
	/// <value><c>true</c> if this instance has air controller player; otherwise, <c>false</c>.</value>
	public bool HasAirControllerPlayer
	{
		get
		{
			return DeviceId != -1;
		}
	}

	/// <summary>
	/// Initialize the player controlled by the specified deviceId.
	/// </summary>
	/// <param name="deviceId">Device identifier.</param>
	public void Initialize(int deviceId)
	{
		this.deviceId = deviceId;
		movement.SetOwner(this);
		movement.enabled = true;
		onInitialize.Invoke();
	}

	/// <summary>
	/// Tell this Player to jump off the bed and die (their player disconnected).
	/// </summary>
	public void Die()
	{
		movement.enabled = false;
		onDie.Invoke();
	}
}

