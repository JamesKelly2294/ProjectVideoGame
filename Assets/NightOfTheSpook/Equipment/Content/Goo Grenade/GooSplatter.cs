using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GooSplatter : MonoBehaviour
{
    [Range(0, 60.0f)]
    public float Lifetime = 30.0f;

    [Range(0, 1.0f)]
    public float BaseSlowModifier = 0.75f;

    private float _elapsedTime;

    private MovementModifier _movementModifier;

    // Start is called before the first frame update
    void Start()
    {
        var equipmentAppearAnimation = GetComponent<EquipmentAppearAnimation>();
        if (equipmentAppearAnimation)
        {
            equipmentAppearAnimation.equipmentAppearedHandler = EquipmentAppearedHandler;
        }
        else
        {
            Setup();
        }
    }

    void EquipmentAppearedHandler(EquipmentAppearAnimation animation)
    {
        Setup();
    }

    private bool _initialized = false;
    void Setup()
    {
        _initialized = true;
        _elapsedTime = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_initialized) { return; }
        _elapsedTime += Time.deltaTime;

        if (_elapsedTime > Lifetime)
        {
            var animation = gameObject.AddComponent<EquipmentDestructionAnimation>();
            animation.scaleCurve = AnimationCurve.EaseInOut(0.0f, 1.0f, 1.0f, 0.0f);
            animation.duration = 0.5f;
            Destroy(this);
            return;
        }
    }

    private float SlowModifierValue
    {
        get
        {
            return BaseSlowModifier;
        }
    }

    private void CalculateMovementModifier()
    {
        _movementModifier = new MovementModifier();
        _movementModifier.type = MovementModifier.Type.Goo;
        _movementModifier.multiplier = SlowModifierValue;
        _movementModifier.source = this;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != LayerConstants.Enemy)
        {
            return;
        }

        var attacker = other.GetComponent<Attacker>();
        if (attacker == null)
        {
            return;
        }

        CalculateMovementModifier();
        Debug.Log("Applied movement modifier with source " + _movementModifier.source);
        attacker.ApplyMovementModifier(_movementModifier);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != LayerConstants.Enemy)
        {
            return;
        }

        var attacker = other.GetComponent<Attacker>();
        if (attacker == null)
        {
            return;
        }
        attacker.RemoveMovementModifier(_movementModifier);
    }
}
