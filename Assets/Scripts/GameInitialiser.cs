using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameInitialiser : MonoBehaviour
{
    public PartyData defaultParty;
    public Button continueButton;
    public void Start()
    {
        string path = Application.persistentDataPath + "/party.data";
        if (!File.Exists(path))
        {
            continueButton.interactable = false;
        }
    }
    public void InitializeGame()
    {
        SaveSystem.SavePartyData(defaultParty);
    }

    public void ToNextScene()
    {
        SceneManager.LoadScene("HomeBase");
    }
}
