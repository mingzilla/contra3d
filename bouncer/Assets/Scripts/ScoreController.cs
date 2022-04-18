using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour
{
    private Text textComponent;

    private void Start()
    {
        textComponent = GetComponent<Text>();
        Camera mainCamera = Camera.main;
        float y = mainCamera.orthographicSize;
        float x = y * mainCamera.aspect;
        textComponent.transform.position = new Vector3((0 - x + 150), (y + 1000), 0);
    }

    void Update()
    {
        textComponent.text = "" + GamePlayerController.hitCount;
    }
}