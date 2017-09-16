﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirVRAutoSelectPointer : AirVRPointer {
    private AirVRInput.Device _currentDevice = AirVRInput.Device.HeadTracker;

    protected override void Update() {
        AirVRInput.Device dev = AirVRInput.IsDeviceAvailable(cameraRig, AirVRInput.Device.TrackedController) ? AirVRInput.Device.TrackedController : AirVRInput.Device.HeadTracker;
        if (dev != _currentDevice) {
            if (AirVRInput.IsDeviceFeedbackEnabled(cameraRig, _currentDevice)) {
                AirVRInput.DisableDeviceFeedback(cameraRig, _currentDevice);
            }
            _currentDevice = dev;
        }

        base.Update();
    }

    // implements AirVRAutoSelectPointer
    protected override AirVRInput.Device device {
        get {
            return _currentDevice;
        }
    }

    public override bool primaryButtonPressed {
        get {
            switch (device) {
                case AirVRInput.Device.HeadTracker:
                    return AirVRInput.GetDown(cameraRig, AirVRInput.Touchpad.Button.Touch) || AirVRInput.GetDown(cameraRig, AirVRInput.Gamepad.Button.A);
                case AirVRInput.Device.TrackedController:
                    return AirVRInput.GetDown(cameraRig, AirVRInput.TrackedController.Button.TouchpadClick) || AirVRInput.GetDown(cameraRig, AirVRInput.TrackedController.Button.IndexTrigger);
            }
            return false;
        }
    }

    public override bool primaryButtonReleased {
        get {
            switch (device) {
                case AirVRInput.Device.HeadTracker:
                    return AirVRInput.GetUp(cameraRig, AirVRInput.Touchpad.Button.Touch) || AirVRInput.GetUp(cameraRig, AirVRInput.Gamepad.Button.A);
                case AirVRInput.Device.TrackedController:
                    return AirVRInput.GetUp(cameraRig, AirVRInput.TrackedController.Button.TouchpadClick) || AirVRInput.GetUp(cameraRig, AirVRInput.TrackedController.Button.IndexTrigger);
            }
            return false;
        }
    }
}
