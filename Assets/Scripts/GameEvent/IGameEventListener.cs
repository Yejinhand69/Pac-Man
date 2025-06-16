using UnityEngine;

public interface IGameEventListener
{
    void OnEventRaised(GameEvent evt, Component sender, object[] args);
}