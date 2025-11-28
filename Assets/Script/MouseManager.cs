using System.Drawing;
using UnityEngine;

public class MouseManager : MonoBehaviour
{
    public GameObject Map;
    public bool isCanClick = true;
    public GameObject selectedBlocks;

    private Vector2 mouseClickPos;

    void Update()
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
                isCanClick = false;                                     //중복 클릭 방지

                if(selectedBlocks == null)
                {
                    selectedBlocks = hit.collider.gameObject;
                }

                else
                {
                    //mouseClickPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
                    
                    MapManager m = Map.GetComponent<MapManager>();

                    if(selectedBlocks.GetComponent<Block>().x -1 == hit.collider.gameObject.GetComponent<Block>().x)
                    {
                        m.ChangeBlock(selectedBlocks, 1);
                        selectedBlocks = null;
                        Debug.Log("왼");
                    }

                    else if (selectedBlocks.GetComponent<Block>().x + 1 == hit.collider.gameObject.GetComponent<Block>().x)
                    {
                        m.ChangeBlock(selectedBlocks, 2);
                        selectedBlocks = null;
                        Debug.Log("오");
                    }

                    else if (selectedBlocks.GetComponent<Block>().y + 1 == hit.collider.gameObject.GetComponent<Block>().y)
                    {
                        m.ChangeBlock(selectedBlocks, 3);
                        selectedBlocks = null;
                        Debug.Log("아래");
                    }

                    else if (selectedBlocks.GetComponent<Block>().y - 1 == hit.collider.gameObject.GetComponent<Block>().y)
                    {
                        m.ChangeBlock(selectedBlocks, 4);
                        selectedBlocks = null;
                        Debug.Log("위");
                    }

                    //선택 취소
                    else
                    {
                        selectedBlocks = null;
                        Debug.Log("취소");
                    }
                }
            }
        }
    }
}

//같은 블럭 클릭시 취소되도록
//마우스 클릭위치로 파악하는것이 아닌 클릭한 블럭 위치를 기준으로 비교할 수 있도록 수정 필요