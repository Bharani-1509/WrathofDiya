using UnityEngine;

public class RaycastReceiver : MonoBehaviour
{
    void OnMouseDown()
    {
        SendMessage("OnShot", SendMessageOptions.DontRequireReceiver);
    }
}
