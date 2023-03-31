﻿// Copyright (C) 2021-2023 Steffen Itterheim
// Refer to included LICENSE file for terms and conditions.

using System;
using UnityEngine;

namespace CodeSmile.ProTiler
{
	[Flags]
	public enum Tile3DFlags
	{
		None = 0,
		DirectionNorth = 1 << 0,
		DirectionEast = 1 << 1,
		DirectionSouth = 1 << 2,
		DirectionWest = 1 << 3,
		FlipHorizontal = 1 << 4,
		FlipVertical = 1 << 5,

		AllDirections = DirectionNorth | DirectionSouth | DirectionEast | DirectionWest,
		AllFlips = FlipHorizontal | FlipVertical,
	}

	public abstract class Tile3DBase : ScriptableObject
	{
		[SerializeField] private GameObject m_Prefab;
		[SerializeField] private Tile3DFlags m_Flags;
		[SerializeField] [HideInInspector] private Matrix4x4 m_Transform;
		public GameObject Prefab
		{
			get => m_Prefab;
			set => m_Prefab = value;
		}
		public Tile3DFlags Flags
		{
			get => m_Flags;
			set => m_Flags = value;
		}
		public Matrix4x4 Transform
		{
			get => m_Transform;
			set => m_Transform = value;
		}
		private void Reset() => m_Flags = Tile3DFlags.DirectionNorth;
		public virtual void RefreshTile(Vector3Int coord, Tilemap3D tilemap) => tilemap.RefreshTile(coord);

		public virtual Tile3DData GetTileData(Vector3Int coord, Tilemap3D tilemap, ref Tile3DData tileData) =>
			throw new NotImplementedException();
	}
}
