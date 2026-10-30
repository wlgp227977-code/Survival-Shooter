using UnityEngine;

public class CameraTarget : MonoBehaviour
{
    public Transform player;

    private void LateUpdate()
    {
        transform.position = player.position;
    }
}
