using UnityEngine;

public class ScreenSize : MonoBehaviour {

    public int width = 300;
    public int height = 100;
    public bool fullscreen = false;

    private void Awake()
    {
        Screen.SetResolution(width, height, fullscreen);
    }
}
