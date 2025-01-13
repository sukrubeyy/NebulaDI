using UnityEditor;
using UnityEngine;
public class CreateNebulaDIEditor : MonoBehaviour
{
    [MenuItem("GameObject/Nebula/Create Nebula Tool", false, 10)]
    public static void CreateNebulaTool()
    {
        GameObject nebulaInstaller = new GameObject("Nebula Installer");
        nebulaInstaller.AddComponent<Nebula>();
        Selection.activeGameObject = nebulaInstaller;

        GameObject Container = new GameObject("Nebula Container");
        Container.transform.SetParent(nebulaInstaller.transform);

    }
}
