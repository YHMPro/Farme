using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Farme.UI;
using UnityEngine.UI;
using Farme.Extend;
using UnityEngine.EventSystems;
public class PanelTest : BasePanel
{
    protected override void Awake()
    {
        base.Awake();
        RegisterComponentsTypes<Image>();


        if(GetComponent("Image",out Image img))
        {
            img.UIEventRegistered(EventTriggerType.PointerClick, ImageClick);
        }
    }


    protected override void Start()
    {
        base.Start();
    }

    private void ImageClick(BaseEventData bEData)
    {
        if(relyWindow.Raycast(out List<RaycastResult> resultLi))
        {
            foreach(var result in resultLi)
            {
                Debug.Log(result.gameObject);
            }
        }
    }
}
