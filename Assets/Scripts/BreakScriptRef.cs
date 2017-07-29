using System.Collections;
using UnityEngine;

public class BreakScriptRef : MonoBehaviour {

    private BreakScript breakscript = null;

    private void Start()
    {
        GameObject remains = GameObject.Find(name + " Remains");
        if (remains == null) Debug.LogError("Could not find '" + remains.name + "'!");
        breakscript = remains.GetComponent<BreakScript>();
    }

    public void cleanUp()
    {
        breakscript.cleanUp();
     }

    public bool isAlive()
    {
        return GetComponent<MeshRenderer>().enabled;
    }

    public void PlayDeath()
    {
        if (isAlive()) StartCoroutine(Kill(false));
    }

    public void Revive()
    {
        if (!isAlive())
        {
            GetComponent<Rigidbody>().isKinematic = false;
            GetComponent<MeshRenderer>().enabled = true;
            cleanUp();
        }
    }

    private IEnumerator Kill(bool destroy)
    {
        if (GetComponent<MeshFilter>() == null || GetComponent<SkinnedMeshRenderer>() == null)
        {
            yield return null;
        }

        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;

        breakscript.transform.position = transform.position;
        breakscript.transform.rotation = transform.rotation;
        breakscript.Explode();

        if (destroy == true)
        {
            yield return new WaitForSeconds(1.0f);
            new WaitForSeconds(1.0f);
            Destroy(gameObject);
        }
    }
}
