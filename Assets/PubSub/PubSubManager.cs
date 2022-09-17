using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PubSubManager : MonoBehaviour
{
    private static PubSubManager _instance;
    public static PubSubManager Instance
    {
        get
        {
            if (!_instance)
            {
                var go = new GameObject();
                go.transform.name = "Pub-Sub Manager";
                _instance = go.AddComponent<PubSubManager>();
            }
            return _instance;
        }
    }
    public Dictionary<string, HashSet<PubSubListener>> listeners = new Dictionary<string, HashSet<PubSubListener>>();

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this);
            return;
        }

        _instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Publish(string key, GameObject sender, object value) {
        PubSubListenerEvent e = new PubSubListenerEvent(key, sender, value);
        if (listeners.ContainsKey(key)) {
            HashSet<PubSubListener> pubSubListeners = listeners[key];
            foreach (PubSubListener listener in pubSubListeners) {
                listener.Receive(e);
            }
        }
    }

    public void Subscribe(string key, PubSubListener listener) {
        if (!listeners.ContainsKey(key)) {
            listeners[key] = new HashSet<PubSubListener>();
        }

        listeners[key].Add(listener);
    }

    public void Unsubscribe(string key, PubSubListener listener) {
        if (!listeners.ContainsKey(key)) {
            return;
        }

        listeners[key].Remove(listener);
    }
}


public class PubSubListenerEvent {
    public string Key;
    public GameObject sender;
    public object value;

    public PubSubListenerEvent(string key, GameObject sender, object value) {
        this.Key = key;
        this.sender = sender;
        this.value = value;
    }
}