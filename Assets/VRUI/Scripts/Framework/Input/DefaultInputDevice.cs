using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public sealed class DefaultInputDevice : VRUIInputDevice {
    public static DefaultInputDevice Create(int id, Dictionary<int, VRUIInputDeviceElement> elements) {
        return new DefaultInputDevice(id, elements);
    }

    private DefaultInputDevice(int id, Dictionary<int, VRUIInputDeviceElement> elements) {
        _id = id;
        _elements = elements;
    }

    private int _id;
    private Dictionary<int, VRUIInputDeviceElement> _elements;

    public override int id {
        get {
            return _id;
        }
    }

    protected override Dictionary<int, VRUIInputDeviceElement> CreateInputElements(VRUIManager manager) {
        return _elements;
    }
}
