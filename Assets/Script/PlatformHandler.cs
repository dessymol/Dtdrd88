using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformHandler : MonoBehaviour
{
    public GameObject quitButton;
    public GameObject exitButton;

    void Start()
    {
#if UNITY_STANDALONE
        quitButton.SetActive(true);   // Show on PC/Mac
        exitButton.SetActive(true);
#else
        quitButton.SetActive(false);  // Hide on Android
        exitButton.SetActive(false);
#endif
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
