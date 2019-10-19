using UnityEngine;

public class Beacon : MonoBehaviour
{
	public RectTransform beacon;
    // Update is called once per frame
    void Update()
    {
		if (beacon)
		{
			var cam = Camera.main;
			var screen = new Vector3(cam.scaledPixelWidth, 0f, 0f);
			var point = cam.WorldToScreenPoint(transform.position);

			point.x = Mathf.Clamp(point.x, 0f, cam.scaledPixelWidth);
			if (point.z < 0f)
			{
				beacon.gameObject.SetActive(true);
				point = screen - point;
			}
			else if (point.x <= 0f || point.x >= cam.scaledPixelWidth)
			{
				beacon.gameObject.SetActive(true);
			}
			else
			{
				beacon.gameObject.SetActive(false);
			}
			point.y = 0f;
			point.z = 0f;
			
			beacon.position = point;
		}
    }
}
