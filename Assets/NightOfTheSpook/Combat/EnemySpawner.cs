using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject Prefab;

    [Range(0.001f, 100.0f)]
    public float CoolDownTime;

    private float _nextSpawnTime = 0.0f;

    // HACK: true on start to prevent onscreen spawners from doing things
    // on start before the visibility check has occured.
    private bool _isVisible = true;

    private Attackable _primaryTarget;

    // Start is called before the first frame update
    void Start()
    {
        // TODO: look for the player camera if it isn't set.
        // TODO: do something/bail if the Prefab game object isn't set.
        _primaryTarget = GameObject.FindGameObjectWithTag("Player").GetComponent<Attackable>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isVisible && _nextSpawnTime < Time.time)
        {
            Debug.Log($"{name} Spawning");
            var newEnemy = Instantiate(Prefab, transform.position, Quaternion.identity);
            newEnemy.SetActive(true);
            _nextSpawnTime = Time.time + CoolDownTime;

            newEnemy.GetComponent<Attacker>().PrimaryTarget = _primaryTarget;
        }
    }

    void OnBecameVisible()
    {
        Debug.Log($"{name} Howdy");
        _isVisible = true;
    }

    void OnBecameInvisible()
    {
        Debug.Log($"{name} Goodbye :<");
        _isVisible = false;
    }
}
