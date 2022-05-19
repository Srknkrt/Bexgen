using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuitBtn : MonoBehaviour
{
    public void Quit()
    {
        SceneManager.LoadScene("Scenes/MainMenu");
    }

}
