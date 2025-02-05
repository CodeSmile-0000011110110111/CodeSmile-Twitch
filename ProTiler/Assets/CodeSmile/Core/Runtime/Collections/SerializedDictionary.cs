// Copyright (C) 2021-2023 Steffen Itterheim
// Refer to included LICENSE file for terms and conditions.

using CodeSmile.Attributes;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace CodeSmile.Collections
{
	[Serializable]
	[FullCovered]
	public class SerializedDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
	{
		[SerializeField] private List<TKey> m_Keys = new();
		[SerializeField] private List<TValue> m_Values = new();

		[ExcludeFromCodeCoverage]
		public void OnBeforeSerialize()
		{
			m_Keys.Clear();
			m_Values.Clear();

			foreach (var keyPair in this)
			{
				m_Keys.Add(keyPair.Key);
				m_Values.Add(keyPair.Value);
			}
		}

		[ExcludeFromCodeCoverage]
		public void OnAfterDeserialize()
		{
			Clear();
			for (var i = 0; i < m_Keys.Count; ++i)
				Add(m_Keys[i], m_Values[i]);

			m_Keys.Clear();
			m_Values.Clear();
		}
	}
}
