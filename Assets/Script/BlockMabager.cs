using UnityEngine;

public class BlockMabager : MonoBehaviour
{
    public GameObject[] Shapes = new GameObject[5]; //5가지 종류의 블럭을 담아둔 배열

    public void CreateBlock(int _shapes, int _x, int _y)
    {
        GameObject instance = Instantiate(Shapes[_shapes], this.transform);
        instance.GetComponent<Block>().boardPos = new Vector2Int(_x, _y);
        instance.GetComponent<Block>().species = _shapes;
        instance.transform.position = new Vector2(_x, -_y);

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

        MapData[_y, _x] = instance;
    }

}
