using UnityEngine;

#pragma warning disable 0649

public class CameraController : MonoBehaviour
{
	[SerializeField]
	float moveSpeed = 5f;
	[SerializeField]
	float zoomSpeed;
	[SerializeField]
	float borderThickness = 15f;
	[SerializeField]
	float minZoom, maxZoom;

	private float zoom;

	void Update()
	{
		AdjustZoom();
		AdjustPosition();
	}

	void AdjustZoom()
	{
		float scrollInput = Input.GetAxis("Mouse ScrollWheel");
		if (scrollInput != 0f)
		{
			zoom = Mathf.Clamp01(zoom + scrollInput);

			float distance = Mathf.Lerp(minZoom, maxZoom, zoom);
			transform.localPosition = new Vector3(transform.localPosition.x, distance, transform.localPosition.z);
		}
	}

	void AdjustPosition()
	{
		Vector3 pos = transform.position;

		if (Input.GetKey(KeyCode.RightArrow) || Input.mousePosition.x >= Screen.width - borderThickness)
		{
			pos.x += moveSpeed * Time.deltaTime;
		}
		else if (Input.GetKey(KeyCode.LeftArrow) || Input.mousePosition.x <= borderThickness)
		{
			pos.x += -moveSpeed * Time.deltaTime;
		}
		if (Input.GetKey(KeyCode.DownArrow) || Input.mousePosition.y <= borderThickness)
		{
			pos.z += -moveSpeed * Time.deltaTime;
		}
		else if (Input.GetKey(KeyCode.UpArrow) || Input.mousePosition.y >= Screen.height - borderThickness)
		{
			pos.z += moveSpeed * Time.deltaTime;
		}

		float clampingOffsetZ = 4;

		pos.x = Mathf.Clamp(pos.x, 0, BaseMetrics.Width);
		pos.z = Mathf.Clamp(pos.z, -clampingOffsetZ, BaseMetrics.Length - clampingOffsetZ);

		transform.position = pos;
	}

	public float Zoom
	{
		get { return zoom; }
	}

}