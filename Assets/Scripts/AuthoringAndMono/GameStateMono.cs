﻿using Unity.Entities;
using UnityEngine;

namespace CodeSamplesDOTS
{
    public class GameStateMono : MonoBehaviour { }

    class GameStateBaker : Baker<GameStateMono>
    {
        public override void Bake(GameStateMono authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);

            AddComponent<GameStateTag>(entity);
        }
    }
}
