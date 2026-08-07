using UnityEngine;

public class Block : MonoBehaviour
{
    public Vector2Int boardPos;
    public int species, fall = 0;

    private void Update()
    {
        this.gameObject.name = $"Block_{boardPos.x}_{boardPos.y}_{fall}";
    }
}