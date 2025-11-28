using System.Collections;
using UnityEngine;

public class BlockMove : MonoBehaviour
{
    private float cellSize = 1f;                // 격자 한 칸 크기
    private int moveFrame = 12;                 // 이동하는 데 걸리는 프레임
    private int gridX = 0;                      // 격자 X 좌표
    private int gridY = 0;                      // 격자 Y 좌표
    private bool isMovingStart = false;         // 이동 중 여부 체크
    private int[] targetLocation;

    public int isMatch = -1;                    // -1 = 미확인 0 = 매치 안 됨 1 = 매치 됨

    private void Start()
    {
        UpdatePosition(); 
    }

    // Update is called once per frame
    void Update()
    {
        if (!isMovingStart) return;

        targetLocation = new int[2] { this.gameObject.GetComponent<Block>().x, this.gameObject.GetComponent<Block>().y };
        if(isMatch == 1)
        {
            StartCoroutine(MoveBlock());
        }
        else
        {

        }
    }

    void UpdatePosition()
    {
        // 격자 좌표 → 실제 월드 좌표로 변환
        transform.position = new Vector3(targetLocation[0] * cellSize, targetLocation[1] * cellSize, 0);
    }

    IEnumerator MoveBlock()
    {
        float nowFrame = 0.0f;

        Vector3 startPos = transform.position;
        Vector3 targetPos = new Vector3(gridX * cellSize, gridY * cellSize, 0);

        while (nowFrame < moveFrame)
        {
            transform.position = Vector3.Lerp(startPos, targetPos, 1.0f / moveFrame * nowFrame);

            nowFrame++;
            yield return new WaitForEndOfFrame();
        }

        // 마지막 위치 보정
        transform.position = targetPos;
    }

    IEnumerator Snapback()
    {
        float nowFrame = 0.0f;

        Vector3 startPos = transform.position;
        Vector3 targetPos = new Vector3(gridX * cellSize, gridY * cellSize, 0);

        while (nowFrame < moveFrame)
        {

            if (nowFrame <= moveFrame / 2)
                transform.position = Vector3.Lerp(startPos, targetPos, 1.0f / (moveFrame * nowFrame * 2));
            else
                transform.position = Vector3.Lerp(targetPos, startPos, 1.0f / (moveFrame * nowFrame * 2));

            nowFrame++;
            yield return new WaitForEndOfFrame();
        }

        // 마지막 위치 보정
        transform.position = startPos;

        yield return new WaitForEndOfFrame();
    }
}
