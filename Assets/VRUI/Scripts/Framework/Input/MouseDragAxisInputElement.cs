using UnityEngine;
using System.Collections;

public class MouseDragAxisInputElement : VRUIInputDeviceElement {
    public MouseDragAxisInputElement(int id, Direction direction, float maxDelta) {
        _id = id;
        _maxDelta = maxDelta;
        _direction = direction;
        _dragging = false;
        _basePos = 0.0f;
    }

    public enum Direction { 
        Horizontal,
        Vertical
    }

    private int _id;
    private float _maxDelta;
    private Direction _direction;
    private bool _dragging;
    private float _basePos;

    private float currentMousePosition() {
        return (_direction == Direction.Horizontal) ? Input.mousePosition.x : Input.mousePosition.y;
    }

    protected override int id {
        get {
            return _id;
        }
    }

    public override float value {
        get {
            return _dragging ? Mathf.Max(-1.0f, Mathf.Min(1.0f, (currentMousePosition() - _basePos) / _maxDelta)) : 0.0f;
        }
    }

    public override void ProcessAndDispatchInputEvent(VRUIManager manager, int deviceID) {
        if (Input.GetMouseButton(0) && _dragging == false) {
            _dragging = true;
            _basePos = currentMousePosition();
        }
        else if (Input.GetMouseButton(0) == false && _dragging) {
            _dragging = false;
        }
    }

    public override void Reset(VRUIManager manager, int deviceID) {
        _dragging = false;
    }
}
