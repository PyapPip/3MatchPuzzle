using System.Drawing;
using UnityEngine;

public class MouseManager : MonoBehaviour
{
    public GameManager gameManager;

    public GameObject Map;

    public void BlcokSelect()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Block hitBlock = hit.collider.gameObject.GetComponent<Block>();

                gameManager.OnClick(new Vector2Int(hitBlock.x, hitBlock.y));
            }
        }
    }

    /*
                else
                {
                    MapManager m = Map.GetComponent<MapManager>();

                    if (selectedBlocks.GetComponent<Block>().x - 1 == hit.collider.gameObject.GetComponent<Block>().x)
                    {
                        Debug.Log("왼");
                        m.ChangeBlock(selectedBlocks, 1);
                        selectedBlocks = null;
                    }

                    else if (selectedBlocks.GetComponent<Block>().x + 1 == hit.collider.gameObject.GetComponent<Block>().x)
                    {
                        Debug.Log("오");
                        m.ChangeBlock(selectedBlocks, 2);
                        selectedBlocks = null;
                    }

                    else if (selectedBlocks.GetComponent<Block>().y - 1 == hit.collider.gameObject.GetComponent<Block>().y)
                    {
                        Debug.Log("위");
                        m.ChangeBlock(selectedBlocks, 3);
                        selectedBlocks = null;
                    }

                    else if (selectedBlocks.GetComponent<Block>().y + 1 == hit.collider.gameObject.GetComponent<Block>().y)
                    {
                        Debug.Log("아래");
                        m.ChangeBlock(selectedBlocks, 4);
                        selectedBlocks = null;
                    }

                    //선택 취소
                    else
                    {
                        selectedBlocks = null;
                        Debug.Log("취소");
                    }
                }
                */
    /*
    public void BlockSwap()
    {
        if (Input.GetMouseButtonUp(0))
        {
            isCanClick = true;
        }

        if (!isCanClick)
            return;

        if (Input.GetMouseButtonDown(0) && isCanClick)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                isCanClick = false;

                MapManager m = Map.GetComponent<MapManager>();

                if (selectedBlocks.GetComponent<Block>().x - 1 == hit.collider.gameObject.GetComponent<Block>().x)
                {
                    Debug.Log("왼");
                    m.ChangeBlock(selectedBlocks, 1);
                    selectedBlocks = null;
                    gameManager.ChangeGameState(2);
                }

                else if (selectedBlocks.GetComponent<Block>().x + 1 == hit.collider.gameObject.GetComponent<Block>().x)
                {
                    Debug.Log("오");
                    m.ChangeBlock(selectedBlocks, 2);
                    selectedBlocks = null;
                    gameManager.ChangeGameState(2);
                }

                else if (selectedBlocks.GetComponent<Block>().y - 1 == hit.collider.gameObject.GetComponent<Block>().y)
                {
                    Debug.Log("위");
                    m.ChangeBlock(selectedBlocks, 3);
                    selectedBlocks = null;
                    gameManager.ChangeGameState(2);
                }

                else if (selectedBlocks.GetComponent<Block>().y + 1 == hit.collider.gameObject.GetComponent<Block>().y)
                {
                    Debug.Log("아래");
                    m.ChangeBlock(selectedBlocks, 4);
                    selectedBlocks = null;
                    gameManager.ChangeGameState(2);
                }

                //선택 취소
                else
                {
                    selectedBlocks = null;
                    gameManager.ChangeGameState(0);
                    Debug.Log("취소");
                    return;
                }
            }
        }
    }
    */
}