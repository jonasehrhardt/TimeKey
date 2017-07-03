using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEditor.VersionControl;

public class LevelManager : MonoBehaviour {

    public GameObject startStage;
    public int minLevelSize = 10;
    public int maxLevelSize = 20;

    private List<GameObject> stages;
    private GameObject lastStage;

    GameObject getRandomStage()
    {
        var randomID = Random.Range(0, stages.Count);
        return stages[randomID];
    }

    // Use this for initialization
    void Start () {

        DirectoryInfo info = new DirectoryInfo(Application.dataPath + "/Prefabs/Level/LevelModules");
        FileInfo[] fileInfo = info.GetFiles();
        stages = new List<GameObject>();

        for (int i = 0; i < fileInfo.Length; i++)
        {
            string ext = fileInfo[i].Extension;
            if (ext != ".prefab") continue;
            string path = "Assets/Prefabs/Level/LevelModules/" + fileInfo[i].Name;
            string name = Path.GetFileNameWithoutExtension(fileInfo[i].Name);

            if(startStage.name != name) stages.Add((GameObject)AssetDatabase.LoadAssetAtPath(path, typeof(GameObject)));
        }

        foreach (Transform child in this.transform)
        {
            if (startStage.name != child.name) Destroy(child.gameObject);
        }


        lastStage = startStage;

        var randomCount = Random.Range(minLevelSize, maxLevelSize);
        for (int i=0; i<randomCount; i++) {
            GameObject stage = Instantiate(getRandomStage(), this.transform);
            stage.transform.position = lastStage.transform.position + new Vector3(20,0,0);
            lastStage = stage;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
