using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvent : MonoBehaviour
{   ///////////////////////////////////// MainMenu \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
    private MainMenu mainMenu;
    public void StartMenu() { mainMenu.SelectButton(); }
    public void StartGame() { mainMenu.StartGame(); }
    public void QuitGame() { mainMenu.Quit(); }
    
    
    
    ///////////////////////////////////// Player \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
}
