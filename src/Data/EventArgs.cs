using System;

namespace Locksmith.Data
{
    public class ItemInfoEventArgs : EventArgs
    {
        public ItemInfoEventArgs(SolKeyLevelElement itemElement)
        {
            if (itemElement == null)
            {
                IsHidden = false;
                IsInBlock = false;
            }
            else
            {
                ItemName = itemElement.Name;
                IsHidden = Utility.IsItemHidden(itemElement.ElementIndex);
                IsInBlock = Utility.IsItemInBlock(itemElement.ElementIndex);
            }
        }

        public String ItemName { get; set; }
        public Boolean IsHidden { get; set; }
        public Boolean IsInBlock { get; set; }
        public int xPosition { get; set; }
        public int yPosition { get; set; }
    }

    public class LevelChangeEventArgs : EventArgs
    {
        public LevelChangeEventArgs(int Level)
        {
            NewLevel = Level;
        }

        public int NewLevel { get; set; }
    }
}