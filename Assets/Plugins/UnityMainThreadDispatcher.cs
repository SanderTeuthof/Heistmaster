using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityMainThreadDispatcher : MonoBehaviour
{
    private static UnityMainThreadDispatcher _instance;
    private Queue<Action> _actionQueue = new Queue<Action>();

    public static UnityMainThreadDispatcher Instance()
    {
        if (_instance == null)
        {
            var go = new GameObject("UnityMainThreadDispatcher");
            _instance = go.AddComponent<UnityMainThreadDispatcher>();
        }
        return _instance;
    }

    void Update()
    {
        while (_actionQueue.Count > 0)
        {
            _actionQueue.Dequeue().Invoke();
        }
    }

    public void Enqueue(Action action)
    {
        _actionQueue.Enqueue(action);
    }
}
