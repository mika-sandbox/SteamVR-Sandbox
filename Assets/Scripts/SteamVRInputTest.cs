﻿using UnityEngine;

using Valve.VR;

public class SteamVRInputTest : MonoBehaviour
{
    public SteamVR_Action_Boolean GrabAction;
    public SteamVR_Input_Sources HandType;

    // Update is called once per frame
    private void Update()
    {
        if (GrabAction.GetState(HandType)) Debug.Log(GrabAction.ToString());
    }
}