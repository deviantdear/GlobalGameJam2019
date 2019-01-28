using System.Collections;
using NaughtyAttributes;
using UnityEngine;
using Text = TMPro.TMP_Text;

public class AirConsoleRoomIdText : MonoBehaviour 
{
	[SerializeField, Required]
	private Text text;

	[SerializeField]
	private string format = "Join {0}";

	private void Update()
	{
		text.text = Toolbox.Game.RoomCode.Equals("0") ? string.Empty : string.Format(format, Toolbox.Game.RoomCode);
	}
}
