using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PubSubSender : MonoBehaviour
{

    private PubSubManager pubSubMan;

    // Start is called before the first frame update
    void Awake()
    {
        pubSubMan = PubSubManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Publish(string key) {
        pubSubMan.Publish(key, gameObject, this);
    }

    public void Publish(string key, object value) {
        pubSubMan.Publish(key, gameObject, value);
    }
}
