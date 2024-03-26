using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    [HideInInspector] public GameObject player1; 
    [HideInInspector] public GameObject player2;

    #region singleton
    public static CameraZoom instance;

    private void Awake()
    {
        instance = this;
    }
    #endregion

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 player1Pos = player1.transform.position;
        Vector3 player2Pos = player2.transform.position;

        float distance = Vector3.Distance(player1Pos, player2Pos);

        float distanceBetweenX = (player1.transform.position.x + player2.transform.position.x) / 2;
        float distanceBewteenZ = (player1.transform.position.x + player2.transform.position.x) / 2;

        float cameraPos = 15 + (Mathf.Sqrt(3 * distance));

        float xCameraPos = distanceBetweenX;
        float yCameraPos = cameraPos;
        float zCameraPos = distanceBewteenZ + cameraPos;

        Vector3 target = new Vector3(xCameraPos, yCameraPos, zCameraPos);

        transform.position = target;
    }
}
