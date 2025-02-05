using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class WinManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public TextMeshProUGUI winnerText;
    void Start()
    {
        string winner = PlayerPrefs.GetString("Winner");
        winnerText.text = winner + " Wins!";
    }

    // Update is called once per frame
    public void PlayAgain()
    {
        SceneManager.LoadScene("TrailCollide");
    }
}
