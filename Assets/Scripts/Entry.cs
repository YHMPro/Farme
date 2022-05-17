using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Farme.UI;
public class Entry : MonoBehaviour
{
    private void Awake()
    {
        WindowRoot.GetSingleton().CreateWindow("ControlWindow", RenderMode.ScreenSpaceOverlay, (controlWindow) =>
        {
            controlWindow.CreatePanel<ControlPanel>("UI/ControlWindow/ControlPanel", "ControlPanel", EnumPanelLayer.SYSTEM, (controlPanel) =>
            {

            });
        });







    }
}
