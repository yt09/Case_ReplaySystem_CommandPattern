

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Replay
{

    /// <summary>
    /// Unity物品管理
    /// </summary>
    public class UnityObjectManager
    {
        public static List<GameObject> goList = new List<GameObject>();
        public static List<Component> coList = new List<Component>();

        public static void RefreshGos()
        {
            goList.Clear();
            Array.ForEach(SceneManager.GetActiveScene().GetRootGameObjects(), p => goList.AddRange(Array.ConvertAll(p.GetComponentsInChildren<Transform>(true), q => q.gameObject)));
            goList.Sort((p, q) => q.GetInstanceID().CompareTo(p.GetInstanceID()));
        }

        public static int GetGameObjectID(GameObject gameObject)
        {
            return goList.FindIndex(p => p == gameObject);
        }

        public static GameObject GetGameObjectByID(int id)
        {
            if (id < 0 || id > goList.Count - 1)
            {
                Debug.Log("ID越界");
                return null;
            }
            return goList[id];
        }

        public static void RefreshCos()
        {
            coList.Clear();
            goList.ForEach(p => coList.AddRange(p.GetComponents<Component>()));
            coList.Sort((p, q) => q.GetInstanceID().CompareTo(p.GetInstanceID()));
        }

        public static void Refresh()
        {
            RefreshGos();
            RefreshCos();
        }

        public static int GetComponentID(Component component)
        {
            return coList.FindIndex(p => p == component);
        }

        public static Component GetComponentByID(int id)
        {
            if (id < 0 || id > coList.Count - 1)
            {
                Debug.Log(id.ToString() + " ID越界");
                return null;
            }
            return coList[id];
        }
    }
}
