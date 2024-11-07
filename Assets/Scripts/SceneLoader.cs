using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

// Copyright (C) Tom Troeger

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;
    // Start is called before the first frame update
   
    #if UNITY_EDITOR
        [SerializeField] private SceneAsset[] initialScenes;
    #endif
    
    private List<string> _initialScenes = new List<string>();
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        if (_initialScenes.Count == 0)
        {
            _initialScenes.Add("GameBowling");
            _initialScenes.Add("GameGolf");
            _initialScenes.Add("GameMenu");
        }
        foreach (var scene in _initialScenes)
        {
            LoadScene(scene);
        }
    }

    public bool LoadScene(string sceneName)
    {
        StartCoroutine(LoadYourAsyncScene(sceneName));
        return true;
    }
    
    IEnumerator LoadYourAsyncScene(string sceneName)
    {
        // The Application loads the Scene in the background as the current Scene runs.
        // This is particularly good for creating loading screens.
        // You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
        // a sceneBuildIndex of 1 as shown in Build Settings.

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

        // Wait until the asynchronous scene fully loads
        while (asyncLoad is { isDone: false })
        {
            yield return null;
        }
    }
    
#if UNITY_EDITOR
    public void OnAfterDeserialize ( ) => FillScenes ( );
    public void OnBeforeSerialize ( ) => FillScenes ( );
    public void OnValidate ( ) => FillScenes ( );

    private void FillScenes ( )
    {
        _initialScenes ??= new List<string>();
        _initialScenes.Clear ( );
        foreach ( var s in initialScenes )
            _initialScenes.Add ( s.name );
    }
#endif
}
