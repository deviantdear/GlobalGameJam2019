using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using NaughtyAttributes;

public class HealthState : MonoBehaviour
{
	[SerializeField, Required]
	private Health target;

	[SerializeField]
	private bool usePercent = false;

	[SerializeField, ShowIf("usePercent", true)]
	[MinValue(0f)]
	private float minHealth = 0f;

	[SerializeField, ShowIf("usePercent", true)]
	[MinValue(0f)]
	private float maxHealth = 5f;

	[SerializeField, ShowIf("usePercent")]
	[Slider(0f, 1f)]
	private float minHealthPercent = 0.5f;

	[SerializeField, ShowIf("usePercent")]
	[Slider(0f, 1f)]
	private float maxHealthPercent = 1f;

	[Header("Events")]
	public UnityEvent onEnterState;
	public UnityEvent onExitState;

	private bool inState = false;

	private void OnValidate()
	{
		maxHealth = Mathf.Max(maxHealth, minHealth);
		maxHealthPercent = Mathf.Max(maxHealthPercent, minHealthPercent);
	}

	private void OnEnable()
	{
		target.onDamage.AddListener(HandleHealthChange);
		target.onHeal.AddListener(HandleHealthChange);
		HandleHealthChange(0f);
	}

	private void OnDisable()
	{
		if (target)
		{
			target.onDamage.RemoveListener(HandleHealthChange);
			target.onHeal.RemoveListener(HandleHealthChange);
		}
	}

	private void HandleHealthChange (float change)
	{
		if (!target)
		{
			return;
		}
		var nowInState = usePercent ? (target.CurrentHealthPercent <= maxHealthPercent && target.CurrentHealthPercent > minHealthPercent) : (target.CurrentHealth <= maxHealth && target.CurrentHealth > minHealth);
		if (nowInState)
		{
			if (!inState)
			{
				inState = true;
				onEnterState.Invoke();
			}
		}
		else
		{
			if (inState)
			{
				inState = false;
				onExitState.Invoke();
			}
		}
	}
}

