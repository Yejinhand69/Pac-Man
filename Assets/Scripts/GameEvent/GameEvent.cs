using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "GameEvent", menuName = "GameEvent")]
public class GameEvent : ScriptableObject
{
    private readonly List<IGameEventListener> listeners = new();

    public void Raise(Component sender = null, object[] args = null)
    {
        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            listeners[i].OnEventRaised(this, sender, args);
        }
    }

    public void RegisterListener(IGameEventListener listener)
    {
        if (!listeners.Contains(listener)) listeners.Add(listener);
    }

    public void UnregisterListener(IGameEventListener listener)
    {
        if (listeners.Contains(listener)) listeners.Remove(listener);
    }
}