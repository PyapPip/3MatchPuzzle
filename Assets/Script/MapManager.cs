using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public int[,,] LevelData = new int[1, 5, 5];
    public List<Vector2Int> NeedFillPos;

    MapComponent mapComponent => gameObject.GetComponent<MapComponent>();

    public void ChangeBlock(GameObject _block, int _dir)
    {
        GameObject[,] mapData = mapComponent.MapData;
        int[,] matchData = mapComponent.MatchData;
        GameObject[,] virtualMap = mapData;                       //비교를 위한 맵 데이터의 복제
        GameObject originalBlock = _block;
        GameObject changeBlock = null;

        int block_x = originalBlock.GetComponent<Block>().x;
        int block_y = originalBlock.GetComponent<Block>().y;
        int[] nowPos = new int[2] { block_x, block_y };
        int[] targetPos = new int[2];

        //dir 1.left 2.right 3.up 4.dawn
        //바꾸러는 방향이 유효한지 확인
        switch (_dir)
        {
            case 1:
                if (_block.GetComponent<Block>().x > 0)
                {
                    changeBlock = virtualMap[block_y, block_x - 1];
                    targetPos = new int[2] { block_x - 1, block_y };
                }
                break;
            case 2:
                if (_block.GetComponent<Block>().x < mapData.GetLength(1))
                {
                    changeBlock = virtualMap[block_y, block_x + 1];
                    targetPos = new int[2] { block_x + 1, block_y };
                }
                break;
            case 3:
                if (_block.GetComponent<Block>().y > 0)
                {
                    changeBlock = virtualMap[block_y - 1, block_x];
                    targetPos = new int[2] { block_x, block_y - 1 };
                }
                break;
            case 4:
                if (_block.GetComponent<Block>().y < mapData.GetLength(0))
                {
                    changeBlock = virtualMap[block_y + 1, block_x];
                    targetPos = new int[2] { block_x, block_y + 1 };
                }
                break;
        }


        Debug.Log("now x: " + nowPos[0] + " y: " + nowPos[1]);
        Debug.Log("target x: " + targetPos[0] + " y: " + targetPos[1]);

        //if (changeBlock == null)
        //    return;
        
        virtualMap[block_y, block_x] = changeBlock;
        virtualMap[targetPos[1], targetPos[0]] = originalBlock; 
        (originalBlock, changeBlock) = (changeBlock, originalBlock);
        matchData = MatchChack(virtualMap);

        //자리를 바꾸었을때 매치된다면
        if (matchData[block_y, block_x] >= 1 || matchData[targetPos[1], targetPos[0]] >= 1)
        {
            originalBlock.gameObject.GetComponent<BlockMove>().MoveStart(1, nowPos);
            changeBlock.gameObject.GetComponent<BlockMove>().MoveStart(1, targetPos);

            //폭발
            for (int y = 0; y < mapData.GetLength(0); y++)
            {
                for (int x = 0; x < mapData.GetLength(1); x++)
                {
                    if (matchData[y, x] > 0)
                    {
                        Destroy(mapData[y, x]);
                        mapData[y, x] = null;
                        for (int i = y; i >= 0; i--)
                        {
                            mapData[i + 1, x] = mapData[i, x];
                            mapData[i, x] = null;
                        }
                        NeedFillPos.Add(new Vector2Int(x, y));
                        return;
                    }
                }
            }

            BlockReSpawn();
        }

        //없다면 자릴 바꿨다 돌아오는 연출
        else
        {
            (originalBlock, changeBlock) = (changeBlock, originalBlock);
            originalBlock.gameObject.GetComponent<BlockMove>().MoveStart(0, targetPos);
            changeBlock.gameObject.GetComponent<BlockMove>().MoveStart(0, nowPos);
        }

        //mapComponent.DataReSet();
    }
        
    //
    int[,] MatchChack(GameObject[,] _virtualMap)
    {
        int count = 0;                                  //검사중 같은 블럭이 있다면 ++
        int prevShapes = -1;
        GameObject[,] mapData = mapComponent.MapData;
        int[,] matchData = mapComponent.MatchData;

        //x 확인
        for (int y = 0; y < _virtualMap.GetLength(0); y++)
        {
            count = 0;  //새 행을 검사할때마다 초기화

            for (int x = 0; x < _virtualMap.GetLength(1); x++)
            {
                Block tagetBlock = _virtualMap[y, x].GetComponent<Block>(); //검사하는 좌표의 블럭의 정보를 받음
                if (tagetBlock.species == prevShapes)                       //해당 블럭의 모양과 이전 모양이 같다면
                {
                    count++;                                                //카운트
                    Debug.Log("x: " + x + " y: " + y);
                }

                else                                                        //같지 않으면 카운트 초기화
                {
                    count = 0;
                }

                if (count >= 2)                                             //아닐때 카운트가 3 이상이라면
                {
                    for (int i = 0; i <= count; i++)
                    {
                        matchData[y, x - i] = 1;                            //해당하는 블럭 좌표에 x 매칭임을 표시
                    }
                }

                prevShapes = tagetBlock.species;
            }
        }

        //y 확인
        for (int x = 0; x < mapData.GetLength(1); x++)
        {
            count = 0;                                                      //새 열을 검사할때마다 초기화

            for (int y = 0; y < mapData.GetLength(0); y++)
            {
                Block tagetBlock = mapData[y, x].GetComponent<Block>();
                if (tagetBlock.species == prevShapes)
                {
                    count++;
                }

                else
                {
                    count = 0;
                }

                if (matchData[y, x] == -1)                              //x,y축 검사가 끝난 블럭임을 표시
                {
                    matchData[y, x] = 0;
                }

                if (count >= 2)                                         
                {
                    for (int i = 0; i <= count; i++)
                    {
                        if (matchData[y - i, x] == 1)                   //x 축 검사에서도 매치가 되었다면
                        {
                            matchData[y - i, x] = 3;                    //x,y 둘 다 해당됨을 표시
                        }
                        else
                        {
                            matchData[y - i, x] = 2;                    //y축 매치가 된 블럭임을 표시
                        }
                    }
                }
                prevShapes = tagetBlock.species;
            }
        }

        return matchData;
    }
    

    //생성하는 x좌표에 맞춰 여러개를 위에 쌓는식으로 생성해주면 끝 아님?
    //파괴하면서 y좌표의 이동이 일어날부분을 미리 바꿔야하지 않을까?
    //파괴한 블럭의 위 블럭들의 y좌표를 1씩 내려주면 된다.
    //블럭이 떨어지는 연출은 유니티 자체의 물리를 이용하면 되는거 아님?
    public void BlockReSpawn()
    {
        GameObject[,] mapData = mapComponent.MapData;
        int[,] matchData = mapComponent.MatchData;

        int s = UnityEngine.Random.Range(0, mapComponent.SpeciesKind);

        //파괴되었던 블럭 위의 블럭들에 얼마나 떨어져야하는지 저장
        for(int i = 0; i < NeedFillPos.Count; i++)
        {
            for (int y = NeedFillPos[i].y; y >= 0; y--)
            {
                mapData[y, NeedFillPos[i].x].GetComponent<Block>().fall++;
            }
        }

        for (int y = 0; y < mapData.GetLength(0); y++)
        {
            for(int x = 0; x < mapData.GetLength(1); x++)
            {
                if (mapData[y,x].GetComponent<Block>().fall != 0)
                {
                    //애니메이션 재생
                }
            }
        }
    }

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

        mapComponent.CreateMap(LevelData);
    }
}