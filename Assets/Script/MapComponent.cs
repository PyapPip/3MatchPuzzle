using UnityEngine;
using System;

public class MapComponent : MonoBehaviour
{
    public GameObject Camera;
    public GameObject[,] MapData;                   //현재 맵의 구성
    public GameObject[] Shapes = new GameObject[5]; //5가지 종류의 블럭을 담아둔 배열
    /// <summary>
    /// -1 = 검사되지 않음, 0 = 검사됨, 1 = x축 매치, 2 = y축 매치, 3 = x,y 매치
    /// </summary> 
    public int[,] MatchData;
    public int SpeciesKind = 0;

    public void CreateMap(int[,,] _levelData)
    {

        MapData = new GameObject[_levelData.GetLength(1), _levelData.GetLength(2)];
        MatchData = new int[_levelData.GetLength(1), _levelData.GetLength(2)];

        for (int y = 0; y < _levelData.GetLength(1); y++)
        {
            for (int x = 0; x < _levelData.GetLength(2); x++)
            {
                CreateBlock(_levelData[0, y, x], x, y);

                if (SpeciesKind < _levelData[0, y, x])
                {
                    SpeciesKind = _levelData[0, y, x];
                }
            }
        }

        Camera.transform.position = new Vector3(_levelData.GetLength(2) / 2, -_levelData.GetLength(1) / 2, -1);
    }

    public void DataReSet()
    {
        Array.Clear(MatchData, -1, MatchData.Length);
        //Array.Clear(blocksToSpawn, 0, blocksToSpawn.Length);
    }

    //블럭의 종류는 이 스크립트가 가지고있는게 맞지 않을까?
    //인스턴스의 이름으로도 충분히 어떤 종류의 조각인지 인식할 순 있다.
    public void CreateBlock(int _shapes, int _x, int _y)
    {
        GameObject instance = Instantiate(Shapes[_shapes], this.transform);
        instance.GetComponent<Block>().x = _x;
        instance.GetComponent<Block>().y = _y;
        instance.GetComponent<Block>().species = _shapes;
        instance.transform.position = new Vector2(_x, -_y);

        MatchData[_y, _x] = -1;
        MapData[_y, _x] = instance;
    }
    
    //좌표 지정을 위해 오버라이딩한 함수
    public void CreateBlock(int _shapes, int _x, int _y, Vector2 _pos)
    {
        GameObject instance = Instantiate(Shapes[_shapes], this.transform);
        instance.GetComponent<Block>().x = _x;
        instance.GetComponent<Block>().y = _y;
        instance.GetComponent<Block>().species = _shapes;
        instance.transform.position = _pos;

        MatchData[_y, _x] = -1;
        MapData[_y, _x] = instance;
    }
}
