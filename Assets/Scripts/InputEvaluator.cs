using UnityEngine;

public class InputEvaluator : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            //Disable Player Input

            //Do Action depending on player input
        }
    }
}
