using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    private Checkpoint currentCheckpoint;
    public string arenaSceneName;
    public Transform player;
    public LayerMask playerLayer;
    public UnityAction onPlayerDeath;

    public bool playerDead;

    private void Awake() {
        if(instance == null)
            instance = this;
        else if(instance != null)
            Destroy(this);
        DontDestroyOnLoad(this);
        playerDead = false;
        onPlayerDeath += PlayerDeath;
    }

    public void SetCurrentCheckpoint(Checkpoint cp) {
        if(currentCheckpoint && currentCheckpoint != cp)
            Destroy(currentCheckpoint.gameObject);
        currentCheckpoint = cp;
        print("current checkpoint set to " + currentCheckpoint.cpCoordinates);
    }

    private void ResetSectionScene() {
        Scene arenaScene = SceneManager.GetSceneByName(arenaSceneName);
        foreach(GameObject obj in arenaScene.GetRootGameObjects()) {
            SectionController section = obj.GetComponent<SectionController>();
            if(section){
                if(section.GetType() == typeof(SectionController))
                    section.ResetSection();
                else
                    ((ArenaController)section).ResetSection();
            }
            else {
                AmmoPickUp ammo = obj.GetComponent<AmmoPickUp>();
                if(ammo)
                    Destroy(ammo.gameObject);
            }
        }
    }

    private void PlayerDeath() {
        Time.timeScale = 0.5f;
        // TODO: bring up death menu
        RespawnPlayer();
    }

    public void RespawnPlayer() {
        ResetSectionScene();
        Time.timeScale = 1;
        playerDead = false;
        player.GetComponent<PlayerController>().Respawn(currentCheckpoint);
    }

    public void DemoFinished() {
        // destroy all don't destroy on load gameobjects
        GameObject x = new GameObject();
        DontDestroyOnLoad(x);
        foreach(GameObject obj in x.scene.GetRootGameObjects())
            Destroy(obj);

        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene("MainScene");
    }
}
