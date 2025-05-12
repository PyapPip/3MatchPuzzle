using System.Collections;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public int[,,] LevelData = new int[1, 5, 5];


    MapComponent mapComponent => gameObject.GetComponent<MapComponent>();

    public void ChangeBlock(GameObject block, int dir)
    {
        GameObject[,] mapData = mapComponent.MapData;
        int[,] matchData = mapComponent.MatchData;
        GameObject[,] virtualMap;

        int x = block.GetComponent<Block>().x;
        int y = block.GetComponent<Block>().y;

        virtualMap = mapData;
        GameObject originalTile = virtualMap[y, x];
        GameObject changeTile = null;

        //dir 1.left 2.right 3.down 4.up
        //바꾸러는 방향이 유효한지 확인
        switch (dir)
        {
            case 1:
                if (block.GetComponent<Block>().x >= 1)
                {
                    changeTile = virtualMap[y, x - 1];
                }
                break;
            case 2:
                if (block.GetComponent<Block>().x < mapData.GetLength(0))
                {
                    changeTile = virtualMap[y, x + 1];
                }
                break;
            case 3:
                if (block.GetComponent<Block>().y < mapData.GetLength(1))
                {
                    changeTile = virtualMap[y - 1, x];
                }
                break;
            case 4:
                if (block.GetComponent<Block>().y >= 1)
                {
                    changeTile = virtualMap[y, x + 1];
                }
                break;
        }

        if (changeTile == null)
            return;

        (originalTile, changeTile) = (changeTile, originalTile);
        MatchChack(virtualMap);

        //터질 수 있다면 자릴 바꾼 후 터뜨리기
        if(matchData[y,x] >= 1 || matchData[changeTile.GetComponent<Block>().y, changeTile.GetComponent<Block>().x] >= 1)
        {
            for (int i = 0; i < mapData.GetLength(0); i++)
            {
                for (int j = 0; j < mapData.GetLength(1); j++)
                {
                    if (matchData[i, j] > 0)
                    {
                        //폭발! 
                        return;
                    }
                }
            }
        }
        /*
        
        */
        //없다면 자릴 바꿨다 돌아오는 연출

        mapComponent.MatchDataReSet();
    }

    //
    void MatchChack(GameObject[,] arr)
    {
        int count = 0;
        int prevShapes = -1;
        GameObject[,] mapData = mapComponent.MapData;
        int[,] matchData = mapComponent.MatchData;

        //x 확인
        for (int y = 0; y < arr.GetLength(0); y++)
        {
            count = 0;

            for (int x = 0; x < arr.GetLength(1); x++)
            {
                Block tagetBlock = arr[y, x].GetComponent<Block>();
                if (tagetBlock.species == prevShapes)
                {
                    count++;
                    continue;
                }

                if (count >= 3)
                {
                    for (int i = 0; i < count; i++)
                    {
                        matchData[y, x - i] = 1;
                    }
                }
                prevShapes = tagetBlock.species;
            }

            if (count >= 3)
            {
                for (int i = 0; i < count; i++)
                {
                    matchData[y, arr.GetLength(1) - 1 - i] = 1;
                }
            }
        }

        //y 확인
        for (int x = 0; x < mapData.GetLength(1); x++)
        {
            for (int y = 0; y < mapData.GetLength(0); y++)
            {
                Block tagetBlock = mapData[y, x].GetComponent<Block>();
                if (tagetBlock.species == prevShapes)
                {
                    count++;
                    if (matchData[y,x] == -1)
                    {
                        matchData[y,x] = 0;
                    }
                    continue;
                }

                if (count >= 3)
                {
                    for (int i = 0; i < count; i++)
                    {
                        if (matchData[y - i, x] == 1)
                        {
                            matchData[y - i, x] = 3;
                        }
                        else
                        {
                            matchData[y - i, x] = 2;
                        }
                    }
                }
                prevShapes = tagetBlock.species;
            }

            if (count >= 3)
            {
                for (int i = 0; i < count; i++)
                {
                    if (matchData[arr.GetLength(0) - 1 - i, x] == 1)
                    {
                        matchData[arr.GetLength(0) - 1 - i, x] = 3;
                    }
                    else
                    {
                        matchData[arr.GetLength(0) - 1 - i, x] = 2;
                    }
                }
            }
        }

        return;
    }

    //블럭 위치를 교환하였을때 3매치가 가능한지 확인하는 함수
    void CanMakeMatch()
    {
        GameObject[,] map = mapComponent.MapData;
        int[,] mapData = mapComponent.MatchData;

        for (int y = 0; y < map.GetLength(0); y++)
        {
            for (int x = 0; x < map.GetLength(1); x++)
            {
                GameObject[,] virtualMap;

                if (x > 0)
                {
                    virtualMap = map;
                    (virtualMap[y, x], virtualMap[y, x - 1]) = (virtualMap[y, x - 1], virtualMap[y, x]);
                    MatchChack(virtualMap);
                }

                if(x < map.GetLength(1))
                {
                    virtualMap = map;
                    (virtualMap[y, x], virtualMap[y, x + 1]) = (virtualMap[y, x + 1], virtualMap[y, x]);
                    MatchChack(virtualMap);
                }

                if (y > 0)
                {
                    virtualMap = map;
                    (virtualMap[y, x], virtualMap[y - 1, x]) = (virtualMap[y - 1, x], virtualMap[y, x]);
                    MatchChack(virtualMap);
                }

                if (y < map.GetLength(0))
                {
                    virtualMap = map;
                    (virtualMap[y, x], virtualMap[y + 1, x]) = (virtualMap[y + 1, x], virtualMap[y, x]);
                    MatchChack(virtualMap);
                }
            }
        }

        //터뜨릴 수 없다면 재배치
        //터뜨릴 수 없는지 확인하는 코드 필요
        Debug.Log("ㅋㅋ");
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


/*
 * CanMakeMatch 함수를 이용해서 
 * 
 * 
 * 
 * 
*/