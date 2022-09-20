using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public World world;

    public string resourceName;
    private Resource _resource;

    [Header("Floating Text")]
    public GameObject floatingTextPrefab;
    public Vector3 floatingTextOffset;

    [Header("User Interaction")]
    [Range(0.5f, 2.0f)]
    public float clickedScaleFactor;
    [Range(0.5f, 2.0f)]
    public float highlightedScaleFactor;
    private Vector3 _startingScale;

    private bool _highlighted;
    private bool _clicked;

    private void OnMouseEnter()
    {
        _highlighted = true;

        SetScale();
    }

    private void OnMouseExit()
    {
        _highlighted = false;

        SetScale();
    }

    private void OnMouseDown()
    {
        _clicked = true;
        var go = Instantiate(floatingTextPrefab);
        go.transform.position = transform.position + floatingTextOffset;
        var floatingText = go.GetComponent<MoveAndFadeText>();
        floatingText.text.text = string.Format("+{0} {1}", _resource.ProductionPerClick, _resource.name);
        floatingText.destroyOnCompletion = true;
        floatingText.TriggerAnimation();

        _resource.ApplyClickProduction();

        SetScale();
    }

    private void OnMouseUp()
    {
        _clicked = false;

        SetScale();
    }

    private void SetScale()
    {
        if (_clicked)
        {
            transform.localScale = _startingScale * clickedScaleFactor;

        }
        else if (_highlighted)
        {
            transform.localScale = _startingScale * highlightedScaleFactor;
        }
        else
        {
            transform.localScale = _startingScale;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (world == null)
        {
            world = FindObjectOfType<World>();
        }

        if (world == null)
        {
            Debug.LogError("Unable to find World instance. Destroying self.");
            Destroy(this);
        }

        _startingScale = transform.localScale;
        _resource = world.resourceStore.GetResource(resourceName);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
