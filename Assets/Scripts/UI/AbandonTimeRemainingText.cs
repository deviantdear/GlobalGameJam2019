using System.Collections;
using NaughtyAttributes;
using UnityEngine;
using Text = TMPro.TMP_Text;

public class AbandonTimeRemainingText : MonoBehaviour 
{
	[SerializeField, Required]
	private Text text;

	[SerializeField]
	private string format = "{0}:{1:00}";

	private void Update()
	{
		if (Toolbox.Game.State == Game.GameState.Abandoning)
		{
			var minutes = Mathf.Floor(Toolbox.Game.AbandonTimeRemaining / 60f);
			var seconds = Mathf.Floor(Toolbox.Game.AbandonTimeRemaining % 60f);
			text.text = string.Format(format, minutes, seconds);	
		}
		else
		{
			text.text = string.Empty;
		}
	}
}
