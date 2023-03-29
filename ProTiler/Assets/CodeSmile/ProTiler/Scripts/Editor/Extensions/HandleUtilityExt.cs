﻿// Copyright (C) 2021-2023 Steffen Itterheim
// Refer to included LICENSE file for terms and conditions.

using CodeSmile.Extensions;
using UnityEditor;
using UnityEngine;

namespace CodeSmile.Editor.ProTiler.Extensions
{
	public static class HandleUtilityExt
	{
		/// <summary>
		///     Converts a 2d GUI point (typically mouse position) to a 3d grid coordinate.
		///     Tests intersection with an XZ plane at the specified Y height (default: 0).
		/// </summary>
		/// <param name="guiPoint">the 2d point ie mouse position</param>
		/// <param name="grid">the grid that determines the grid size</param>
		/// <param name="coord">the 3d grid coordinate on the plane</param>
		/// <param name="planeY">the height of the grid plane, default: 0</param>
		/// <returns>true if the guiPoint intersected with the plane</returns>
		public static bool GUIPointToGridCoord(Vector2 guiPoint, Vector3Int gridSize, out Vector3Int coord, float planeY = 0f)
		{
			var ray = HandleUtility.GUIPointToWorldRay(guiPoint);
			if (ray.IntersectsPlane(out Vector3 mouseWorldPos, planeY))
			{
				coord = mouseWorldPos.ToGridCoord(gridSize);
				return true;
			}

			coord = default;
			return false;
		}

		/// <summary>
		///     Adds a default control (default: passive) and returns the control Id.
		/// </summary>
		/// <param name="hint"></param>
		/// <param name="focusType">default: passive</param>
		/// <returns>control Id</returns>
		public static int AddDefaultControl(int hint, FocusType focusType = FocusType.Passive)
		{
			var controlId = GUIUtility.GetControlID(hint, FocusType.Passive);
			HandleUtility.AddDefaultControl(controlId);
			return controlId;
		}
	}
}