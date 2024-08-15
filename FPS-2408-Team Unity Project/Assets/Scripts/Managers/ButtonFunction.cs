using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFunction : MonoBehaviour
{
    [SerializeField] private Transform respawnPoint;

    public void resume()
    {
        UIManager.instance.stateUnpause();
    }
    public void restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        UIManager.instance.stateUnpause();
    }
    public void quit()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void respawn()
    {
        GameManager.instance.playerControllerRef.ResetHealth();
        GameManager.instance.playerRef.transform.position = respawnPoint.position;
        UIManager.instance.stateUnpause();
    }
    // Start is called before the first frame update

}
