using UnityEngine;
using System.Collections;

public class UnityAxisInputElement : VRUIInputDeviceElement {
    public UnityAxisInputElement(int id, string axisName) {
        _id = id;
        _axisName = axisName;
    }

    private int _id;
    private string _axisName;

    protected override int id {
        get {
            return _id;
        }
    }

    public override float value {
        get {
            return Input.GetAxis(_axisName);
        }
    }

    public override void ProcessAndDispatchInputEvent(VRUIManager manager, int deviceID) {}
    public override void Reset(VRUIManager manager, int deviceID) {}
}
