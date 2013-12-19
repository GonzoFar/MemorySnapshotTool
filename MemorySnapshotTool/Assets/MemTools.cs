using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace DiagnosticTools
{
    /// <summary>
    ///Tools for working with Unity Memory
    /// </summary>
    public class MemoryTools
    {
        /// <summary>
        /// Gets the object snap shot.
        /// </summary>
        /// <returns>
        /// A collection of all unity memory objects
        /// </returns>
        public static IList<Object> GetObjectSnapShot()
        {
            Object[] resObjs = UnityEngine.Resources.FindObjectsOfTypeAll(typeof(Object));
            Object[] sceneObjs = UnityEngine.Object.FindObjectsOfType(typeof(Object));
            IEnumerable<Object> union = resObjs.Union(sceneObjs);
            return union.ToArray<Object>();
        }

        /// <summary>
        /// Sorts the object collection by object Type, then by name of object.
        /// This makes for an easy compare of two different memory snapshots
        /// </summary>
        /// <returns></returns>
        public static IList<Object> Sort(IList<Object> unSorted)
        {
            if (unSorted == null)
                return null;
            IList<Object> sorted = unSorted.OrderBy(x => x.GetType().ToString()).ThenBy(x => x.name).ToList();
            return sorted;
        }

        /// <summary>
        /// Take a collection of objects and turn it into an array of strings ready to be written
        /// to the snapshot file
        /// </summary>
        /// <param name="objectData">Data to string format</param>
        /// <returns>Strings to record to file</returns>
        public static string[] GetObjectFormattedString(IList<Object> objectData)
        {
            if (objectData == null)
                return new string[0];
            string[] formatted = new string[objectData.Count];
            for (int i = 0; i < objectData.Count; i++)
            {
                if (objectData[i] == null)
                    formatted[i] = "NULL";
                else
                    formatted[i] = objectData[i].GetType().ToString() + ", " + objectData[i].name;
            }
            return formatted;
        }

        /// <summary>
        /// Serialize the object data to disk
        /// </summary>
        /// <param name="path">Full path of file to write</param>
        /// <param name="objectData">Object snapshot to record</param>
        /// <param name="fileHeaders">Any additional information to append at file tope</param>
        /// <returns>True if successful file write</returns>
        public static bool WriteSnapShot(string path, IList<Object> objectData, params string[] fileHeaders)
        {
            try
            {
                List<string> data = new List<string>();
                if (fileHeaders != null)
                {
                    data.AddRange(fileHeaders);
                    data.Add(string.Empty);
                }
                data.AddRange(GetObjectFormattedString(objectData));

                if (!Directory.Exists(Path.GetDirectoryName(path)))
                    Directory.CreateDirectory(Path.GetDirectoryName(path));

                File.WriteAllLines(path, data.ToArray(), System.Text.Encoding.UTF8);
                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError("Couldn't save memory snapshot: " + e.Message);
            }
            return false;
        }

        /// <summary>
        /// Takes a memory snapshot, sorts it by type and name, then writes it to file at path along with fileHeaders
        /// </summary>
        /// <param name="path">Where to save snapshot</param>
        /// <param name="fileHeaders">Any additional headers to include at top of snapshot file</param>
        /// <returns>True if successful writing</returns>
        public static bool WriteSnapShotSorted(string path, params string[] fileHeaders)
        {
            var snapshot = GetObjectSnapShot();
            var sorted = Sort(snapshot);
            return WriteSnapShot(path, sorted, fileHeaders);
        }
    }
}