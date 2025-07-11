using System.Collections;
using UnityEngine;

public class PoolMember : MonoBehaviour
{
    public Pool Pool { get; set; }

    public void SetPool(Pool pool)
    {
        Pool = pool;
    }
    public void ReturnToPool(float delay = 0f)
    {
        if (delay > 0)
        {
            StartCoroutine(DelayReturnToPool(delay));
            return;
        }
        Pool.ReturnObject(this);
    }

    IEnumerator DelayReturnToPool(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        ReturnToPool();
    }
}
