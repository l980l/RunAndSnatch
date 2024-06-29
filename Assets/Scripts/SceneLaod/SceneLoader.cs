using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LoadType
{
    AnyButton,
    Collide,
}

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private LoadType loadType;
    [SerializeField] private int NextSceneInt;

    void Update()
    {
        if (loadType == LoadType.AnyButton)
        {
            if (Input.GetKeyDown(KeyCode.F1))
            {
                LoadingSceneController.LoadScene(NextSceneInt);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        LoadingSceneController.LoadScene(NextSceneInt);
    }
}
