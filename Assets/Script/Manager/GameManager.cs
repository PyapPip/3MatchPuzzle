using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 0.wait 1.select 2.move 3.destroy 4.respawn 5.fall 6.check
/// </summary>
public enum GameState
{
    wait,
    select,
    trySwap,
    destroy,
    settle,
    check,
    debug
}

public class GameManager : MonoBehaviour
{
    [SerializeField] private MouseManager mouseManager;
    [SerializeField] private BlockManager blockManager;
    [SerializeField] private BoardManager boardManager;
    [SerializeField] private FXManager    fxManager;

    [SerializeField] private GameState gameState;

    private Vector2Int SelectBlockPos = new Vector2Int(-1, -1);      //초기값
    private bool isMatched;

    /// <summary>
    ///  wait, select, move, destroy, settle, check
    /// </summary>
    public void ChangeGameState(GameState _state)
    {
        gameState = _state;

        //Debug.Log(gameState);

        switch (gameState)
        {
            case GameState.wait:
                {
                    CycleEndInit();
                    boardManager.BoardData.GetLength(0);
                    break;
                }
            case GameState.destroy:
                {
                    boardManager.ResolveMatchedBlock();
                    break;
                }
            case GameState.settle:
                {
                    /*디버그용
                    {
                        ChangeGameState(GameState.debug);

                        for (int i = 0; i < fallBlockList.Count; i++)
                        {
                            Block block = fallBlockList[i].GetComponent<Block>();
                            if (boardManager.BoardData[block.boardPos.y, block.boardPos.x] == null)
                            {
                                Debug.Log("Pos" + block.boardPos + " BlockSpecies:" + block.species + " BoardData:null");
                            }
                            else
                            {
                                Debug.Log("Pos" + block.boardPos + " BlockSpecies:" + block.species + " BoardData:"
                                    + boardManager.BoardData[block.boardPos.y, block.boardPos.x].GetComponent<Block>().species);
                            }

                        }

                        break;
                    }
                    //*/

                    List<GameObject> fallBlockList = boardManager.BlockReSpawn();
                    blockManager.PlayFall(fallBlockList);
                    break;
                }
            case GameState.check:
                {
                    boardManager.NowBoardCheck();
                    break;
                }
            case GameState.debug:
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
        ChangeGameState(GameState.trySwap);
        isMatched = _result;

        if (isMatched)
        {
            blockManager.PlaySwap(_block1, _block2);
        }
        else
        {
            blockManager.PlaySnapBack(_block1, _block2);
        }
    }

    public void MoveEnd()
    {
        if(gameState == GameState.trySwap)
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

        else if(gameState == GameState.destroy)
        {
            boardManager.DestroyMatchedBlock();
            ChangeGameState(GameState.settle);
        }

        else if (gameState == GameState.settle)
        {
            ChangeGameState(GameState.check);
        }
    }

    public void CycleEndInit()
    {
        SelectBlockPos = new Vector2Int(-1, -1);
        isMatched = false;
        boardManager.InitBoard();
        fxManager.countActiveEffect = 0;
    }

    public void GameSet()
    {
        for (int fxCount = 0; fxCount < 10; fxCount++)
        {
            fxManager.CreateMatchEffect();
        }

        boardManager.CreateMap(boardManager.levelData);
    }

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }

    private void Start()
    {
        GameSet();
        ChangeGameState(GameState.wait);
    }
}