using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

internal class AirVRPointerEventData : PointerEventData {
    public AirVRPointerEventData(EventSystem eventSystem) : base(eventSystem) { }

    public Ray worldSpaceRay;
}

internal static class PointerEventDataExtension {
    public static bool IsVRPointer(this PointerEventData pointerEventData) {
        return pointerEventData is AirVRPointerEventData;
    }

    public static Ray GetRay(this PointerEventData pointerEventData) {
        return (pointerEventData as AirVRPointerEventData).worldSpaceRay;
    }
}