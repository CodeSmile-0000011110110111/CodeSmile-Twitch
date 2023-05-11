﻿// Copyright (C) 2021-2023 Steffen Itterheim
// Refer to included LICENSE file for terms and conditions.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using Unity.Properties;
using LayerSize = UnityEngine.Vector2Int;

namespace CodeSmile.ProTiler.Data
{
	/// <summary>
	///     Represents a single, flat layer of tiles. Essentially a 2D array.
	/// </summary>
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public struct Tile3DLayer
	{
		[CreateProperty] private Tile3D[] m_Tiles;

		/// <summary>
		///     Get/set tile at index. No bounds check is performed.
		/// </summary>
		/// <param name="index"></param>
		public Tile3D this[int index]
		{
			get => m_Tiles[index];
			set => m_Tiles[index] = value;
		}

		/// <summary>
		///     Counts non-empty tiles.
		/// </summary>
		public int TileCount
		{
			get
			{
				if (IsInitialized == false)
					return 0;

				var count = 0;
				for (var index = 0; index < m_Tiles.Length; index++)
					count += m_Tiles[index].IsEmpty ? 0 : 1;
				return count;
			}
		}

		/// <summary>
		///     The max. size of the tile storage buffer.
		/// </summary>
		public int Capacity => IsInitialized ? m_Tiles.Length : 0;

		/// <summary>
		///     Returns true if the tile storage is not null. It can be initialized or null'ed via Resize().
		/// </summary>
		public bool IsInitialized => m_Tiles != null;

		/// <summary>
		///     Create a new layer with the given dimensions. A (0,0) sized layer will leave the tile storage uninitialized.
		/// </summary>
		/// <param name="size"></param>
		public Tile3DLayer(LayerSize size)
		{
			m_Tiles = null;
			AllocateTilesBuffer(size);
		}

		/// <summary>
		///     Resize the tile storage to the given size. If size is 0 in any dimension, the storage is null and
		///     IsInitialized returns false.
		/// </summary>
		/// <param name="size"></param>
		public void Resize(LayerSize size) => AllocateTilesBuffer(size);

		/// <summary>
		///     Sets tiles at the given coordinate using a list of Tile3DCoord instances.
		/// </summary>
		/// <param name="tileCoords"></param>
		/// <param name="width"></param>
		public void SetTiles(IEnumerable<Tile3DCoord> tileCoords, int width)
		{
			foreach (var coordData in tileCoords)
			{
				var index = Grid3DUtility.ToIndex2D(coordData.Coord, width);
				this[index] = coordData.Tile;
			}
		}

		[ExcludeFromCodeCoverage]
		public override string ToString() => $"{nameof(Tile3DLayer)}(Capacity: {Capacity}, Non-Empty: {TileCount})";

		private void AllocateTilesBuffer(LayerSize size)
		{
			if (size.x < 0 || size.y < 0)
				throw new ArgumentException($"negative size is not allowed: {size}");

			var capacity = size.x * size.y;
			m_Tiles = capacity > 0 ? new Tile3D[capacity] : null;
		}
	}
}
