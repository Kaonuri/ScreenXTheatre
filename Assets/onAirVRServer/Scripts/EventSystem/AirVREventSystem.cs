using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AirVREventSystem : EventSystem {
    protected override void OnApplicationFocus(bool hasFocus) {
        // do nothing to prevents from being paused when lose focus
    }
}
