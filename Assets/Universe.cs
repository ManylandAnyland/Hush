using UnityEngine;
using System.Collections;

public class Universe : MonoBehaviour {

    public AudioSource successSound;
    public GameObject textObject;

    private string newline = "\n";

	void Start () {
        string sceneFilePath = "../Assets/Universe.unity";
        string sceneFilesOutputPath = "../SceneFiles";

        bool doSplit = false;
        
        if (doSplit) {
            SplitFilesFromSceneFile(sceneFilePath, sceneFilesOutputPath);
        }
        else {
            MergeFilesIntoSceneFile(sceneFilePath, sceneFilesOutputPath);
        }
        
        ShowSuccess(doSplit);
        Invoke("Quit", 0.5f);
	}

    void ShowSuccess(bool doSplit) {
        textObject.GetComponent<TextMesh>().text = "Ok, " + (doSplit ? "split" : "merge") + " done.";
        successSound.Play();
    }
	
	void SplitFilesFromSceneFile(string sceneFilePath, string sceneFilesOutputPath) {
        string text = System.IO.File.ReadAllText(sceneFilePath);
        text = text.Replace("\r\n", newline);
        text = text.Replace("\r", newline);

        string separator = "--- !u!";
        
        string[] parts = text.Split(
                new string[] {separator},
                System.StringSplitOptions.RemoveEmptyEntries
                );

        if ( System.IO.Directory.Exists(sceneFilesOutputPath) ) {
            System.IO.Directory.Delete(sceneFilesOutputPath, true);
        }
        System.IO.Directory.CreateDirectory(sceneFilesOutputPath);

        for (var i = 1; i < parts.Length; i++) {
            string firstLine = parts[i].Split(
                    new string[] {newline},
                    System.StringSplitOptions.RemoveEmptyEntries)[0];
            string id = firstLine.Split("&"[0])[1];
       
            System.IO.File.WriteAllText(
                    sceneFilesOutputPath + "/" + id + ".txt",
                    separator + parts[i]
                    );
        }
	}

    void MergeFilesIntoSceneFile(string sceneFilePath, string sceneFilesOutputPath) {
        string fileStart = "%YAML 1.1" + newline +
                "%TAG !u! tag:unity3d.com,2011:" + newline;

        string allText = fileStart;

        foreach ( string file in System.IO.Directory.GetFiles(sceneFilesOutputPath) ) {
            string fileText = System.IO.File.ReadAllText(file);
            allText += fileText;
        }
        
        System.IO.File.WriteAllText(sceneFilePath, allText);
	}

	void Quit() {
        Application.Quit();
    }
    	
}
