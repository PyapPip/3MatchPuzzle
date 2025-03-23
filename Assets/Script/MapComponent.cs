using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapComponent : MonoBehaviour
{
    public GameObject[,] MapData;
    /// <summary>
    /// -1 = 검사되지 않음, 0 = 검사됨, 1 = x축 매치, 2 = y축 매치, 3 = x,y 매치
    /// </summary> 
    public int[,] MatchData;    

    public void CreateMap(int[,,] levelData, GameObject[] shapes)
    {

        MapData = new GameObject[levelData.GetLength(1), levelData.GetLength(2)];
        MatchData = new int[levelData.GetLength(1), levelData.GetLength(2)];

        for (int y = 0; y < levelData.GetLength(1); y++)
        {
            for (int x = 0; x < levelData.GetLength(2); x++)
            {
                GameObject instance = Instantiate(shapes[levelData[0, y, x]], this.transform);
                instance.GetComponent<Block>().x = x;
                instance.GetComponent<Block>().y = y;
                instance.GetComponent<Block>().species = levelData[0, y, x];
                instance.transform.position = new Vector2(x, -y);
                MatchData[y, x] = -1;

                MapData[y, x] = instance;
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
                    
                    //특수블럭 생성은 여기서
                }
            }
        }
    }
}
