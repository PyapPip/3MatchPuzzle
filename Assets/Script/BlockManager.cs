using UnityEngine;

public class BlockManager : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;

    public GameObject[] Shapes = new GameObject[5]; //5가지 종류의 블럭을 담아둔 배열

    private int moveBlockCount;

    public GameObject CreateBlock(int _shapes, int _x, int _y)
    {
        GameObject instance = Instantiate(Shapes[_shapes], this.transform);
        instance.GetComponent<Block>().boardPos = new Vector2Int(_x, _y);
        instance.GetComponent<Block>().species = _shapes;
        instance.transform.position = new Vector2(_x, -_y);

        return instance;
    }

    //좌표 지정을 위해 오버라이딩한 함수
    public GameObject CreateBlock(int _shapes, int _x, int _y, int _fall)
    {
        GameObject instance = Instantiate(Shapes[_shapes], this.transform);
        instance.GetComponent<Block>().boardPos.x = _x;
        instance.GetComponent<Block>().boardPos.y = _y;
        instance.GetComponent<Block>().species =    _shapes;
        instance.GetComponent<Block>().fall =       _fall;
        instance.transform.position =               new Vector2(_x, -_y);

        return instance;
    }

    public void playSwap(GameObject _block1, GameObject _block2)
    {
        moveBlockCount = 2;
        _block2.pos
        _block1.GetComponent<BlockMove>().MoveStart(BlockMove.BlockAnimState.Swaping, _block2.transform.position);
        _block2.GetComponent<BlockMove>().MoveStart(BlockMove.BlockAnimState.Swaping, _block1.transform.position);
    }

    public void playSnapBack(GameObject _block1, GameObject _block2)
    {
        moveBlockCount = 2;
        _block1.GetComponent<BlockMove>().MoveStart(BlockMove.BlockAnimState.Snapback, _block2.transform.position);
        _block2.GetComponent<BlockMove>().MoveStart(BlockMove.BlockAnimState.Snapback, _block1.transform.position);
    }

    public void BlockMoveEnd()
    {
        moveBlockCount--;

        if (moveBlockCount <= 0)
        {
            gameManager.MoveEnd();
        }
    }
}
