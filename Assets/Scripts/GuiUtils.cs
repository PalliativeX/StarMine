﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GuiUtils
{
	static Camera mainCamera = Camera.main;
	static Texture2D whiteTexture;
	public static Texture2D WhiteTexture
	{
		get {
			if (whiteTexture == null)
			{
				whiteTexture = new Texture2D(1, 1);
				whiteTexture.SetPixel(0, 0, Color.white);
				whiteTexture.Apply();
			}

			return whiteTexture;
		}
	}

	public static void DrawScreenRect(Rect rect, Color color)
	{
		GUI.color = color;
		GUI.DrawTexture(rect, WhiteTexture);
		GUI.color = Color.white;
	}

	public static void DrawScreenRectBorder(Rect rect, float thickness, Color color)
	{
		// Top
		DrawScreenRect(new Rect(rect.xMin, rect.yMin, rect.width, thickness), color);
		// Left
		DrawScreenRect(new Rect(rect.xMin, rect.yMin, thickness, rect.height), color);
		// Right
		DrawScreenRect(new Rect(rect.xMax - thickness, rect.yMin, thickness, rect.height), color);
		// Bottom
		DrawScreenRect(new Rect(rect.xMin, rect.yMax - thickness, rect.width, thickness), color);
	}

	public static Rect GetScreenRect(Vector3 screenPos1, Vector3 screenPos2)
	{
		screenPos1.y = Screen.height - screenPos1.y;
		screenPos2.y = Screen.height - screenPos2.y;

		var topLeft = Vector3.Min(screenPos1, screenPos2);
		var bottomRight = Vector3.Max(screenPos1, screenPos2);

		return Rect.MinMaxRect(topLeft.x, topLeft.y, bottomRight.x, bottomRight.y);
	}

	public static Bounds GetViewportBounds(Camera camera, Vector3 screenPos1, Vector3 screenPos2)
	{
		var v1 = mainCamera.ScreenToViewportPoint(screenPos1);
		var v2 = mainCamera.ScreenToViewportPoint(screenPos2);
		var min = Vector3.Min(v1, v2);
		var max = Vector3.Max(v1, v2);
		min.z = camera.nearClipPlane;
		max.z = camera.farClipPlane;

		var bounds = new Bounds();
		bounds.SetMinMax(min, max);
		return bounds;
	}

}
