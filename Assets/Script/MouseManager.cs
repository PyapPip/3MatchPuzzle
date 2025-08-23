using System.Drawing;
using UnityEngine;

public class MouseManager : MonoBehaviour
{
    public GameObject Map;
    public bool isCanClick = true;

    private GameObject selectedBlocks;
    private Vector2 mouseClickPos;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && isCanClick)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                isCanClick = false;                                     //중복 클릭 방지
                selectedBlocks = hit.collider.gameObject;
            }
        }
        if (Input.GetMouseButton(0) && selectedBlocks != null)
        {
            mouseClickPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
            MapManager m = Map.GetComponent<MapManager>();
            if (mouseClickPos.x - selectedBlocks.transform.position.x < -1)
            {
                m.ChangeBlock(selectedBlocks, 1);
            }

            else if (mouseClickPos.x - selectedBlocks.transform.position.x > 1)
            {
                m.ChangeBlock(selectedBlocks, 2);
            }

            else if (mouseClickPos.y - selectedBlocks.transform.position.y < -1)
            {
                m.ChangeBlock(selectedBlocks, 3);
            }

            else if (mouseClickPos.y - selectedBlocks.transform.position.y > 1)
            {
                m.ChangeBlock(selectedBlocks, 4);
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            isCanClick = true;
        }
    }
}