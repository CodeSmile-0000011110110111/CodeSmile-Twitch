﻿// Copyright (C) 2021-2023 Steffen Itterheim
// Refer to included LICENSE file for terms and conditions.

namespace CodeSmile.Tile
{
	public sealed partial class TileLayer
	{
#if UNITY_EDITOR
		public void OnValidate()
		{
			if (m_ClearTiles)
			{
				m_ClearTiles = false;
				ClearTiles();
			}

			UpdateTileCount();
			ClampGridSize();
			ClampTileSetIndex();
			SetTileName();
		}

		private void SetTileName() => m_SelectedTileName = TileSet.GetPrefab(m_SelectedTileSetIndex)?.name;

		private void ClampGridSize()
		{
			if (Grid != null)
				Grid.ClampGridSize();
		}

#endif
	}
}