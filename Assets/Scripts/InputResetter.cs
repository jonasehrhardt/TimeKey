using UnityEngine;

public class InputResetter : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GameManager.instance.pcharacter.ResetSize();
            //Reset player input (!not enable!)
            GameManager.instance.inputManager.ResetPlayerInputs();

            GameManager.instance.addPointsForObstacleCompletion();

			GameManager.instance.pcharacter.ResetSmash ();
        }
    }
}
