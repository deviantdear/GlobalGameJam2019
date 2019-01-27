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

	[SerializeField]
	[Tooltip("The amount of health regenerated per second.")]
	[MinValue(0f)]
	private float healthRegen = 0f;

	[SerializeField]
	[Tooltip("The amount of time before health regen can start.")]
	[MinValue(0f)]
	private float healthRegenDelay = 1f;

	[SerializeField]
	[Tooltip("Can this object be healed?")]
	private bool isHealable = true;

	[SerializeField]
	[Tooltip("Can this object be damaged?")]
	private bool isDamageable = true;

	[Header("Events")]
	public HealthChangeEvent onDamage = new HealthChangeEvent();
	public HealthChangeEvent onHeal = new HealthChangeEvent();
	public UnityEvent onDie = new UnityEvent();

	private float health;
	private float healingTime;

	public bool IsDead
	{
		get
		{
			return health <= 0f;
		}
	}

	public bool IsDamageable
	{
		get
		{
			return isDamageable;
		}
		set
		{
			isDamageable = value;
		}
	}

	private void Awake()
	{
		health = maxHealth;
	}

	private void Update()
	{
		if (Time.realtimeSinceStartup > healingTime)
		{
			Heal(healthRegen * Time.deltaTime);
		}
	}

	public void Damage(float damage)
	{
		if (IsDead || !isDamageable)
			return;

		// Apply damage
		var appliedDamage = Mathf.Min(health, damage);
		health -= appliedDamage;
		healingTime = Time.realtimeSinceStartup + healthRegenDelay;
		onDamage.Invoke(damage);

		// Die if this killed us
		if (IsDead)
		{
			onDie.Invoke();
		}
	}

	public void Heal(float healing)
	{
		if (IsDead || !isHealable)
			return;

		// Apply healing
		var appliedHealing = Mathf.Min(healing, maxHealth - health);
		health += appliedHealing;
		onHeal.Invoke(appliedHealing);
	}

	[System.Serializable]
	public class HealthChangeEvent : UnityEvent<float> { }
}

