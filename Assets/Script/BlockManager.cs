using UnityEngine;

public class BlockManager : MonoBehaviour
{
    public GameObject[] Shapes = new GameObject[5]; //5가지 종류의 블럭을 담아둔 배열

    public GameObject CreateBlock(int _shapes, int _x, int _y)
    {
        GameObject instance = Instantiate(Shapes[_shapes], this.transform);
        instance.GetComponent<Block>().boardPos = new Vector2Int(_x, _y);
        instance.GetComponent<Block>().species = _shapes;
        instance.transform.position = new Vector2(_x, -_y);

        return instance;
    }

    //좌표 지정을 위해 오버라이딩한 함수
    public GameObject CreateBlock(int _shapes, int _x, int _y, Vector2 _spwonPos)
    {
        GameObject instance = Instantiate(Shapes[_shapes], this.transform);
        instance.GetComponent<Block>().boardPos.x = _x;
        instance.GetComponent<Block>().boardPos.y = _y;
        instance.GetComponent<Block>().species = _shapes;
        instance.transform.position = _spwonPos;

        return instance;
    }

    public void playSwap(Vector2Int _targetPos, Vector2Int _dir)
    {

    }

    public void playSnapBack(Vector2Int _targetPos, Vector2Int _dir)
    {

    }
}
