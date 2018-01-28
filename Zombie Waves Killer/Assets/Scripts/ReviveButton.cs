using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Advertisements;

public class ReviveButton : MonoBehaviour {

    public static bool isReviveButtonPressed = false;

    void Start()
    {
        Advertisement.Initialize("1679728");
        isReviveButtonPressed = false;
    }

    public void StartRevive()
    {
        ShowOptions so = new ShowOptions();
        so.resultCallback = Revive;
        if (Advertisement.IsReady())
        {
            Advertisement.Show("rewardedVideo", so);
        }
    }

    private void Revive(ShowResult showResult)
    {
        if (showResult == ShowResult.Finished)
        {
            isReviveButtonPressed = true;
            ZombieController.attackDistanceThreshold = 1.5f;
            PlayerController.IsPlayerDead = false;
        }
        else
        {
            isReviveButtonPressed = false;
            ZombieController.attackDistanceThreshold = 0f;
            PlayerController.IsPlayerDead = true;
        }
    }

    /*private void Revive(ShowResult showResult)
    {
        if (showResult == ShowResult.Finished)
        {
            isReviveButtonPressed = true;
            reviveUI.SetActive(false);
            playerPrefab.SetActive(true);
            fadeImage.SetActive(false);
            buttonsControlUI.SetActive(true);
        }
        else
        {
            gameOverUI.SetActive(true);
            reviveUI.SetActive(false);
            fadeImage.SetActive(true);
            buttonsControlUI.SetActive(false);
        }
    }*/
}
