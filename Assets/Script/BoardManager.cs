using System.Collections.Generic;
using Unity.Android.Gradle;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public GameObject[,] BoardData; //y x

    [SerializeField] private GameManager gameManager;
    [SerializeField] private BlockManager blockManager;
    [SerializeField] private GameObject cameraObject;

    private int speciesKind;
    private int[,,] levelData = new int[1, 5, 5];   //단계 y x
    private bool[,] matchedBlocks;                  //x y
    private int[] countMatchedBlock;       //x y

    //블럭 스왑
    public void TrySwap(Vector2Int _targetBlockPos, Vector2Int _dir)
    {
        int[,] virtualMap = new int[BoardData.GetLength(1), BoardData.GetLength(0)];
        int[,] matchData = new int[BoardData.GetLength(1), BoardData.GetLength(0)];

        Block selectBlock = BoardData[_targetBlockPos.y, _targetBlockPos.x].GetComponent<Block>();
        Block targetBlock = BoardData[_targetBlockPos.y + _dir.y, _targetBlockPos.x + _dir.x].GetComponent<Block>();

        for (int x = 0; x < virtualMap.GetLength(0); x++)
        {
            for (int y = 0; y < virtualMap.GetLength(1); y++)
            {
                virtualMap[x, y] = BoardData[y, x].GetComponent<Block>().species;
            }
        }

        (virtualMap[selectBlock.boardPos.x, selectBlock.boardPos.y], virtualMap[targetBlock.boardPos.x, targetBlock.boardPos.y])
        = (virtualMap[targetBlock.boardPos.x, targetBlock.boardPos.y], virtualMap[selectBlock.boardPos.x, selectBlock.boardPos.y]);

        bool matchResult = MatchChack(virtualMap);

        if (matchResult)
        {
            (BoardData[selectBlock.boardPos.x, selectBlock.boardPos.y], BoardData[targetBlock.boardPos.x, targetBlock.boardPos.y])
            = (BoardData[targetBlock.boardPos.x, targetBlock.boardPos.y], BoardData[selectBlock.boardPos.x, selectBlock.boardPos.y]);
        }

        gameManager.GetMatchResult(matchResult, selectBlock.gameObject, targetBlock.gameObject);
    }

    //매치 확인
    public bool MatchChack(int[,] _virtualMap)
    {
        int count;
        int prevShapes = -1;
        bool isMatched = false;

        matchedBlocks = new bool[BoardData.GetLength(1), BoardData.GetLength(0)];

        //xy 검사
        for (int y = 0; y < _virtualMap.GetLength(1); y++)
        {
            count = 1;  //새 행을 검사할때 마다 초기화
            prevShapes = -1;

            for (int x = 0; x < _virtualMap.GetLength(0); x++)
            {
                if (prevShapes == _virtualMap[x, y])
                {
                    count++;
                }

                else
                {
                    prevShapes = _virtualMap[x, y];
                    count = 1;
                }

                if (count >= 3)
                {
                    isMatched = true;
                    for (int i = 0; i < count; i++)
                    {
                        matchedBlocks[x - i, y] = true;
                    }
                }
            }
        }

        for (int x = 0; x < _virtualMap.GetLength(0); x++)
        {
            count = 0;  //새 열을 검사할때 마다 초기화
            prevShapes = -1;

            for (int y = 0; y < _virtualMap.GetLength(1); y++)
            {
                if (prevShapes == _virtualMap[x, y])
                {
                    count++;
                }

                else
                {
                    prevShapes = _virtualMap[x, y];
                    count = 1;
                }

                if (count >= 3)
                {
                    isMatched = true;
                    for (int i = 0; i < count; i++)
                    {
                        matchedBlocks[x, y - i] = true;
                    }
                }
            }
        }

        return isMatched;
    }

    public void DestroyBlock()
    {
        for (int x = 0; x < matchedBlocks.GetLength(0); x++)
        {
            for (int y = 0; y < matchedBlocks.GetLength(1); y++)
            {
                if (matchedBlocks[x,y])
                {
                    countMatchedBlock[x]++;
                    Destroy(BoardData[y, x]);
                    BoardData[y, x] = null;

                    Debug.Log("X = " + x + "  Y = " + y);

                    //파괴되었던 블럭 위의 블럭들에 얼마나 떨어져야하는지 저장
                    for (int upperY = y - 1; upperY >= 0; upperY--)
                    {
                        if (BoardData[upperY, x] == null)
                            continue;

                        Block block = BoardData[upperY, x].GetComponent<Block>();

                        if (block != null)
                        {
                            block.fall++;
                        }
                    }
                }
            }
        }

        gameManager.ChangeGameState(GameState.respawn);
    }

    //이 주석 아래는 수정 예정
    public void CreateMap(int[,,] _levelData)
    {
        BlockManager blcokManager = this.gameObject.GetComponent<BlockManager>();

        BoardData = new GameObject[_levelData.GetLength(1), _levelData.GetLength(2)];
        countMatchedBlock = new int[_levelData.GetLength(2)];

        for (int y = 0; y < _levelData.GetLength(1); y++)
        {
            for (int x = 0; x < _levelData.GetLength(2); x++)
            {
                BoardData[y, x] = blcokManager.CreateBlock(_levelData[0, y, x], x, y);

                if (speciesKind < _levelData[0, y, x])
                {
                    speciesKind = _levelData[0, y, x];
                }
            }
        }

        cameraObject.transform.position = new Vector3(_levelData.GetLength(2) / 2, -_levelData.GetLength(1) / 2, -1);
    }

    //블럭 리스폰
    public void BlockReSpawn()
    {
        for (int i = 0; i < countMatchedBlock.Length; i++)
        {
            for(int j = 0;  j < countMatchedBlock[i]; j++)
            {
                blockManager.CreateBlock(Random.Range(0, speciesKind), i, -j - 1, countMatchedBlock[i] + j + 1);
            }
        }
    }

    /*`
    //블럭 위치를 교환하였을때 3매치가 가능한지 확인하는 함수
    void CanMakeMatch()
    {
        GameObject[,] map = mapComponent.MapData;   
        int[,] mapData = new int[0,0];              //매치 확인을 위한 임시 변수

        for (int y = 0; y < map.GetLength(0); y++)
        {
            for (int x = 0; x < map.GetLength(1); x++)
            {
                GameObject[,] virtualMap;

                if (x > 0)
                {
                    virtualMap = map;
                    (virtualMap[y, x], virtualMap[y, x - 1]) = (virtualMap[y, x - 1], virtualMap[y, x]);
                    mapData = MatchChack(virtualMap);
                }

                if(x < map.GetLength(1))
                {
                    virtualMap = map;
                    (virtualMap[y, x], virtualMap[y, x + 1]) = (virtualMap[y, x + 1], virtualMap[y, x]);
                    mapData = MatchChack(virtualMap);
                }

                if (y > 0)
                {
                    virtualMap = map;
                    (virtualMap[y, x], virtualMap[y - 1, x]) = (virtualMap[y - 1, x], virtualMap[y, x]);
                    mapData = MatchChack(virtualMap);
                }

                if (y < map.GetLength(0))
                {
                    virtualMap = map;
                    (virtualMap[y, x], virtualMap[y + 1, x]) = (virtualMap[y + 1, x], virtualMap[y, x]);
                    mapData = MatchChack(virtualMap);
                }
            }
        }

        if(mapData.GetLength(0) == 0 || mapData.GetLength(1) == 0)
        {
            //터뜨릴 수 있으면 리턴
            return;
        }

        else
        {
            //가능한게 없으면 재배치
        }

        Debug.Log("ㅋㅋ");
    }
    */

    void Start()
    {
        // 모듈화 예정
        levelData = new int[1, 5, 5]{
            {
                { 0, 1, 2, 1, 0},
                { 1, 2, 0, 2, 3},
                { 3, 0, 3, 3, 1},
                { 3, 2, 0, 2 ,1},
                { 0, 3, 2, 1, 0}
            }
        };

        CreateMap(levelData);
    }
}