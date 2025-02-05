using UnityEngine;

public class AimIndicator : MonoBehaviour
{
    [SerializeField]
    private GameObject arrowMesh;

    public void SetArrowScale(float scale)
    {
        arrowMesh.transform.localScale = new Vector3(scale, scale, scale);
    }
}
