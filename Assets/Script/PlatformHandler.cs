using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformHandler : MonoBehaviour
{
    public GameObject quitButton;
    public GameObject exitButton;

    void Start()
    {
#if UNITY_ANDROID
        quitButton.SetActive(false);  // Hide on Android
        exitButton.SetActive(false);
#else
        quitButton.SetActive(true);   // Show on PC or other platforms
        exitButton.SetActive(true);
#endif
    }



    // Update is called once per frame
    void Update()
    {

    }
}
