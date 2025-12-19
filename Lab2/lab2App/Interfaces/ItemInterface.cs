using InventorySystem.Enums;

namespace InventorySystem.Interfaces
{
    public interface IItem
    {
        string Id { get; }
        string Name { get; }
        string Description { get; }
        ItemType Type { get; }
        Rarity Rarity { get; }
        int Price { get; }
    }

    public interface IEquipable : IItem
    {
        EquipmentSlot Slot { get; }
        bool IsEquipped { get; set; }
        void Equip();
        void Unequip();
    }

    public interface IUsable : IItem
    {
        int Quantity { get; set; }
        int MaxStack { get; }
        void Use();
    }

    public interface IUpgradable : IItem
    {
        int Level { get; }
        int MaxLevel { get; }
        void Upgrade();
        bool CanUpgrade();
    }

    public interface IQuestItem : IItem
    {
        QuestItemType QuestItemType { get; }
        string EffectDescription { get; }
        bool CanUse();
        void UseSpecial();
    }
}