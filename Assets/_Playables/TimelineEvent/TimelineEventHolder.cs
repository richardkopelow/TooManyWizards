using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TimelineEventHolder : MonoBehaviour {
    public UnityEvent Event;

    public void InvokeEvent()
    {
        Event.Invoke();
    }
}
