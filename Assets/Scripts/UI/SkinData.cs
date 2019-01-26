using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Skin Data")]
public class SkinData : ScriptableObject
{
    public Sprite buttonSprite;
    public SpriteState buttonSpriteState;

    public Color defaultColor;
    public Color confirmColor;
    public Color declineColor;
    public Color warningColor;

}
