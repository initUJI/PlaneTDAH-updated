using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneCameraOrientation : MonoBehaviour
{
    public ScreenOrientation screenOrientation = ScreenOrientation.Portrait;

    // Start is called before the first frame update
    void Start()
    {
        Screen.orientation = screenOrientation;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }
}
