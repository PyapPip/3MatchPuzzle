using UnityEngine;

public class MatchEffect : MonoBehaviour
{
    private void EffectEnd()
    {
        GetComponentInParent<FXManager>().countActiveEffect--;
        this.gameObject.SetActive(false);
    }
}
