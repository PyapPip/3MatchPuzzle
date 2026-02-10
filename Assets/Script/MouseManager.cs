using System.Drawing;
using UnityEngine;

public class MouseManager : MonoBehaviour
{
    public GameManager gameManager;

    public GameObject Map;
    public bool isCanClick = true;

    public void BlcokSelect()
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
            Vector2Int selectBlockPos;

            if (Physics.Raycast(ray, out hit))
            {
                selectBlockPos = hit.collider.gameObject.GetComponent<Block>().boardPos;
                isCanClick = false;                                     //중복 클릭 방지

                gameManager.OnClick(selectBlockPos);
            }
        }
    }
}