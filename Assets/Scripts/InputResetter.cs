using UnityEngine;

public class InputResetter : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            //Reset player input (!not enable!)
        }
    }
}
