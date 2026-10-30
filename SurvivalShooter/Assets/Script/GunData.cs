using UnityEngine;

[CreateAssetMenu(fileName = "GunData", menuName = "Scriptable Objects/GunData")]
public class GunData : ScriptableObject
{
    public AudioClip shotClip;
    public float damage = 50f;
    public float timeBetFire = 0.12f;
}
