using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WiggleAnimation : MonoBehaviour
{
    public AnimationCurve walkVertical;
    public AnimationCurve walkWobble;
    public float walkVerticalTime, walkVerticalTotalTime;
    private GameObject _inner;
    private Vector3 _oldPosition;

    private Attacker _attacker;

    // Start is called before the first frame update
    void Start()
    {
        var innerTransform = transform.Find("Inner");
        if (innerTransform != null)
        {
            _inner = innerTransform.gameObject;
        }
        else
        {
            Destroy(this);
        }

        _attacker = GetComponent<Attacker>();
        _oldPosition = transform.position;

        walkVerticalTime = Random.Range(0, walkVerticalTotalTime);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Wiggle();
    }

    private void Wiggle()
    {

        Vector3 movement = transform.position - _oldPosition;
        movement = new Vector3(movement.x, 0, movement.z);
        _oldPosition = transform.position;

        // Animate walk
        if (movement != Vector3.zero)
        {
            walkVerticalTime += Time.deltaTime;
            if (walkVerticalTime > walkVerticalTotalTime)
            {
                walkVerticalTime -= walkVerticalTotalTime;
            }
        }
        else
        {
            // If we are beyond half way, just continue
            if (walkVerticalTime > walkVerticalTotalTime / 2 && walkVerticalTime < walkVerticalTotalTime)
            {
                walkVerticalTime += Time.deltaTime;
                walkVerticalTime = Mathf.Min(walkVerticalTotalTime, walkVerticalTime);
            }
            else if (walkVerticalTime < walkVerticalTotalTime / 2 && walkVerticalTime > 0)
            {
                walkVerticalTime -= Time.deltaTime;
                walkVerticalTime = Mathf.Max(0, walkVerticalTime);
            }
        }
        float innerVerticalOffet = walkVertical.Evaluate(walkVerticalTime / walkVerticalTotalTime);
        _inner.transform.position = new Vector3(_inner.transform.position.x, innerVerticalOffet, _inner.transform.position.z);

        if (_attacker && _attacker.PrimaryTarget != null)
        {
            var targetPos = _attacker.PrimaryTarget.transform.position;
            _inner.transform.LookAt(new Vector3(targetPos.x, _inner.transform.position.y, targetPos.z));
        }
        else
        {
            _inner.transform.LookAt(_inner.transform.position + movement);
        }
        Vector3 eulerRotation = _inner.transform.rotation.eulerAngles;
        _inner.transform.rotation = Quaternion.Euler(new Vector3(eulerRotation.x, eulerRotation.y, walkWobble.Evaluate(walkVerticalTime / walkVerticalTotalTime) * 90));

    }
}
