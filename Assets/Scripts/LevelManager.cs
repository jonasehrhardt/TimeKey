using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.IO;
#if UNITY_EDITOR
using UnityEditor.VersionControl;
#endif

public class LevelManager : MonoBehaviour
{

    public GameObject startStage;
    public List<GameObject> stages;
    public int obstacles = 99;

    private GameObject lastStage;

    GameObject getRandomStage()
    {
        var randomID = Random.Range(0, stages.Count);
        return stages[randomID];
    }

    // Use this for initialization
    void Start()
    {
        /*
        DirectoryInfo info = new DirectoryInfo(Application.dataPath + "/Prefabs/Level/LevelModules");
        FileInfo[] fileInfo = info.GetFiles();
        stages = new List<GameObject>();

        for (int i = 0; i < fileInfo.Length; i++)
        {
            string ext = fileInfo[i].Extension;
            if (ext != ".prefab") continue;
            string path = "Assets/Prefabs/Level/LevelModules/" + fileInfo[i].Name;
            string name = Path.GetFileNameWithoutExtension(fileInfo[i].Name);

            #if UNITY_EDITOR
            if(startStage.name != name) stages.Add((GameObject)AssetDatabase.LoadAssetAtPath(path, typeof(GameObject)));
            #endif
        
        }
        */

        //Destroy everything but the starting area
        foreach (Transform child in transform)
        {
            if (startStage.name != child.name) Destroy(child.gameObject);
        }

        lastStage = startStage;
                
        for (int i = 0; i < obstacles; i++)
        {
            GameObject stage = Instantiate(getRandomStage(), transform);
            stage.transform.position = lastStage.transform.position + new Vector3(20, 0, 0);
            lastStage = stage;
        }
    }
}
