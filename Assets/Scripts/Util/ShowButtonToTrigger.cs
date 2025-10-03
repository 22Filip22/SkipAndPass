using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShowButtonToTrigger : MonoBehaviour
{
    [SerializeField]
    char CharacterToShow;


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

            SceneManager.LoadScene("elementry_school");
        }
    }
}
