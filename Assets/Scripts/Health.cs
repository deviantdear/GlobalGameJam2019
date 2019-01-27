using UnityEngine;
using System.Collections;
using NaughtyAttributes;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
	[SerializeField]
	[Tooltip("The max amount of health that can be had.")]
	[MinValue(0f)]
	private float maxHealth = 2f;

	[Header("Events")]
	public DamageEvent onDamage = new DamageEvent();
	public UnityEvent onDie = new UnityEvent();

	private float health;

	public bool IsDead
	{
		get
		{
			return health <= 0f;
		}
	}

	private void Awake()
	{
		health = maxHealth;
	}

	public void Damage(float damage)
	{
		if (IsDead)
			return;

		// Apply damage
		var appliedDamage = Mathf.Min(health, damage);
		health -= appliedDamage;
		onDamage.Invoke(damage);

		// Die if this killed us
		if (IsDead)
		{
			onDie.Invoke();
		}
	}

	[System.Serializable]
	public class DamageEvent : UnityEvent<float> { }
}

