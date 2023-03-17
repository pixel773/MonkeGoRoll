using BepInEx;
using System;
using UnityEngine;
using Utilla;
using UnityEngine.XR;
using System.Collections.Generic;
using GorillaLocomotion;

namespace MonkeGoRoll
{
    [ModdedGamemode]
    [BepInDependency("org.legoandmars.gorillatag.utilla", "1.6.7")]
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Plugin : BaseUnityPlugin
    {
        bool inRoom = false;

        bool YButton;
        bool XButton;

        Rigidbody PlayerRigidbody;
        Transform Player;

        void Start()
        {
            Utilla.Events.GameInitialized += OnGameInitialized;
        }

        void OnEnable()
        {
            HarmonyPatches.ApplyHarmonyPatches();
        }

        void OnDisable()
        {
            PlayerRigidbody.freezeRotation = true;
            Player.transform.localRotation = new Quaternion(0, 0, 0, 0);
            HarmonyPatches.RemoveHarmonyPatches();
        }

        void OnGameInitialized(object sender, EventArgs e)
        {
            PlayerRigidbody = GameObject.Find("GorillaPlayer").GetComponent<Rigidbody>();
            Player = GameObject.Find("GorillaPlayer").GetComponent<Transform>();
        }

        void Update()
        {
            List<InputDevice> list = new List<InputDevice>();
            InputDevices.GetDevicesWithCharacteristics(UnityEngine.XR.InputDeviceCharacteristics.HeldInHand | UnityEngine.XR.InputDeviceCharacteristics.Left | UnityEngine.XR.InputDeviceCharacteristics.Controller, list);
            list[0].TryGetFeatureValue(CommonUsages.secondaryButton, out YButton);
            list[0].TryGetFeatureValue(CommonUsages.primaryButton, out XButton);

            if(YButton == true && XButton == false && inRoom == true)
            {
                PlayerRigidbody.freezeRotation = false;
            }
            if(XButton == true && YButton == false && inRoom == true)
            {
                PlayerRigidbody.freezeRotation = true;
                Player.transform.localRotation = new Quaternion(0, 0, 0, 0);
            }
        }

        [ModdedGamemodeJoin]
        public void OnJoin(string gamemode)
        {
            inRoom = true;
        }

        [ModdedGamemodeLeave]
        public void OnLeave(string gamemode)
        {
            PlayerRigidbody.freezeRotation = true;
            Player.transform.localRotation = new Quaternion(0, 0, 0, 0);
            inRoom = false;
        }
    }
}
