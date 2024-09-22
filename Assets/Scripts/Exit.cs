using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour
{

    void Update() {
        if (Input.GetKey("escape"))
            {
                ExitGame();
            }
        
    }

    public void ExitGame (){
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}
