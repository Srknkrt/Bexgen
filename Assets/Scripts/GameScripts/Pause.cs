using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    public Image panel;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            StopGame(true);
        }
    }

    public void StopGame(bool isGamePaused = false)
    {
        if (isGamePaused)
        {
            panel.gameObject.SetActive(true);
            Time.timeScale = 0;
        }
    }
}
