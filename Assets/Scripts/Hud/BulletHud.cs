using UnityEngine;
using UnityEngine.UI;

public class BulletHud : MonoBehaviour
{
    [SerializeField] private Text text;

    public int numOfBulletsToShow
    {
        set => text.text = value.ToString();
    }
}