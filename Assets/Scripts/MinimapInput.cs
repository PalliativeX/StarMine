using UnityEngine;

public class MinimapInput : MonoBehaviour
{
	public CameraController cameraController;

	public RectTransform minimapPanel;
	float minX, minY, maxX, maxY;

	public void Awake()
	{
		// TODO: Calculate instead of hardcoding
		minX = 0;
		minY = 0;
		maxX = minX + minimapPanel.sizeDelta.x;
		maxY = minY + minimapPanel.sizeDelta.y;
	}

	private void Update()
	{
		// TODO: Prob optimize
		if (Input.GetMouseButton(0))
		{
			Vector2 mousePos = Input.mousePosition;
			if (mousePos.x >= minX && mousePos.x <= maxX && 
				mousePos.y >= minY && mousePos.y <= maxY)
			{
				float offsetX = mousePos.x / (maxX - minX);
				float offsetZ = mousePos.y / (maxY - minY);

				// TODO: Now hardcoded, change that
				float additionalOffsetZ = Mathf.Lerp(3, 10, cameraController.Zoom);

				cameraController.transform.localPosition = new Vector3(cameraController.BoundX * offsetX, cameraController.transform.localPosition.y, 
																	   cameraController.BoundZ * offsetZ - additionalOffsetZ);
			}
		}
	}

}
