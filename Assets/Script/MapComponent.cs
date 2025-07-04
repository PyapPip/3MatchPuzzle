﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapComponent : MonoBehaviour
{
    public GameObject[,] MapData;
    public GameObject[] Shapes = new GameObject[5];
    /// <summary>
    /// -1 = 검사되지 않음, 0 = 검사됨, 1 = x축 매치, 2 = y축 매치, 3 = x,y 매치
    /// </summary> 
    public int[,] MatchData;

    private int[] clearedBlockCount;
    private int SpeciesKind = 0;

    public void CreateMap(int[,,] levelData)
    {

        MapData = new GameObject[levelData.GetLength(1), levelData.GetLength(2)];
        MatchData = new int[levelData.GetLength(1), levelData.GetLength(2)];
        clearedBlockCount = new int[levelData.GetLength(2)];
        for (int y = 0; y < levelData.GetLength(1); y++)
        {
            for (int x = 0; x < levelData.GetLength(2); x++)
            {
                CreateBlock(levelData[0, y, x], x, y);
                GameObject instance = Instantiate(Shapes[levelData[0, y, x]], this.transform);
                instance.GetComponent<Block>().x = x;
                instance.GetComponent<Block>().y = y;
                instance.GetComponent<Block>().species = levelData[0, y, x];
                instance.transform.position = new Vector2(x, -y);

                if (SpeciesKind < levelData[0, y, x])
                {
                    SpeciesKind = levelData[0, y, x];
                }
            }
        }

        this.gameObject.transform.position = new Vector3(-levelData.GetLength(2) / 2, levelData.GetLength(1) / 2, 0);
    }

    public void MatchDataReSet()
    {
        for(int x = 0; x < MatchData.GetLength(0); x++)
        {
            for(int y = 0; y < MatchData.GetLength(1); y++)
            {
                MatchData[y, x] = -1;
            }
        }
    }

    //블럭을 파괴하는 코드만 남기고 매치된 블럭을 파괴하는건 매니져에서.
    public void DestroyMatchedBlocks()
    {
        GameObject temp;
        for (int y = 0; y < MatchData.GetLength(0); y++)
        {
            for (int x = 0; x < MatchData.GetLength(1); x++)
            {
                if (MatchData[y,x] > 0)
                {
                    temp = MapData[y, x];
                    MapData[y, x] = null;
                    Destroy(temp);
                    
                }
            }
        }
    }


    //블럭의 종류는 이 스크립트가 가지고있는게 맞지 않을까?
    //인스턴스의 이름으로도 충분히 어떤 종류의 조각인지 인식할 순 있다.
    public void CreateBlock(int shapes, int x, int y)
    {
        GameObject instance = Instantiate(Shapes[shapes], this.transform);
        instance.GetComponent<Block>().x = x;
        instance.GetComponent<Block>().y = y;
        instance.GetComponent<Block>().species = shapes;
        instance.transform.position = new Vector2(x, -y);

        MatchData[y, x] = -1;
        MapData[y, x] = instance;
    }
}
