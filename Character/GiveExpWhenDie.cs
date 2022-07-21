using UnityEngine;

public class GiveExpWhenDie : MonoBehaviour
{
    public int expMin = 5;
    public int expMax = 7;

    public void GiveExp()
    {
        int random = Random.Range(expMin, expMax);
        GameManager.Instance.AddExp(random, transform);
    }
}
