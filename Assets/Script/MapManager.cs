using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public GameObject[] Shapes = new GameObject[5]; //5가지 종류의 블럭을 담아둔 배열
    public GameManager gameManager;
    public GameObject Camera;

    private GameObject[,] MapData;
    private List<Vector2Int> NeedFillPos;
    private int SpeciesKind;
    private int[,,] LevelData = new int[1, 5, 5];

    //블럭 스왑
    public void BlockSwap(Vector2Int _targetBlockPos, Vector2Int _dir)
    {
        
        int[,] virtualMap = new int[MapData.GetLength(1), MapData.GetLength(0)];
        int[,] matchData = new int[MapData.GetLength(1), MapData.GetLength(0)];

        Block selectBlock = MapData[_targetBlockPos.y, _targetBlockPos.x].GetComponent<Block>();
        Block targetBlock = MapData[_targetBlockPos.y + _dir.y, _targetBlockPos.x + _dir.x].GetComponent<Block>();

        for (int x = 0; x < virtualMap.GetLength(0); x++)
        {
            for (int y = 0; y < virtualMap.GetLength(1); y++)
            {
                virtualMap[x, y] = MapData[y, x].GetComponent<Block>().species;
            }
        }

        (virtualMap[selectBlock.boardPos.x, selectBlock.boardPos.y], virtualMap[targetBlock.boardPos.x, targetBlock.boardPos.y])
        = (virtualMap[targetBlock.boardPos.x, targetBlock.boardPos.y], virtualMap[selectBlock.boardPos.x, selectBlock.boardPos.y]);

        NeedFillPos = MatchChack(virtualMap);

        //매치 되지 않았다면
        if(NeedFillPos.Count == 0)
        {
            gameManager.MatchResult(false, _targetBlockPos, _dir);
        }

        else
        {
            gameManager.MatchResult(true, _targetBlockPos, _dir);
        }
    }

    //매치 확인
    List<Vector2Int> MatchChack(int[,] _virtualMap)
    {
        int count = 1;
        int prevShapes = -1;

        List<Vector2Int> matchedBlocks = new List<Vector2Int>();

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
                    count = 0;
                }

                if (count > 2)
                {
                    for (int i = 0; i < count; i++)
                    {
                        matchedBlocks.Add(new Vector2Int(x - i, y));
                    }
                }
            }
        }

        for (int x = 0; x < _virtualMap.GetLength(0); x++)
        {
            count = 1;  //새 행을 검사할때 마다 초기화
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
                    count = 0;
                }

                if (count > 2)
                {
                    for (int i = 0; i < count; i++)
                    {
                        matchedBlocks.Add(new Vector2Int(x, y - i));
                    }
                }
            }
        }
        return matchedBlocks;
    }

    //이 주석 아래는 수정 예정
    public void CreateMap(int[,,] _levelData)
    {
        BlockManager blcokManager = this.gameObject.GetComponent<BlockManager>();

        MapData = new GameObject[_levelData.GetLength(1), _levelData.GetLength(2)];

        for (int y = 0; y < _levelData.GetLength(1); y++)
        {
            for (int x = 0; x < _levelData.GetLength(2); x++)
            {
                MapData[y, x] = blcokManager.CreateBlock(_levelData[0, y, x], x, y);

                if (SpeciesKind < _levelData[0, y, x])
                {
                    SpeciesKind = _levelData[0, y, x];
                }
            }
        }

        Camera.transform.position = new Vector3(_levelData.GetLength(2) / 2, -_levelData.GetLength(1) / 2, -1);
    }

    //블럭 리스폰
    public void BlockReSpawn()
    {
        int s = UnityEngine.Random.Range(0, SpeciesKind);

        //파괴되었던 블럭 위의 블럭들에 얼마나 떨어져야하는지 저장
        for (int i = 0; i < NeedFillPos.Count; i++)
        {
            for (int y = NeedFillPos[i].y; y >= 0; y--)
            {
                MapData[y, NeedFillPos[i].x].GetComponent<Block>().fall++;
            }
        }

        for (int y = 0; y < MapData.GetLength(0); y++)
        {
            for (int x = 0; x < MapData.GetLength(1); x++)
            {
                if (MapData[y, x].GetComponent<Block>().fall != 0)
                {
                    //애니메이션 재생
                }
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

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }                                                                                                

    void Start()
    {
        // 모듈화 예정
        LevelData = new int[1, 5, 5]{
            {
                { 0, 1, 2, 1, 0},
                { 1, 2, 0, 2, 3},
                { 3, 0, 3, 3, 1},
                { 3, 2, 0, 2 ,1},
                { 0, 3, 2, 1, 0}
            }
        };

        
    }
}