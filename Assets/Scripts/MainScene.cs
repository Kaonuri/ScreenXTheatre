﻿using System.Collections;
using UnityEngine;

public class MainScene : MonoBehaviour
{
    [SerializeField] private AirVRCameraRig cameraRig;

    private IEnumerator Start()
    {
        yield return AirVRCameraFade.FadeAllCameras(this, true, 1f);
    }

	private void Update()
    {
        if (cameraRig.isBoundToClient)
        {
            if (AirVRInput.GetButton(cameraRig, AirVRInput.Gamepad.Button.Back))
            {

            }
        }


//		if (OVRUIPlatformMenu.menu.quitOnBackClick == _moviePlayer.isPlaying) {
//			OVRUIPlatformMenu.menu.quitOnBackClick = !_moviePlayer.isPlaying;
//		}
	}
}
