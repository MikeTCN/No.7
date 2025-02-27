﻿// Crest Ocean System

// Copyright 2021 Wave Harmonic Ltd

using Crest.Spline;
using UnityEngine;

namespace Crest
{
    /// <summary>
    /// Default data attached to all spline points
    /// </summary>
    [AddComponentMenu("")]
    public class SplinePointData : SplinePointDataBase
    {
        /// <summary>
        /// The version of this asset. Can be used to migrate across versions. This value should
        /// only be changed when the editor upgrades the version.
        /// </summary>
        [SerializeField, HideInInspector]
#pragma warning disable 414
        int _version = 0;
#pragma warning restore 414

        [Tooltip("Multiplier for spline radius."), SerializeField]
        [DecoratedField, OnChange(nameof(NotifyOfSplineChange))]
        float _radiusMultiplier = 1f;
        public float RadiusMultiplier { get => _radiusMultiplier; set => _radiusMultiplier = value; }

        // Currently returns (radius multiplier, nothing)
        public override Vector2 GetData()
        {
            return new Vector2(_radiusMultiplier, 0f);
        }
    }
}
