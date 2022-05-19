using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuButtons : MonoBehaviour
{
    public void PlayBtn()
    {
        SceneManager.LoadScene("Scenes/Game");
    }

    public void SettingsBtn()
    {

    }

    public void QuitBtn()
    {
        Application.Quit();
    }
}
