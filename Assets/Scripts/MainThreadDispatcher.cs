using System;
using UnityEngine;
using System.Collections.Generic;

public class MainThreadDispatcher : MonoBehaviour
{
    private static MainThreadDispatcher instance;

    public static MainThreadDispatcher Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameObject("MainThreadDispatcher").AddComponent<MainThreadDispatcher>();
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ExecuteOnMainThread(Action action)
    {
        lock (queue)
        {
            queue.Enqueue(action);
        }
    }

    private readonly Queue<Action> queue = new Queue<Action>();

    private void Update()
    {
        lock (queue)
        {
            while (queue.Count > 0)
            {
                Action action = queue.Dequeue();
                action.Invoke();
            }
        }
    }
}
