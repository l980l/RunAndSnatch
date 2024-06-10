using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    [SerializeField]
    private int NextSceneInt;
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F1))
        {
            LoadingSceneController.LoadScene(NextSceneInt);
        }
    }
}
