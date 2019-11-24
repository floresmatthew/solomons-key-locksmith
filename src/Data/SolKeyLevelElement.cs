using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Locksmith.Data
{
    public enum ElementTypes
    {
        SpawnPoint,
        Key,
        Exit,
        Item,
        Enemy
    }

    public class SolKeyLevelElement
    {
        public SolKeyLevelElement()
        {
        }

        public SolKeyLevelElement(ElementTypes ElementType, byte Index, byte Position)
        {
            this.ElementType = ElementType;
            this.Position = Position;
            this.ElementIndex = Index;

            Name = GetElementName();
        }

        private string GetElementName()
        {
            if (this.ElementType == ElementTypes.Item)
                return SolRom.GetItemName(this.ElementIndex);
            else if (this.ElementType == ElementTypes.Enemy)
                return SolRom.GetEnemyName(this.ElementIndex);
            return null;
        }

        public ElementTypes ElementType { get; set; }
        public byte Position { get; set; }
        public byte ElementIndex { get; set; }
        public String Name { get; set; }
        public GameTiles Tile { get; set; }
    }   
}
