using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ControlEscenas : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (SceneManager.GetActiveScene().buildIndex == 0)
            {
                // Si estamos en la escena del menú principal, cerrar la aplicación
                Application.Quit();
            }
            else
            {
                // Si estamos en otra escena, cargar el menú principal
                SceneManager.LoadScene(0);
            }
        }
    }
    public void abrirEscena(int numeroEscena)
    {
        SceneManager.LoadScene(numeroEscena);
    }
}
