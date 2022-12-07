using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using CharacterClass = Stats.Entities;
namespace Stats.Entities
{
    public partial class CharacterAuthoring : MonoBehaviour
    {
        public CharacterClass Info;

    }

    class ChararacterStatBaker : Baker<CharacterAuthoring>
    {
        public override void Bake(CharacterAuthoring authoring)
        {
            BaseCharacterComponent character = new();
            character.SetupDataEntity(authoring.Info);
                AddComponentObject (character);
        }
    }
}
