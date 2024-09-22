using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ControlEscenas : MonoBehaviour
{
    public void abrirEscena(int numeroEscena) {
        SceneManager.LoadScene(numeroEscena);
    }
}
