using System;
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
    destroy,
    fall
}

public class GameManager : MonoBehaviour
{
    public MouseManager mouseManager;
    public MapManager mapManager;
 
    private GameState gameState;

    private Vector2Int selecBlockPos = new Vector2Int(-1, -1);      //초기값

    /// <summary>
    /// 0.wait  1.select  2.move  3.destroy  4.fail
    /// </summary>
    public void ChangeGameState(GameState _state)
    {
        gameState = _state;
    }

    public void OnClick(Vector2Int _pos)
    {
        if (gameState != GameState.wait && gameState != GameState.select)
            return;

        switch (gameState)
        {
            case GameState.wait:
                {
                    selecBlockPos = _pos;
                    ChangeGameState(GameState.select);
                    return;
                }
            case GameState.select:
                {
                    Vector2Int diff = _pos - selecBlockPos;

                    //인접한 블럭 클릭 시(미구현)
                    if (Mathf.Abs(diff.x) + Mathf.Abs(diff.y) == 1)
                    {
                        mapManager.ChangeBlock(selecBlockPos, diff);
                    }

                    //먼 거리 -> 재 선택
                    else if (Mathf.Abs(diff.x) + Mathf.Abs(diff.y) > 1)
                    {
                        selecBlockPos = _pos;
                    }

                    //같은 칸 선택 -> 선택 취소
                    else
                    {
                        selecBlockPos = new Vector2Int(-1, -1);
                        ChangeGameState(GameState.wait);
                    }

                    return;
                }
        }
    }

    void Update()
    {
        switch (gameState)
        {
            case GameState.wait:
            case GameState.select:
                {
                    mouseManager.BlcokSelect();
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
