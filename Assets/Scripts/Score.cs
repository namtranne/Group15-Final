
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public Transform player;
    public Text scoreTxt;

    // Update is called once per frame
    void Update()
    {
        scoreTxt.text = player.position.z.ToString("0");
    }
}
