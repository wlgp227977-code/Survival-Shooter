using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public static readonly string AxisX = "Vertical";
    public static readonly string AxisZ = "Horizontal";
    public static readonly string FireAxis = "Fire1";
    
    public float MoveX {get; private set;}
    public float MoveZ {get; private set;}
    public bool Fire {get; private set;}

    private void OnDisable()
    {
        MoveX = 0f;
        MoveZ = 0f;
        Fire = false;
    }

    void Update()
    {
        MoveX = Input.GetAxis(AxisX);
        MoveZ = Input.GetAxis(AxisZ);
        Fire = Input.GetButton(FireAxis);
    }
}
