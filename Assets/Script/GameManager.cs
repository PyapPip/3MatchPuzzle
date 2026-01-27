using UnityEngine;
using UnityEngine.PlayerLoop;

/// <summary>
/// 0.wait  1.select  2.move  3.fail
/// </summary>
public enum GameState
{
    wait,
    select,
    move,
    fall
}

public class GameManager : MonoBehaviour
{
    public MouseManager mouseManager;
    public MapManager mapManager;
 
    private GameState gameState;

    /// <summary>
    /// 0.wait  1.select  2.move  3.fail
    /// </summary>
    public void ChangeGameState(int _state)
    {
        gameState = (GameState)_state;
    }

    void Update()
    {
        switch (gameState)
        {
            case GameState.wait:
                {
                    mouseManager.BlcokSelect();
                    return;
                }
            case GameState.select:
                {
                    mouseManager.BlockSwap();
                    return;
                }
            case GameState.move:
                {
                    //mapManager.
                    return;
                }
            case GameState.fall:
                {
                    return;
                }
        }
    }
}
