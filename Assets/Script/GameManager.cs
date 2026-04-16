using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 0.wait 1.select 2.move 3.destroy 4.respawn 5.fall 6.check
/// </summary>
public enum GameState
{
    wait,
    select,
    move,
    destroy,
    respawn,
    fall,
    check
}

public class GameManager : MonoBehaviour
{
    [SerializeField] private MouseManager mouseManager;
    [SerializeField] private BlockManager blockManager;
    [SerializeField] private BoardManager boardManager;

    [SerializeField] private GameState gameState;

    private Vector2Int SelectBlockPos = new Vector2Int(-1, -1);      //초기값
    private bool isMatched;

    /// <summary>
    ///  wait, select, move, destroy, fall, respawn, check
    /// </summary>
    public void ChangeGameState(GameState _state)
    {
        gameState = _state;

        //Debug.Log(gameState);

        switch (gameState)
        {
            case GameState.destroy:
                {
                    boardManager.DestroyBlock();
                    break;
                }
            case GameState.respawn:
                {
                    blockManager.playFall(boardManager.BlockReSpawn());
                    break;
                }
            case GameState.check:
                {

                    break;
                }
        }
    }
                                      
    public void OnClick(Vector2Int _pos)
    {
        if (gameState != GameState.wait && gameState != GameState.select)
        {
            Debug.Log("OnClick 예외");
            return; 
        }

        switch (gameState)
        {
            case GameState.wait:
                {
                    SelectBlockPos = _pos;
                    ChangeGameState(GameState.select);
                    return;
                }
            case GameState.select:
                {
                    Vector2Int diff = _pos - SelectBlockPos;

                    //인접한 블럭 클릭 시
                    if (Mathf.Abs(diff.x) + Mathf.Abs(diff.y) == 1)
                    {
                        boardManager.TrySwap(SelectBlockPos, diff);
                    }

                    //먼 거리 -> 재 선택
                    else if (Mathf.Abs(diff.x) + Mathf.Abs(diff.y) > 1)
                    {
                        SelectBlockPos = _pos;
                    }

                    //같은 칸 선택 -> 선택 취소
                    else
                    {
                        SelectBlockPos = new Vector2Int(-1, -1);
                        ChangeGameState(GameState.wait);
                    }

                    return;
                }
        }
    }

    public void GetMatchResult(bool _result, GameObject _block1, GameObject _block2)
    {
        ChangeGameState(GameState.move);
        isMatched = _result;

        if (isMatched)
        {
            blockManager.playSwap(_block1, _block2);
        }
        else
        {
            blockManager.playSnapBack(_block1, _block2);
        }
    }

    public void MoveEnd()
    {
        if(gameState == GameState.move)
        {
            if (isMatched)
            {
                ChangeGameState(GameState.destroy);
            }
            else
            {
                ChangeGameState(GameState.select);
            }
        }

        if (gameState == GameState.fall)
        {
            ChangeGameState(GameState.check);
        }
    }

    public void CycleEndInit()
    {
        ChangeGameState(GameState.wait);
        SelectBlockPos = new Vector2Int(-1, -1);
        isMatched = false;
    }

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }
}