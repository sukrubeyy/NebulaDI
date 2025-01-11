using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-1000)]
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
        StartCoroutine(StartInject());
    }

    IEnumerator StartInject()
    {
        yield return null;
        NebulaExtentions.FindInjectAttributesInScene(Container);
        //FindInjectAttributesInScene();
    }


    void Start()
    {
        CreateContainer();
        OverrideBindings();
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public virtual void OverrideBindings()
    {
    }

    void CreateContainer()
    {
        Container = Servises.GenerateContainer();
    }



}
