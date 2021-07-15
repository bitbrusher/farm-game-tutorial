using UnityEngine;

public class IsometricZ : MonoBehaviour
{
    private void Start()
    {
        var objTransform = transform;
        var position = objTransform.position;
        position = new Vector3(position.x, position.y, position.y / 100f);
        objTransform.position = position;
    }
}