using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(scene());
    }

    IEnumerator scene()
    {
        yield return new WaitForSeconds(4);

        SceneManager.LoadScene("MainMenu");

        yield return null;
    }
}
