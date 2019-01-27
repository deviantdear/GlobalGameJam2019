using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
	[SerializeField]
	private LayerMask damageMask = (LayerMask)(-1);

	[SerializeField]
	private GameObject[] hitEffects;

	private Rigidbody rigidbody;
	private float damage;

	public Rigidbody Rigidbody
	{
		get
		{
			return rigidbody;
		}
	}

	private void Awake()
	{
		rigidbody = GetComponent<Rigidbody>();
	}

	public void Initialize(float damage)
	{
		this.damage = damage;
	}

	private void OnCollisionEnter(Collision collision)
	{
		HandleCollision(collision.gameObject);
	}

	private void OnTriggerEnter(Collider other)
	{
		HandleCollision(other.gameObject);
	}

	private void HandleCollision(GameObject other)
	{
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
				health.Damage(damage);
			}
		}

		// Spawn hit effect
		if (hitEffects.Length > 0)
		{
			var hitEffect = hitEffects[Random.Range(0, hitEffects.Length)];
			Instantiate(hitEffect, transform.position, Quaternion.LookRotation(transform.forward));
		}

		// Despawn
		Destroy(gameObject);
	}
}

