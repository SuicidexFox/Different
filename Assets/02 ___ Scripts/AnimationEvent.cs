using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AnimationEvent : MonoBehaviour
{   ///////////////////////////////////// MainMenu \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
    public static AnimationEvent animationsEvent;
    private MainMenu mainMenu;
    public void SelectButton() { mainMenu.SelectButton(); }
    public void StartGame() { mainMenu.StartGame(); }
    public void QuitGame() { mainMenu.Quit(); }
    
    ///////////////////////////////////// Pause \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
    public void Scenenwechsel() { mainMenu.BackMainMenu(); }
    
    
    
    
    
    public void CloseDialogUI() { GameManager.instance.AnimationEventCloseDialogUI(); }
    public void SelectQuestButton() { GameManager.instance.AnimationSelectButtonQuestUI(); }
    public void CloseQuestUI() { GameManager.instance.AnimationEventCloseQuestUI(); }
}
