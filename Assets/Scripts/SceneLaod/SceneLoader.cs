using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LoadType
{
    Click,
    Collide,
}

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private LoadType loadType;
    [SerializeField] private int NextSceneInt;

    void Update()
    {
        if (loadType == LoadType.Click)
        {
            if (Input.GetMouseButtonUp(0))
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
