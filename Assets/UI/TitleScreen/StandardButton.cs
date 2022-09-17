using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(PubSubSender))]
public class StandardButton : Button, IPointerEnterHandler, IPointerExitHandler
{
    private PubSubSender _sender;

    // Start is called before the first frame update
    protected override void Start()
    {
        _sender = GetComponent<PubSubSender>();
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);

        _sender.Publish("button.highlight.begin");
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);

        _sender.Publish("button.highlight.end");
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);

        _sender.Publish("button.press.begin");
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);

        _sender.Publish("button.press.end");
    }
}