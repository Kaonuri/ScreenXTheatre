using UnityEngine;
using System.Collections;

public class UnityButtonInputElement : VRUIButtonInputElement {
    public UnityButtonInputElement(int id, KeyCode keyCode) {
        _id = id;
        _keyCode = keyCode;
    }

    private int _id;
    private KeyCode _keyCode;

    protected override int id {
        get {
            return _id;
        }
    }

    protected override bool isButtonDown() {
        return Input.GetKey(_keyCode);
    }

    protected override bool isButtonUp() {
        return Input.GetKey(_keyCode) == false;
    }
}

