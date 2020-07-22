using UnityEngine;

public class CameraController : MonoBehaviour
{
	// FIXME: Do we really need a map for getting bounds?
	[SerializeField]
	Map map;

	public float moveSpeed = 5f;
	public float zoomSpeed;

	public float borderThickness = 15f;

	public float minZoom, maxZoom;

	private float zoom;
	private float boundX, boundZ;

	private void Start()
	{
		boundX = map.Width;
		boundZ = map.Length;
	}

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

		pos.x = Mathf.Clamp(pos.x, 0, boundX);
		pos.z = Mathf.Clamp(pos.z, -clampingOffsetZ, boundZ-clampingOffsetZ);

		transform.position = pos;
	}

	public float BoundX
	{
		get { return boundX; }
	}

	public float BoundZ
	{
		get { return boundZ; }
	}

	public float Zoom
	{
		get { return zoom; }
	}

}
