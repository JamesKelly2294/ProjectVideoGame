using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PubSubListener))]
public class GooSplatter : MonoBehaviour
{
    [Range(0, 60.0f)]
    public float Lifetime = 30.0f;

    [Range(0, 1.0f)]
    public float BaseSlowModifier = 0.75f;

    public EquipmentConfiguration Configuration;

    private float _elapsedTime;

    private MovementModifier _movementModifier;

    private UpgradeManager _upgradeManager;

    private Vector3 _startingScale;

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

        var spookyGameManager = FindObjectOfType<SpookyGameManager>();
        _upgradeManager = spookyGameManager.GetComponent<UpgradeManager>();

        _startingScale = transform.localScale;

        RecalculateUpgrades();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_initialized) { return; }
        _elapsedTime += Time.deltaTime;

        if (_elapsedTime > _trueLifetime)
        {
            var animation = gameObject.AddComponent<EquipmentDestructionAnimation>();
            animation.scaleCurve = AnimationCurve.EaseInOut(0.0f, 1.0f, 1.0f, 0.0f);
            animation.duration = 0.5f;
            Destroy(this);
            return;
        }
    }

    private Vector3 _trueScale = Vector3.one;
    private float _truePower = 1.0f;
    private float _trueLifetime = 1.0f;
    public void RecalculateUpgrades()
    {
        RecalculateTrueLifetime();
        RecalculateTruePower();
        RecalculateTrueScale();
    }

    private void RecalculateTrueLifetime()
    {
        var upgrade = _upgradeManager.UpgradeOrZero(Configuration);
        var multiplier = Mathf.Pow(1.5f, upgrade.lifetime);

        _trueLifetime = Lifetime * multiplier;
    }

    private void RecalculateTruePower()
    {
        var upgrade = _upgradeManager.UpgradeOrZero(Configuration);
        var multiplier = Mathf.Pow(0.8f, upgrade.power);

        _truePower = BaseSlowModifier * multiplier;
    }

    private void RecalculateTrueScale()
    {
        var upgrade = _upgradeManager.UpgradeOrZero(Configuration);
        var multiplier = Mathf.Pow(1.5f, upgrade.special);

        _trueScale = _startingScale * multiplier;
        transform.localScale = _trueScale;
    }

    private void CalculateMovementModifier()
    {
        _movementModifier = new MovementModifier();
        _movementModifier.type = MovementModifier.Type.Goo;
        _movementModifier.multiplier = () => {
            return _truePower;
        };
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
