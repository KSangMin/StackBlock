using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

public abstract class BaseUI : MonoBehaviour
{
    protected abstract UIState GetUIState();

    public virtual void Init()
    {

    }

    public void SetActive(UIState state)
    {
        gameObject.SetActive(GetUIState() == state);
    }
}
