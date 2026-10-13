using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        StartGameNow();   
    }

    void StartGameNow()
    {
        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            SceneManager.LoadScene("Main");
        }
    }
}
