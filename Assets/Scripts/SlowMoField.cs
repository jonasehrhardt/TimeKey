using UnityEngine;

public class SlowMoField : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            //Enable Player Input
            GameManager.instance.inputManager.EnablePlayerInput();
            GameManager.instance.pcharacter.SlowDown(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            //Disable Player Input
            GameManager.instance.inputManager.DisablePlayerInput();
            GameManager.instance.pcharacter.SlowDown(false);
        }
    }
}
