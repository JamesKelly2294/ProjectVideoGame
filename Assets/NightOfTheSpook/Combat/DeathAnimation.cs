using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathAnimation : MonoBehaviour
{

    private float time = 0, totalTime = 5f;
    private Material material;
    private AnimationCurve transparencyCurve;

    void SetLayerRecursively(GameObject obj, int newLayer)
    {
        if (null == obj)
        {
            return;
        }

        obj.layer = newLayer;

        foreach (Transform child in obj.transform)
        {
            if (null == child)
            {
                continue;
            }
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        var wiggleAnimation = GetComponent<WiggleAnimation>();
        if (wiggleAnimation)
        {
            wiggleAnimation.enabled = false;
            Destroy(wiggleAnimation);
        }

        SetLayerRecursively(gameObject, LayerConstants.Dead);

        transparencyCurve = new AnimationCurve(
            new Keyframe(0, 1),
            new Keyframe(0.5f, 1),
            new Keyframe(0.9f, 0.05f),
            new Keyframe(1, 0)
        );

        Rigidbody rigidbody = GetComponent<Rigidbody>();
        rigidbody.freezeRotation = false;
        rigidbody.AddForceAtPosition(Vector3.up * 10, Vector3.forward, ForceMode.Force);

        Vector3 playerMeDelta = GameObject.FindObjectOfType<Player>().gameObject.transform.position - gameObject.transform.position;
        rigidbody.AddForceAtPosition(playerMeDelta.normalized * 10, Vector3.forward, ForceMode.Force);

        // Make the game object transparent.
        material = GetComponentInChildren<Renderer>().material;
        material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
        material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        material.SetInt("_ZWrite", 0);
        material.DisableKeyword("_ALPHATEST_ON");
        material.DisableKeyword("_ALPHABLEND_ON");
        material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
        material.renderQueue = 3000;

        Attackable attackable = GetComponent<Attackable>();
        if (attackable != null) {
            attackable.enabled = false;
        }
        
        Attacker attacker = GetComponent<Attacker>();
        if (attacker != null) {
            attacker.enabled = false;
        }

        PubSubSender sender = GetComponent<PubSubSender>();
        if (sender == null) { sender = gameObject.AddComponent<PubSubSender>(); }
        sender.Publish("i.died", gameObject);

        if (attackable != null && attackable.IsPlayer) {
            sender.Publish("gameOver");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        var color = material.color;
        time += Time.deltaTime;

        if (time > totalTime) {
            GameObject.Destroy(gameObject, 0.1f);
        }

        material.color = new Color (color.r, color.g, color.b, transparencyCurve.Evaluate(time / totalTime));
    }
}
