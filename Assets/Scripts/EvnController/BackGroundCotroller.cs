using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    private float startPos, length;
    public GameObject cam;
    public float parallaxEffect; // 0 = theo camera hoàn toàn, 1 = không di chuyển, 0.5 = di chuyển nửa tốc độ

    void Start()
    {
        startPos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void FixedUpdate()
    {
        // Tính khoảng cách camera đã di chuyển (tỉ lệ parallax)
        float distance = cam.transform.position.x * parallaxEffect;
        float movement = cam.transform.position.x * (1 - parallaxEffect);

        // Cập nhật vị trí background theo chuyển động camera
        transform.position = new Vector3(startPos + distance, transform.position.y, transform.position.z);

        // Nếu background đã vượt khỏi chiều dài, thì dịch ngược lại để cuộn vô tận
        if (movement > startPos + length)
        {
            startPos += length;
        }
        else if (movement < startPos - length)
        {
            startPos -= length;
        }
    }
}
