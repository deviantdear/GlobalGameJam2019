using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlexibleUI : MonoBehaviour {
    public SkinData skinData;

    protected virtual void OnSkinUI()
    {

    }

    public virtual void Awake()
    {
        OnSkinUI();
    }


}
