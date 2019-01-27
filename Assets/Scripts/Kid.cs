using UnityEngine;
using System.Collections;

public class Kid : MonoBehaviour
{
	// TODO: Health?
	// TODO: Animations?

	[ContextMenu("Kill")]
	public void Kill()
	{
		FindObjectOfType<Game>().LoseGame();
	}
}

