using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GlobalEventManager : MonoBehaviour
{
    public static UnityEvent unityEvent = new UnityEvent();

    public static void SendUnityEvent() 
    {
        unityEvent.Invoke();
    }
    
}
