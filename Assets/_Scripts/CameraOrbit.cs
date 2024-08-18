using UnityEngine;

public class CameraOrbit : MonoBehaviour
{
    public Transform target;        // Objek yang akan menjadi pusat orbit kamera
    public float distance = 5.0f;   // Jarak kamera dari objek
    public float xSpeed = 120.0f;   // Kecepatan rotasi kamera pada sumbu X
    public float ySpeed = 120.0f;   // Kecepatan rotasi kamera pada sumbu Y

    public float yMinLimit = -20f;  // Batas minimum rotasi sumbu Y
    public float yMaxLimit = 80f;   // Batas maksimum rotasi sumbu Y

    private float x = 0.0f;
    private float y = 0.0f;

    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;
    }

    void LateUpdate()
    {
        // Mengecek jika tombol mouse kanan ditekan
        if (Input.GetMouseButton(1))
        {
            // Mendapatkan input mouse
            x += Input.GetAxis("Mouse X") * xSpeed * distance * 0.02f;
            y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;

            // Mengatur batas rotasi sumbu Y
            y = Mathf.Clamp(y, yMinLimit, yMaxLimit);

            // Rotasi kamera berdasarkan input mouse
            Quaternion rotation = Quaternion.Euler(y, x, 0);

            // Mengatur posisi kamera sesuai rotasi dan jarak dari objek
            Vector3 position = rotation * new Vector3(0.0f, 0.0f, -distance) + target.position;

            // Memperbarui rotasi dan posisi kamera
            transform.rotation = rotation;
            transform.position = position;
        }
    }
}
