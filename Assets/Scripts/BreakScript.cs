using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakScript : MonoBehaviour {

    private GameObject debris = null;
    private List<GameObject> pieces = new List<GameObject>();

    void Start()
    {
        debris = new GameObject("Debris");
        debris.SetActive(false);
        Break();
    }

    public void Explode()
    {
        debris.transform.position = transform.position;
        debris.transform.rotation = transform.rotation;
        debris.SetActive(true);

        foreach (GameObject p in pieces)
        {
            Rigidbody rb = p.GetComponent<Rigidbody>();
            rb.isKinematic = false;
            Vector3 explosionPos = new Vector3(transform.position.x + Random.Range(-0.5f, 0.5f), transform.position.y + Random.Range(0f, 0.5f), transform.position.z + Random.Range(-0.5f, 0.5f));
            rb.AddExplosionForce(Random.Range(300, 500), explosionPos, 5);
        }
    }

    void FixedUpdate()
    {
        foreach (GameObject p in pieces)
        {
            Rigidbody rb = p.GetComponent<Rigidbody>();
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, 0);
            rb.transform.position = new Vector3(rb.transform.position.x, rb.transform.position.y, 0);
        }
    }

    public void cleanUp()
    {
        foreach (GameObject p in pieces)
        {
            Rigidbody rb = p.GetComponent<Rigidbody>();
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.transform.position = debris.transform.position;
            rb.transform.rotation = debris.transform.rotation;
            rb.isKinematic = true;
        }
        debris.SetActive(false);
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
        //Break();
        Explode();

        if (destroy == true)
        {
            yield return new WaitForSeconds(1.0f);
            new WaitForSeconds(1.0f);
            Destroy(gameObject);
        }
    }

    public void Break()
    {
        Mesh M = new Mesh();
        if (GetComponent<MeshFilter>())
        {
            M = GetComponent<MeshFilter>().mesh;
        }
        else if (GetComponent<SkinnedMeshRenderer>())
        {
            M = GetComponent<SkinnedMeshRenderer>().sharedMesh;
        }

        Material[] materials = new Material[0];
        if (GetComponent<MeshRenderer>())
        {
            materials = GetComponent<MeshRenderer>().materials;
        }
        else if (GetComponent<SkinnedMeshRenderer>())
        {
            materials = GetComponent<SkinnedMeshRenderer>().materials;
        }

        Vector3[] verts = M.vertices;
        Vector3[] normals = M.normals;
        Vector2[] uvs = M.uv;
        for (int submesh = 0; submesh < M.subMeshCount; submesh++)
        {

            int[] indices = M.GetTriangles(submesh);

            for (int i = 0; i < indices.Length; i += 3)
            {
                Vector3[] newVerts = new Vector3[3];
                Vector3[] newNormals = new Vector3[3];
                Vector2[] newUvs = new Vector2[3];
                for (int n = 0; n < 3; n++)
                {
                    int index = indices[i + n];
                    newVerts[n] = verts[index];
                    newUvs[n] = uvs[index];
                    newNormals[n] = normals[index];
                }

                Mesh mesh = new Mesh();
                mesh.vertices = newVerts;
                mesh.normals = newNormals;
                mesh.uv = newUvs;

                mesh.triangles = new int[] { 0, 1, 2, 2, 1, 0 };

                GameObject GO = new GameObject("Triangle " + (i / 3));
                GO.layer = LayerMask.NameToLayer("Debris");
                GO.transform.position = debris.transform.position;
                GO.transform.rotation = debris.transform.rotation;
                GO.transform.parent = debris.transform;
                GO.AddComponent<MeshRenderer>().material = materials[submesh];
                GO.AddComponent<MeshFilter>().mesh = mesh;
                GO.AddComponent<BoxCollider>();
                GO.AddComponent<Rigidbody>().isKinematic = true;

                pieces.Add(GO);
            }
        }
    }
}
