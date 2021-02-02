using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPanel : MonoBehaviour
{
    private DepartmentStoreGame game;
    private Animator animator;

    public void StartSign(DepartmentStoreGame _game)
    {
        game = _game;
        gameObject.SetActive(true);
    }

    public void StartClerk()
    {
        game.clerkPanel.StartAnimation();
    }

    public void StartGame()
    {
        HomeManager.Instance.flexPlaceManager.FlexPlaceGames[0].DepartmentMiniGameStart();
    }
}
