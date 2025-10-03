using System;
using TMPro;
using UnityEditor;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShowButtonToTrigger : MonoBehaviour
{
    [SerializeField]
    char CharacterToShow;

#if UNITY_EDITOR
    [SerializeField] private SceneAsset sceneAsset;
#endif

    private void Start()
    {
        GetComponentInChildren<TextMeshProUGUI>().text = CharacterToShow + "";

        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(
            (KeyCode)Enum.Parse(
                typeof(KeyCode), CharacterToShow.ToString().ToUpper())))
        {
            Debug.Log("-------------------------------------------");

            SceneManager.LoadScene(sceneAsset.name);
        }
    }
}
