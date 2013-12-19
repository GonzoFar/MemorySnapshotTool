using UnityEngine;
using System.Collections;
using DiagnosticTools;

/// <summary>
/// An example diagnostic tool for recording snapshots.  Modify for your own needs.
/// This example simply displays a button that takes a memory snapshot.
/// </summary>
public class SnapshotController_Example : MonoBehaviour
{
    const string relativeFilePath = "/MemorySnapshots/";
    const string fileNamePrefix = "snapshot_";
    const string fileExt = ".txt";

    int snapShotIterator = 0;

    static bool _created = false;

    /// <summary>
    /// We only want one instance of this tool when we're using it.
    /// And that instance persists between scenes.
    /// </summary>
    void Awake()
    {
        if (_created)
            Destroy(this);
        else
        {
            _created = true;
            DontDestroyOnLoad(this);
        }
    }

    void OnGUI()
    {
        if (GUILayout.Button("Take snapshot number " + snapShotIterator))
        {
            //Writes to project folder in editor, writes to cache folder on devices
            string path = Application.isEditor ? Application.dataPath : Application.persistentDataPath;
            path += (relativeFilePath + fileNamePrefix + snapShotIterator.ToString() + fileExt);

            //Make some headers for the new file
            string[] headers = new string[]
            {
                "Snapshot Number: "+snapShotIterator.ToString(),
                "Timestamp: "+UnityEngine.Time.realtimeSinceStartup.ToString()
                //, Add any other info you want at the top of the file...
            };

            //Write a new snapshot away at the path specified and with the above headers
            if (MemoryTools.WriteSnapShotSorted(path, headers))
            {
                Debug.Log("Snapshot number " + snapShotIterator.ToString() + " written to path: " + path);
                snapShotIterator++;
            }
        }
    }
}
