using UnityEngine;
using System.Collections;

public class ControllerInput : MonoBehaviour {

    public OSType OS;

    public enum OSType {
        MOBILE, MAC, WINDOWS
    }

	// Use this for initialization
	void Start () {
        #if UNITY_EDITOR
            OS = OSType.WINDOWS;

        #elif UNITY_IOS
            OS = OSType.MOBILE;

        #elif UNITY_ANDROID
            OS = OSType.MOBILE;

        #elif UNITY_STANDALONE_OSX
            OS = OSType.MAC;

        #elif UNITY_STANDALONE_WIN
            OS = OSType.WINDOWS;
        #endif
    }
}
