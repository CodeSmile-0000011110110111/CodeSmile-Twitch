﻿// Copyright (C) 2021-2023 Steffen Itterheim
// Refer to included LICENSE file for terms and conditions.

using CodeSmile.ProTiler;
using System;
using UnityEditor;
using UnityEditor.Overlays;
using UnityEngine;

namespace CodeSmile.Editor.ProTiler.Overlays
{
	[Overlay(typeof(SceneView), Global.TileEditorName, Global.TileEditorName)]
	[Icon(Global.OverlayIconPath + "OverlayToolbarIcon.png")]
	public class TileLayerOverlay : ToolbarOverlay
	{
		public static void SetToggleOn(Type toggleType)
		{
			SceneView.lastActiveSceneView.TryGetOverlay(Global.TileEditorName, out var overlay);
			var tileWorldOverlay = overlay as TileLayerOverlay;
			if (tileWorldOverlay != null)
			{
				// TODO: how to access the overlay's elements??

				/*
				foreach (var element in tileWorldOverlay.)
				{
					var otherToggle = element as TileEditorToolbarRadioToggle;
					if (otherToggle != null && otherToggle != toggle)
						otherToggle.SetValueWithoutNotify(false);
				}
			*/
			}
		}

		private TileLayerOverlay()
			: base(TileLayerSelectionToggle.Id, TileLayerPenDrawToggle.Id, TileLayerRectDrawToggle.Id) {}
	}
}