using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine;

public class PubSubListener : MonoBehaviour
{

    private PubSubManager pubSubMan;
    public List<PubSubListenerDelegateRow> Subscriptions;

    // Start is called before the first frame update
    void Start()
    {
        pubSubMan = PubSubManager.Instance;
        Subscribe();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnDestroy() {
        Unsubscribe();
    }

    public void Subscribe() {
        foreach (var sub in Subscriptions) {
            Subscribe(sub.Key);
        }
    }

    public void Subscribe(string key) {
        pubSubMan.Subscribe(key, this);
    }

    public void Subscribe(string key, PubSubListenerUnityEvent e) {
        Subscriptions.Add(new PubSubListenerDelegateRow(key, e));
        Subscribe(key);
    }

    public void Unsubscribe() {
        foreach (var sub in Subscriptions) {
            if (pubSubMan)
            {
                pubSubMan.Unsubscribe(sub.Key, this);
            }
        }
        Subscriptions.Clear();
    }

    public void Unsubscribe(string key) {
        pubSubMan.Unsubscribe(key, this);
        Subscriptions.Remove(Subscriptions.Find((s) => s.Key == key));
    }

    public void Receive(PubSubListenerEvent e) {
        foreach (var sub in Subscriptions) {
            if (sub.Key == e.Key) {
                sub.Delegate.Invoke(e);
            }
        }
    }
}

[Serializable]
public class PubSubListenerUnityEvent: UnityEvent<PubSubListenerEvent> { }

[Serializable]
public class PubSubListenerDelegateRow {
    public string Key;
    public PubSubListenerUnityEvent Delegate;

    public PubSubListenerDelegateRow(string key, PubSubListenerUnityEvent e) {
        this.Key = key;
        this.Delegate = e;
    }
}
