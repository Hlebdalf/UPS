using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TextLoader : MonoBehaviour {
    private void Start() {
        // Debug.Log("Streaming Assets Path: " + Application.streamingAssetsPath);
    }

    public string LoadText(string fileName) {
        string path = Path.Combine(Application.streamingAssetsPath, fileName) + ".txt";

        if (File.Exists(path)) return (string)(File.ReadAllText(path));
        else {
            Debug.Log("Error: File (" + fileName + ") was not found");
            return "Error";
        }
    }
}
