using UnityEngine;
using UnityEngine.SceneManagement; 
using UnityEngine.UI;
public class Buttons : MonoBehaviour
{
    
    public void StartGame()
    {
        SceneManager.LoadScene("test");
    }

    public void ContinueGame()
    {
        SceneManager.GetSceneByName("test");
    }

    public void ReturnMain()
    {
        SceneManager.LoadScene("Menu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    void Start()
    {
     
    }


}
