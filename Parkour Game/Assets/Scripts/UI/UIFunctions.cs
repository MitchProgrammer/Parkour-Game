using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIFunctions : MonoBehaviour
{
    public void LoadLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }
    public void ToggleUI(GameObject UIPanel)
    {
        UIPanel.SetActive(!UIPanel.activeSelf);
    }
    public void Quit()
    {
        Application.Quit();
    }
}