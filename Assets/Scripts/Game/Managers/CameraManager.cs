using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public GameManager GameManager;

    public Camera MenuCamera;
    public Camera GameCamera;
    public Camera GameUICamera;
    void Start()
    {
        SetCamerasState(false);
        GameManager.GameStarted += GameManager_GameStarted;
    }

    private void GameManager_GameStarted(object sender, System.EventArgs e)
    {
        SetCamerasState(true);
    }

    private void SetCamerasState(bool gameStarted)
    {
        MenuCamera.gameObject.SetActive(!gameStarted);
        GameCamera.gameObject.SetActive(gameStarted);
        GameUICamera.gameObject.SetActive(gameStarted);
    }
}
