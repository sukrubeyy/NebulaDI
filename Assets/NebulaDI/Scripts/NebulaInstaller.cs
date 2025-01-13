using UnityEngine;
using UnityEngine.SceneManagement;

public class NebulaInstaller : MonoBehaviour
{
    protected NebulaServiceCollection Servises = new NebulaServiceCollection();
    protected NebulaContainer Container;
    #region Editor

    #endregion

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        NebulaExtentions.FindInjectAttributesInScene(Container);
    }

    void Awake()
    {
        CreateContainer();
        OverrideBindings();
        DontDestroyOnLoad(gameObject);
    }

    public virtual void OverrideBindings()
    {
    }

    void CreateContainer()
    {
        Container = Servises.GenerateContainer(transform.GetChild(0));
    }
}
