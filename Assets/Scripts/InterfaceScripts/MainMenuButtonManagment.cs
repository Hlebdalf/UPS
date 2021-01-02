using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtonManagment : MonoBehaviour {
    private Animator GodAnimator;
    
    public GameObject God;

    private void Start() {
        GodAnimator = God.GetComponent <Animator> ();   
    }

    public void StartLoadMenu() {
        GodAnimator.SetFloat("Reverse", 1);
        GodAnimator.Play("LoadMenuAnimation");
    }

    public void StartExitMenu() {
        GodAnimator.SetFloat("Reverse", 1);
        GodAnimator.Play("ConfirmAnimation");
    }
    public void ExitGame() {
        Application.Quit();
    }

    public void BackFromLoad() {
        GodAnimator.SetFloat("Reverse", -1);
        GodAnimator.Play("LoadMenuAnimation");
        
    }

    public void CancelExit() {
        GodAnimator.SetFloat("Reverse", -1);
        GodAnimator.Play("ConfirmAnimation");
    }

    public void StartGame() {
        SceneManager.LoadScene("GameField");
    }
}
