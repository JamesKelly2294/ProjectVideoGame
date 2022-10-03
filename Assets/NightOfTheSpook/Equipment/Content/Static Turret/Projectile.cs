using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class Projectile : MonoBehaviour
{
    [Range(0, 10.0f)]
    public float speed = 4.0f;

    [Range(0, 10.0f)]
    public float lifetime = 5.0f;

    [HideInInspector]
    public float damage;

    private float _startTime;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody>().AddForce(transform.forward * speed, ForceMode.Impulse);

        _startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > _startTime + lifetime)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != LayerConstants.Enemy)
        {
            return;
        }

        var attackable = other.GetComponent<Attackable>();
        if (attackable == null)
        {
            return;
        }

        attackable.InflictDamage(damage, null);
        Destroy(gameObject);
    }
}
