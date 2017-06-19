using UnityEngine;

public class SlowMoField : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            //Enable Player Input
        }
    }
}
