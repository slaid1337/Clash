using System.Linq;
using UnityEditor;
using UnityEngine;


public static class HierarchyExtention
{
#if UNITY_EDITOR
    [MenuItem("Hierarchy/SelectObjectsWithSameNames")]
    public static void SelectObjectsWithSameNames()
    {
        string[] names = Selection.objects.Select(o => o.name).ToArray();

        GameObject[] findingObjects = GameObject.FindObjectsOfType<GameObject>().Where(x => names.Contains(x.name)).Select(x => x.gameObject).ToArray();

        Selection.objects = findingObjects;
    }
#endif
}
