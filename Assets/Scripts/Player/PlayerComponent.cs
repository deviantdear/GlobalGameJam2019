using UnityEngine;
using System.Collections;

public class PlayerComponent : MonoBehaviour
{
	protected Player owner;

	public Player Owner
	{
		get
		{
			return owner;
		}
	}

	public virtual void SetOwner(Player owner)
	{
		this.owner = owner;
	}
}

