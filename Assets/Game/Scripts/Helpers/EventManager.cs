using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class EventManager
{
    #region FlipFlop
    public static event UnityAction ObjectRotate;
    public static void OnObjectRotate() => ObjectRotate?.Invoke();
    #endregion
}
