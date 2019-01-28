using UnityEngine;
using System.Collections;
using NaughtyAttributes;
using UnityEngine.Events;

public class Kid : MonoBehaviour
{
	[SerializeField, Required]
	[Tooltip("The kid's health.")]
	private Health health;

	[SerializeField, Required]
	[Tooltip("The kid's Animator.")]
	private Animator animator;

	[Header("Events")]
	public UnityEvent onSleep = new UnityEvent();
	public UnityEvent onWakeUpHappy = new UnityEvent();
	public UnityEvent onWakeUpSad = new UnityEvent();
	public UnityEvent onWakeUp = new UnityEvent();

	public Health Health
	{
		get
		{
			return health;
		}
	}

	[Button("Find Health")]
	private void FindHealth()
	{
		var foundHealth = GetComponentInChildren<Health>();
		if (foundHealth != health)
		{
			health = foundHealth;
			#if UNITY_EDITOR
			UnityEditor.EditorUtility.SetDirty(this);
			#endif
		}
	}

	[Button("Find Animator")]
	private void FindAnimator()
	{
		var foundAnimator = GetComponentInChildren<Animator>();
		if (foundAnimator != animator)
		{
			animator = foundAnimator;
			#if UNITY_EDITOR
			UnityEditor.EditorUtility.SetDirty(this);
			#endif
		}
	}

	private void Awake()
	{
		health.onDamage.AddListener(HandleHealthChange);
		health.onHeal.AddListener(HandleHealthChange);
		health.onDie.AddListener(HandleDie);
	}

	private void HandleHealthChange(float change)
	{
		animator.SetFloat("Health Percent", health.CurrentHealthPercent);		
	}

	private void HandleDie()
	{
		var game = FindObjectOfType<Game>();
		if (game)
		{
			game.LoseGame();
		}
	}

	public void Sleep()
	{
		onSleep.Invoke();
	}

	public void WakeUp(bool happy)
	{
		if (happy)
		{
			onWakeUpHappy.Invoke();
		}
		else
		{
			onWakeUpSad.Invoke();
		}
		onWakeUp.Invoke();
	}
}

