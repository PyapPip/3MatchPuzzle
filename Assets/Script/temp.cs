using System.Collections;
using UnityEngine;

public class temp : MonoBehaviour
{
    public float cellSize = 1f;      // 격자 한 칸 크기
    public int moveFrame = 50;       // 이동하는 데 걸리는 프레임
    public int gridX = 0;           // 격자 X 좌표
    public int gridY = 0;           // 격자 Y 좌표
    public bool isMoving = false;   // 이동 중 여부 체크

    private void Start()
    {
        UpdatePosition();
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving) return; //이동 중이면 입력 무시

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            gridY += 1;
            StartCoroutine(MoveBlock());
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            gridY -= 1;
            StartCoroutine(MoveBlock());
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            gridX -= 1;
            StartCoroutine(MoveBlock());
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            gridX += 1;
            StartCoroutine(MoveBlock());
        }
    }



    void UpdatePosition()
    {
        // 격자 좌표 → 실제 월드 좌표로 변환
        transform.position = new Vector3(gridX * cellSize, gridY * cellSize, 0);
    }

    IEnumerator MoveBlock()
    {
        float nowFrame = 0.0f;

        isMoving = true;

        Vector3 startPos = transform.position;
        Vector3 targetPos = new Vector3(gridX * cellSize, gridY * cellSize, 0);
        /*
        while (nowFrame < moveFrame)
        {
            transform.position = Vector3.Lerp(startPos, targetPos, 1.0f / moveFrame * nowFrame);

            nowFrame++;
            yield return new WaitForEndOfFrame();
        }


        // 마지막 위치 보정
        transform.position = targetPos;
        */

        while (nowFrame < moveFrame)
        {

            if (nowFrame <= moveFrame / 2)
                transform.position = Vector3.Lerp(startPos, targetPos, 2.0f / moveFrame * nowFrame);
            else
                transform.position = Vector3.Lerp(targetPos, startPos, 2.0f / moveFrame * nowFrame);
            
            nowFrame++;
            yield return new WaitForEndOfFrame();
        }

        if(nowFrame==moveFrame)
            transform.position = startPos;

        isMoving = false;
    }


}