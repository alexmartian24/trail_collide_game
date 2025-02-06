using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("TrailCollide"); 
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)){
            SceneManager.LoadScene("TrailCollide");
        }
    }

}
