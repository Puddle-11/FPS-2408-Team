using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    public LayerMask projectileIgnore;
    public LayerMask penetratingIgnore;
    public static GameManager instance;


    [HideInInspector] public GameObject playerRef;
    [HideInInspector] public PlayerController playerControllerRef;
    private int enemyCount;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("Found multiple game managers in scene\nDestroyed Game Manager at " + gameObject.name);
            Destroy(this);
        }
    }

    public void respawn()
    {
        playerControllerRef.ResetHealth();
        playerControllerRef.spawnPlayer();
        
        UIManager.instance.StateUnpause();
    }
    private void OnEnable()
    {
        BootLoadManager.instance.gameSceneChangeEvent += respawn;
        
    }
    private void OnDisable()
    {
        BootLoadManager.instance.gameSceneChangeEvent -= respawn;

    }
    private void Start()
    {

        if (TryFindPlayer(out playerRef))
        {
            playerRef.TryGetComponent<PlayerController>(out playerControllerRef);
            if (playerControllerRef != null) respawn();
        }
    }
    public bool TryFindPlayer(out GameObject _ref)
    {
        GameObject tempref = GameObject.FindWithTag("Player");
        if (tempref != null)
        {
            _ref = tempref;
            return true;
        }
        _ref = null;
        return false;

    }

    public void updateGameGoal(int _amount)
    {
        enemyCount += _amount;
        UIManager.instance.SetEnemyCount(enemyCount);

        if (enemyCount <= 0)
        {
            //UIManager.instance.ToggleEnemyCount(false);
            UIManager.instance.ToggleWinMenu(true);
        }
    }

}
