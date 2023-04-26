﻿// Copyright (C) 2021-2023 Steffen Itterheim
// Refer to included LICENSE file for terms and conditions.

using CodeSmile.ProTiler.Data;
using UnityEngine;

namespace CodeSmile.ProTiler.Assets
{
	public abstract class Tile3DAssetBase : ScriptableObject
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

		private void Init() => SetDefaultFlags();

		private void Awake() => Init();

		private void Reset() => Init();

		private void SetDefaultFlags() => m_Flags = Tile3DFlags.DirectionNorth;

		// public virtual void RefreshTile(Vector3Int coord, Tilemap3D tilemap) => tilemap.RefreshTile(coord);
		// public virtual Tile3DData GetTileData(Vector3Int coord, Tilemap3D tilemap, ref Tile3DData tileData) => throw new NotImplementedException();
	}
}
