using System.Collections;
using NaughtyAttributes;
using UnityEngine;
using Text = TMPro.TMP_Text;

public class NewGameTimeRemainingText : MonoBehaviour 
{
	[SerializeField, Required]
	private Text text;

	[SerializeField, TextArea]
	private string format = "Game Over\n{0}:{1:00}";

	private void Update()
	{
		if (Toolbox.Game.State == Game.GameState.Completed)
		{
			var minutes = Mathf.Floor(Toolbox.Game.NewGameTimeRemaining / 60f);
			var seconds = Mathf.Floor(Toolbox.Game.NewGameTimeRemaining % 60f);
			text.text = string.Format(format, minutes, seconds);	
		}
		else
		{
			text.text = string.Empty;
		}
	}
}
