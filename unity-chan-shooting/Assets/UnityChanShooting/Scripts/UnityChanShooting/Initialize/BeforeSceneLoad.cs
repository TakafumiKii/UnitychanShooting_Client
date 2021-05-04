using UnityEngine;

public class BeforeSceneLoad {

    const int WindowWidth = 532;
    const int WindowHeight = 945;
    const bool IsFullScreen = false;
    const int PreferredRefreshRate = 60;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void FixedWindowSize()
    {
        Screen.SetResolution(WindowWidth, WindowHeight, IsFullScreen, PreferredRefreshRate);
    }
}
