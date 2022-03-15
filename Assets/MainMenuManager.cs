using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class MainMenuManager : MonoBehaviour
{

    public TMP_InputField nameInput;
    private string userName;
    public static MainMenuManager Instance;

    private void Awake()
    {   
        if (Instance != null)
        {
            GameObject.Destroy(this);
        }else{
            Instance = this;
            DontDestroyOnLoad(this);
        }
    }

    public void SetUserName()
    {
        userName = nameInput.GetComponentInChildren<TMP_InputField>().text;
    }

    public string GetUserName()
    {
        return userName;
    }

    public void StartGame()
    {
        if(userName != null)
        {
            SceneManager.LoadScene(1);
        }else{ 
            Debug.Log("No name entered"); 
        }    
    }
}
