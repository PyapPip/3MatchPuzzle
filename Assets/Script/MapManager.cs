using System.Collections;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public int[,,] MapData = new int[1, 5, 5];
    public GameObject[] blocks = new GameObject[5];
    public GameObject[,] Map;

    private GameObject temp;

    private void CreateMap()
    {
        Map = new GameObject[MapData.GetLength(1), MapData.GetLength(2)];

        for (int y = 0; y < MapData.GetLength(1); y++)
        {
            for (int x = 0; x < MapData.GetLength(2); x++)
            {
                GameObject instance = Instantiate(blocks[MapData[0, y, x]], this.transform);
                instance.GetComponent<Block>().x = x;
                instance.GetComponent<Block>().y = y;
                instance.GetComponent<Block>().species = MapData[0, y, x];
                instance.transform.position = new Vector2(x, -y);

                Map[y, x] = instance;
            }
        }

        this.gameObject.transform.position = new Vector3 (-MapData.GetLength(2)/2, MapData.GetLength(1)/2, 0);
    }

    public void ChangeBlock(GameObject block, int dir)
    {
        //dir 1.left 2.right 3.down 4.up

        //바꾸러는 방향이 유효한지 확인
        //오브젝트 좌표 변경
        switch (dir)
        {
            case 1:
                if(block.GetComponent<Block>().x >= 1)
                {
                    //터질 수 있는지 확인
                    //터질 수 있다면 자릴 바꾼 후 터뜨리기
                    //없다면 자릴 바꿨다 돌아오는 연출
                }
                break;
            case 2:
                if (block.GetComponent<Block>().x < Map.GetLength(0))
                {
                    
                }
                break;
            case 3:
                if(block.GetComponent<Block>().y < Map.GetLength(1))
                {
                    
                }
                break;
            case 4:
                if(block.GetComponent<Block>().y >= 1)
                {
                    
                }
                break;
        }
    }

    void MatchChack(GameObject[,] arr)
    {
        //게임오브젝트 배열을 복사해오는것이 부하나 버그를 불러올 수 있는가?
        int count = 0;
        int prevSpecies = -1;

        //x 확인
        for (int y = 0; y < arr.GetLength(0); y++)
        {
            for (int x = 0; x < arr.GetLength(1); x++)
            {
                if (arr[y,x].GetComponent<Block>().species == prevSpecies)
                {
                    count++;
                }
                else
                {
                    prevSpecies = arr[y, x].GetComponent<Block>().species;
                }
            }
        }

        if(count >= 3)
        {
            //터뜨리기
        }

        for (int x = 0; x < Map.GetLength(1); x++)
        {
            for (int y = 0; y < Map.GetLength(0); y++)
            {
                if (Map[y, x].GetComponent<Block>().species == prevSpecies)
                {
                    count++;
                }
                else
                {
                    prevSpecies = Map[y, x].GetComponent<Block>().species;
                }
            }
        }

        if (count >= 3)
        {
            //터뜨리기
        }

        return;

        //터트려야할 것을 어떻게 반환할것인가?
    }

    void CanMakeMatch()
    {
        for (int y = 0; y < Map.GetLength(0); y++)
        {
            for (int x = 0; x < Map.GetLength(1); x++)
            {
                Map[y,x].GetComponent<Block>().isMatchChack = true;
                GameObject[,] virtualMap;

                if (x > 0)
                {
                    virtualMap = Map;
                    (virtualMap[y, x], virtualMap[y, x - 1]) = (virtualMap[y, x - 1], virtualMap[y, x]);
                    MatchChack(virtualMap);
                }

                if(x < Map.GetLength(1))
                {
                    virtualMap = Map;
                    (virtualMap[y, x], virtualMap[y, x + 1]) = (virtualMap[y, x + 1], virtualMap[y, x]);
                    MatchChack(virtualMap);
                }

                if (y > 0)
                {
                    virtualMap = Map;
                    (virtualMap[y, x], virtualMap[y - 1, x]) = (virtualMap[y - 1, x], virtualMap[y, x]);
                    MatchChack(virtualMap);
                }

                if (y < Map.GetLength(0))
                {
                    virtualMap = Map;
                    (virtualMap[y, x], virtualMap[y + 1, x]) = (virtualMap[y + 1, x], virtualMap[y, x]);
                    MatchChack(virtualMap);
                }
            }
        }

        //터뜨릴 수 없다면 재배치
    }

    void Start()
    {
        MapData = new int[1, 5, 5]{
            {
                { 0, 1, 2, 1, 0},
                { 1, 2, 0, 2, 3},
                { 3, 0, 3, 3, 1},
                { 3, 2, 0, 2 ,1},
                { 0, 3, 2, 1, 0}
            }
        };

        CreateMap();
    }
}
