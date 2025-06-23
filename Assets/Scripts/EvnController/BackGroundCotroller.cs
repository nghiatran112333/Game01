using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class BackgroundController : MonoBehaviour
{
    public Transform mainCam;
    public Transform MidBg;
    public Transform SideBg;
    public float length;

    void Start()
    {
        if (mainCam == null && Camera.main != null)
            mainCam = Camera.main.transform;
    }

    void Update()
    {
        if (mainCam.position.x > MidBg.position.x)
        {
            UpdateBackGroundPosition(Vector3.right);
        }
        else if (mainCam.position.x < MidBg.position.x)
        {
            UpdateBackGroundPosition(Vector3.left);
        }
    }

    void UpdateBackGroundPosition(Vector3 direction)
    {
        SideBg.position = MidBg.position+direction*length;
        Transform temp = MidBg;
        MidBg = SideBg;
        SideBg = temp;
    }
}
