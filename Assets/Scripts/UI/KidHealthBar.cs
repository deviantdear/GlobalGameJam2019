using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;

public class KidHealthBar : MonoBehaviour 
{
	[SerializeField, Required]
	private Slider healthBar;

	private void Update()
	{
		healthBar.normalizedValue = Toolbox.Game.Kid.Health.CurrentHealthPercent;
	}
}
