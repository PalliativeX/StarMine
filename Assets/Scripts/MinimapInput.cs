using UnityEngine;

public class MinimapInput : MonoBehaviour
{
	[SerializeField]
	CameraController cameraController;
	[SerializeField]
	HumanPlayerInputController inputController;
	[SerializeField]
	RectTransform minimapPanel;
	Vector2 min, max;

	public void Awake()
	{
		min.x = 0;
		min.y = 0;
		max.x = min.x + minimapPanel.sizeDelta.x;
		max.y = min.y + minimapPanel.sizeDelta.y;
	}

	private void Update()
	{
		if (Input.GetMouseButton(0) && !inputController.isSelecting)
		{
			Vector2 mousePos = Input.mousePosition;
			if (mousePos.x >= min.x && mousePos.x <= max.x && 
				mousePos.y >= min.y && mousePos.y <= max.y)
			{
				float offsetX = mousePos.x / (max.x - min.x);
				float offsetZ = mousePos.y / (max.y - min.y);

				// TODO: Now hardcoded, change that
				float additionalOffsetZ = Mathf.Lerp(3, 10, cameraController.Zoom);

				cameraController.transform.localPosition = new Vector3(BaseMetrics.Width * offsetX, cameraController.transform.localPosition.y, 
																	   BaseMetrics.Length * offsetZ - additionalOffsetZ);
			}
		}
	}

}
