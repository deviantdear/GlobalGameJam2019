using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using NDream.AirConsole;
using UnityEngine.Events;

public class Weapon : MonoBehaviour 
{
	public enum BarrelMode : byte
	{
		None,
		Cyclic,
		Random,
	}

	[SerializeField]
	[Tooltip("The number of bullets per second.")]
	[MinValue(0f)]
	private float fireRate = 10f;

	[SerializeField]
	[Tooltip("The damage each bullet deals.")]
	[MinValue(0f)]
	private float damage = 1f;

	[SerializeField]
	[Tooltip("The cone bullets are fired in.")]
	[MinValue(0f), MaxValue(180f)]
	private float accuracy = 5f;

	[SerializeField, Required]
	[Tooltip("The bullet prefab fired by the gun.")]
	private Projectile projectilePrefab;

	[SerializeField]
	[Tooltip("The speed of the projectile in m/s.")]
	[MinValue(0f)]
	private float muzzleVelocity = 10f;

	[SerializeField, Required]
	[Tooltip("The object that determines the direction of the bullets.")]
	private Transform direction;

	[SerializeField]
	[Tooltip("The barrel selection method.")]
	private BarrelMode barrelMode = BarrelMode.None;

	[SerializeField, ReorderableList]
	[Tooltip("The point where bullets are spawned.")]
	private WeaponBarrel[] barrels;

	[Header("Events")]
	public UnityEvent onPullTrigger = new UnityEvent();
	public UnityEvent onReleaseTrigger = new UnityEvent();
	public UnityEvent onShoot = new UnityEvent();

	private bool firing = false;
	private float nextShotTime;
	private int barrelIndex = 0;

	public void PullTrigger()
	{
		if (!firing)
		{
			firing = true;
			onPullTrigger.Invoke();
		}
	}

	public void ReleaseTrigger()
	{
		if (firing)
		{
			firing = false;
			onReleaseTrigger.Invoke();
		}
	}

	private void Update()
	{
		if (firing && Time.realtimeSinceStartup > nextShotTime)
		{
			var barrel = barrels[barrelIndex];
			var projectile = Instantiate<Projectile>(projectilePrefab, barrel.transform.position, direction.rotation * Quaternion.Euler(Random.onUnitSphere * accuracy));
			projectile.Initialize(damage);
			projectile.Rigidbody.AddForce(projectile.transform.forward * muzzleVelocity, ForceMode.Impulse);

			// Raise events
			onShoot.Invoke();
			barrel.Shoot();

			// Cycle barrel
			switch (barrelMode)
			{
				case BarrelMode.Cyclic:
					barrelIndex = barrels.Length == 0 ? 0 : Mathf.RoundToInt(Mathf.Repeat(barrelIndex + 1, barrels.Length));
					break;
				case BarrelMode.Random:
					barrelIndex = Random.Range(0, barrels.Length);
					break;
			}

			nextShotTime = Time.realtimeSinceStartup + (1f / fireRate);
		}
	}
}
