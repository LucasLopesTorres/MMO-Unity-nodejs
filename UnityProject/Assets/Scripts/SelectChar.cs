using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectChar : MonoBehaviour
{
    public InputField playerNameInput;

    public string PlayerName{get; private set; }
    public int CharId { get; private set; }

    public GameObject[] charList;

    private void Start()
    {
        CharId = 0;
        CurrentChar();
    }

    //EndEdit input PlayerName
    public void UpdatePlayerName()
    {
        PlayerName = playerNameInput.text;
    }

    //NextButton CharPanel
    public void NextChar()
    {
        if (CharId < charList.Length)
        {
            CharId++;
            CurrentChar();
        }
    }

    //PreviousButton CharnPanel
    public void PreviousChar()
    {
        if (CharId > 0)
        {
            CharId--;
            CurrentChar();
        } 
    }

    void CurrentChar()
    {
        foreach (var item in charList)
        {
            if (item.activeSelf)
                item.SetActive(false);
        }

        charList[CharId].SetActive(true);
    }
}
