using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// トランジション
/// </summary>
public class Transition : MonoBehaviour
{
    public virtual void Begin(UnityAction onComplete = null)
    {
        onComplete?.Invoke();
    }
}
