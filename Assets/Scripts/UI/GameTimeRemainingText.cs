using System.Collections;
using NaughtyAttributes;
using UnityEngine;
using Text = TMPro.TMP_Text;

public class GameTimeRemainingText : MonoBehaviour 
{
	[SerializeField, Required]
	private Text text;

	[SerializeField]
	private string format = "{0}:{1:00}";

	private void Update()
	{
		var minutes = Mathf.Max(0f, Mathf.Floor(Toolbox.Game.GameTimeRemaining / 60f));
		var seconds = Mathf.Max(0f, Mathf.Floor(Toolbox.Game.GameTimeRemaining % 60f));
		text.text = string.Format(format, minutes, seconds);
	}
}
