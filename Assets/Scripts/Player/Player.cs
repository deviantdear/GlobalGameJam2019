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

	[SerializeField]
	[Tooltip("The Weapon used by the this player.")]
	private Weapon weapon;

	[SerializeField]
	[Tooltip("The skins affecting this player.")]
	private PlayerSkin[] skins;

	[Header("Events")]
	public UnityEvent onInitialize = new UnityEvent();
	public UnityEvent onReady = new UnityEvent();
	public UnityEvent onUnready = new UnityEvent();
	public UnityEvent onDie = new UnityEvent();

	private bool isReady = false;
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
	/// Gets a value indicating whether this instance is ready.
	/// </summary>
	/// <value><c>true</c> if this instance is ready; otherwise, <c>false</c>.</value>
	public bool IsReady
	{
		get
		{
			return isReady;
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
	public bool HasAirConsolePlayer
	{
		get
		{
			return DeviceId != -1;
		}
	}

	[Button("Find Movement Component")]
	private void FindMovementComponent()
	{
		var foundMovement = GetComponentInChildren<PlayerMovement>();
		if (foundMovement != movement)
		{
			movement = foundMovement;
			#if UNITY_EDITOR
			UnityEditor.EditorUtility.SetDirty(this);
			#endif
		}
	}

	[Button("Find Skin Components")]
	private void FindSkinComponents()
	{
		skins = GetComponentsInChildren<PlayerSkin>();
		#if UNITY_EDITOR
		UnityEditor.EditorUtility.SetDirty(this);
		#endif
	}

	/// <summary>
	/// Initialize the player controlled by the specified deviceId.
	/// </summary>
	/// <param name="deviceId">Device identifier.</param>
	public void Initialize(int deviceId, bool autoReady=false)
	{
		this.deviceId = deviceId;
		movement.SetOwner(this);
		foreach (var skin in skins)
		{
			skin.SetOwner(this);
		}
		onInitialize.Invoke();
		if(autoReady)
		{
			Ready();
		}
	}

	public void Ready()
	{
		isReady = true;
		onReady.Invoke();
	}

	public void Unready()
	{
		isReady = false;
		onUnready.Invoke();
	}

	/// <summary>
	/// Tell this Player to jump off the bed and die (their player disconnected).
	/// </summary>
	public void Die()
	{
		isReady = false;
		onDie.Invoke();
	}

	private void Update()
	{
		if (isReady)
		{
			if (HasAirConsolePlayer && Toolbox.Input.HasController(DeviceId))
			{
				// AirConsole controls
				var controller = Toolbox.Input.GetController(DeviceId);
				if (controller.Aim.Pressed)
				{
					weapon.PullTrigger();
				}
				else
				{
					weapon.ReleaseTrigger();
				}
			}
			#if UNITY_EDITOR
			else
			{
				// Editor controls
				if (Input.GetButton("Shoot"))
				{
					weapon.PullTrigger();
				}
				else
				{
					weapon.ReleaseTrigger();
				}
			}
			#endif
		}
	}
}

