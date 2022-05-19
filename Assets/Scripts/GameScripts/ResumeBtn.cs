using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResumeBtn : MonoBehaviour
{
    public Image panel;

    public void Resume()
    {
        panel.gameObject.SetActive(false);
        Time.timeScale = 1.0f;
    }
}
