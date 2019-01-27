using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class PlayerSkin : PlayerComponent 
{
	[SerializeField, ReorderableList]
	[Tooltip("The Materials used for each player. If a player does not have a valid DeviceId, the first Material will be used.")]
	private Material[] materials;

	[SerializeField]
	[Tooltip("The Renderers whose Materials are changed.")]
	private Renderer[] renderers;

	public override void SetOwner(Player owner)
	{
		base.SetOwner(owner);

		var hasMaterial = (owner.DeviceId == -1 || owner.DeviceId >= materials.Length);
		var material = materials[hasMaterial ? 0 : owner.DeviceId - 1]; // FIXME: DeviceId looks to be 1-indexed. Is this a consistent behaviour?
		if (!hasMaterial)
		{
			Debug.LogWarningFormat("Owner does not have a valid DeviceId ({0}), using first Material ({1}). Add more Materials to {2} to fix this!", owner.DeviceId, material, name); 
		}
		if(material)
		{
			foreach (var renderer in renderers)
			{
				renderer.material = material;
			}
		}
	}
}
