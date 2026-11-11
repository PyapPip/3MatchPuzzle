using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public GameObject[,] BoardData; //y x
    public int[,,] levelData = 
        new int[1, 5, 5]{
            {
                { 0, 1, 2, 1, 0},
                { 1, 2, 0, 2, 3},
                { 3, 0, 3, 3, 1},
                { 3, 2, 0, 2 ,1},
                { 0, 3, 2, 1, 0}
            }
        };

    [SerializeField] private GameManager gameManager;
    [SerializeField] private BlockManager blockManager;
    [SerializeField] private GameObject cameraObject;
    [SerializeField] private FXManager fxManager;

    private int mapWidth;       //x 길이
    private int mapHeight;      //y 길이

    private bool[,] matchedBlocks;                  //y x
    private int speciesKind;        
    private int[] countMatchedBlock;                //해당하는 x좌표에 몇개의 블럭이 매치되었는지 저장하는 배열

    //BoardData에서 게임오브젝트 받아오기
    public GameObject GetBlock(Vector2Int _pos)
    {
        return BoardData[_pos.y, _pos.x]; 
    }

    //사이클이 끝날때마다 초기화
    public void InitBoard()
    {
        //죽은 보드인지 확인 후 처리하는 함수 있을 예정

        countMatchedBlock = null;
        matchedBlocks = null;   
        countMatchedBlock = null;

        for (int y = 0; y < mapHeight; y++)
        {
            for(int x = 0; x < mapWidth; x++)
            {
                BoardData[y, x].GetComponent<Block>().fall = 0;
                BoardData[y, x].GetComponent<Block>().isMoved = false;
            }
        }

    }

    //스왑 시도
    public bool TrySwap(Vector2Int _selectBlockPos, Vector2Int _dir)
    {
        int[,] virtualMap = new int[mapHeight, mapWidth];
        int[,] matchData = new int[mapHeight, mapWidth];
                           
        Vector2Int targetBlock = _selectBlockPos + _dir;

        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                virtualMap[y, x] = BoardData[y, x].GetComponent<Block>().species;                       
            }
        }

        (virtualMap[_selectBlockPos.y, _selectBlockPos.x], virtualMap[targetBlock.y, targetBlock.x])
        = (virtualMap[targetBlock.y, targetBlock.x], virtualMap[_selectBlockPos.y, _selectBlockPos.x]);

        bool matchResult = MatchCheck(virtualMap);

        return matchResult;
    }

    //실제 블럭 스왑
    public void BlockSwap(Vector2Int _selectBlockPos, Vector2Int _dir)
    {
        Vector2Int targetBlock = _selectBlockPos + _dir;

        (BoardData[_selectBlockPos.y, _selectBlockPos.x], BoardData[targetBlock.y, targetBlock.x])
            = (BoardData[targetBlock.y, targetBlock.x], BoardData[_selectBlockPos.y, _selectBlockPos.x]);
    }

    //매치 확인
    public bool MatchCheck(int[,] _virtualMap)
    {
        int count;
        int prevShapes = -1;
        bool isMatched = false;

        matchedBlocks = new bool[mapHeight, mapWidth];

        //가로 검사
        for (int y = 0; y < mapHeight; y++)
        {
            count = 1;  //새 행을 검사할때 마다 초기화
            prevShapes = -1;

            for (int x = 0; x < mapWidth; x++)
            {
                if (prevShapes == _virtualMap[y, x])
                {
                    count++;
                }

                else
                {
                    prevShapes = _virtualMap[y, x];
                    count = 1;
                }

                if (count >= 3)
                {
                    isMatched = true;
                    for (int i = 0; i < count; i++)
                    {
                        matchedBlocks[y, x - i] = true;
                    }
                }
            }
        }

        //새로 검사
        for (int x = 0; x < mapWidth; x++)
        {
            count = 0;  //새 열을 검사할때 마다 초기화
            prevShapes = -1;

            for (int y = 0; y < mapHeight; y++)
            {
                if (prevShapes == _virtualMap[y, x])
                {
                    count++;
                }

                else
                {
                    prevShapes = _virtualMap[y, x];
                    count = 1;
                }

                if (count >= 3)
                {
                    isMatched = true;
                    for (int i = 0; i < count; i++)
                    {
                        matchedBlocks[y - i, x] = true;
                    }
                }
            }
        }

        return isMatched;
    }

    //매치된 블럭 상호작용, 매치된 블럭에 파괴 애니메이션을 재생시키고, 위의 블럭들이 얼마나 떨어져야하는지 계산
    public void ResolveMatchedBlock()
    {
        countMatchedBlock = new int[mapWidth]; 

        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                if (matchedBlocks[y,x])
                {
                    countMatchedBlock[x]++;
                    GameObject targetBlock = BoardData[y, x];
                    blockManager.PlayDestroy(targetBlock);
                    fxManager.PlayMatchEffect(targetBlock.transform.position);

                    //파괴되었던 블럭 위의 블럭들에 얼마나 떨어져야하는지 저장

                    AddFallAbove(x, y);
                }
            }
        }
    }

    //매치된 블럭을 파괴. 파괴 애니매이션이 끝나고 호출 됨
    public void DestroyMatchedBlock()
    {
        for(int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                if (!matchedBlocks[y, x])
                    continue;

                Destroy(BoardData[y, x]);
                BoardData[y, x] = null;
            }
        }
    }

    //해당하는 블럭이 파괴되었을때 위의 블럭들이 얼마나 떨어져야하는지 계산
    private void AddFallAbove(int _boardX, int _upperY)
    {
        for (int y = _upperY - 1; y >= 0; y--)
        {
            if (BoardData[y, _boardX] == null)
                continue;

            Block block = BoardData[y, _boardX].GetComponent<Block>();

            if (block != null)
            {
                block.fall++;
            }
        }
    }

    //매치된 블럭을 제거한 후, 새로운 블럭이 생성
    public List<GameObject> BlockReSpawn()
    {
        List<GameObject> fallBlocks = new List<GameObject>();
        for (int x = 0; x < countMatchedBlock.Length; x++)
        {
            for (int y = 0; y < countMatchedBlock[x]; y++)
            {
                //조각 종류 랜덤, 보드상 좌표(x, fall값이 더해질것을 고려한 y), fall값, 실제 좌표
                GameObject newBlock =
                    blockManager.CreateBlock(Random.Range(0, speciesKind + 1), new Vector2Int(x, -y - 1), countMatchedBlock[x], new Vector2(x, y + 1));

                fallBlocks.Add(newBlock);
                newBlock.GetComponent<Block>().isMoved = true;
            }
        }

        return ResolveFall(fallBlocks);
    }

    //생성된 블럭과 기존 블럭들을 fall값에 맞게 실제 좌표로 떨어뜨리고, BoardData를 갱신
    public List<GameObject> ResolveFall(List<GameObject> _fallBlockList)
    {   
        GameObject[,] virtualMap = new GameObject[mapHeight, mapWidth];

        for (int y = mapHeight - 1; y >= 0; y--)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                if (BoardData[y, x] == null)
                    continue;

                Block block = BoardData[y, x].GetComponent<Block>();

                if (block.fall > 0)
                {
                    _fallBlockList.Add(block.gameObject);
                }
                else
                {
                    virtualMap[y, x] = BoardData[y, x]; 
                }
            }
        }

        for (int i = _fallBlockList.Count - 1; i >= 0; i--)
        {
            GameObject block = _fallBlockList[i];

            if (block == null)
                continue;

            Block blockComponent = block.GetComponent<Block>();

            if (blockComponent != null && blockComponent.fall > 0)
            {
                virtualMap[blockComponent.boardPos.y + blockComponent.fall, blockComponent.boardPos.x] = block;
                blockComponent.boardPos.y += blockComponent.fall;
                blockComponent.isMoved = true;
            }
        }

        BoardData = virtualMap;

        return _fallBlockList;
    }

    //게임 상태가 check일때 호출되는 함수. 현재 보드에서 매치가 가능한지 확인
    public void NowBoardCheck()
    {
        int[,] virtualMap = new int[mapHeight, mapWidth];

        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                virtualMap[y, x] = BoardData[y, x].GetComponent<Block>().species;
            }
        }

        InitBoard();

        bool matchResult = MatchCheck(virtualMap);

        if(matchResult)
        {
            gameManager.ChangeGameState(GameState.destroy);
            return;
        }

        else
        {
            gameManager.ChangeGameState(GameState.wait);
        }
    }


    //이 주석 아래는 수정 예정
    public void CreateMap(int[,,] _levelData)
    {
        mapHeight = _levelData.GetLength(1);
        mapWidth = _levelData.GetLength(2);

        BoardData = new GameObject[mapHeight, mapWidth];

        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                BoardData[y, x] = blockManager.CreateBlock(_levelData[0, y, x], x, y);

                if (speciesKind < _levelData[0, y, x])
                {
                    speciesKind = _levelData[0, y, x];
                }
            }
        }

        cameraObject.transform.position = new Vector3(_levelData.GetLength(2) / 2, -_levelData.GetLength(1) / 2, -1);
    }


    private bool DeadBoardCheck()
    {
        List<Vector2Int> needCheckBlockList = new List<Vector2Int>();

        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                if(BoardData[y, x].GetComponent<Block>().isMoved)
                {
                    needCheckBlockList.Add(new Vector2Int(x, y));
                }
            }
        }

        for (int i = 0; i < needCheckBlockList.Count; i++)
        {
            Vector2Int blockPos = needCheckBlockList[i];
            // 필요한 작업 수행
            if (blockPos.x > 0 && TrySwap(blockPos, Vector2Int.left))
            {
                return false;
            }
            if (blockPos.x < mapWidth - 1 && TrySwap(blockPos, Vector2Int.right))
            {
                return false;
            }
            if (blockPos.y > 0 && TrySwap(blockPos, Vector2Int.down))
            {
                return false;
            }
            if (blockPos.y < mapHeight - 1 && TrySwap(blockPos, Vector2Int.up))
            {
                return false;
            }
        }

        return true;
    }


    /*
    void AddBoardData(GameObject _newBlock, Vector2Int _boardPos)
    {
        if (BoardData[_boardPos.y, _boardPos.x] == null)
        {
            BoardData[_boardPos.y, _boardPos.x] = _newBlock;
        }
    }
    */
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