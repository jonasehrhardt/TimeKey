using UnityEngine;

public class ScreenSize : MonoBehaviour {

    public int width = 300;
    public int height = 100;
    
    private int currentScaleFactor = 1;

    private void Awake()
    {
        Screen.SetResolution(width, height, false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (currentScaleFactor < 5)
            {
                currentScaleFactor += 1;
                Screen.SetResolution(width * currentScaleFactor, height * currentScaleFactor, false);
            }
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            if (currentScaleFactor > 1)
            {
                currentScaleFactor -= 1;
                Screen.SetResolution(width * currentScaleFactor, height * currentScaleFactor, false);
            }
        }
    }
}
