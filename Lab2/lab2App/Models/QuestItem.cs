using InventorySystem.Enums;
using InventorySystem.Interfaces;

namespace InventorySystem.Models
{
    public class QuestItem : Item, IQuestItem, IUsable
    {
        public QuestItemType QuestItemType { get; private set; }
        public string EffectDescription { get; private set; }
        public int Quantity { get; set; }
        public int MaxStack { get; private set; }

        public QuestItem(string id, string name, QuestItemType questType, Rarity rarity,
                         string effectDescription, int price, int maxStack = 1)
            : base(id, name, ItemType.Quest, rarity, price)
        {
            QuestItemType = questType;
            EffectDescription = effectDescription;
            Quantity = 1;
            MaxStack = maxStack;
        }

        public bool CanUse()
        {
            return Quantity > 0;
        }

        public void UseSpecial()
        {
            if (CanUse())
            {
                Quantity--;
                Console.WriteLine($"Использован {Name}");
                Console.WriteLine($"Эффект: {EffectDescription}");

                if (Quantity == 0)
                {
                    Console.WriteLine($"{Name} теперь израсходован");
                }
            }
            else
            {
                Console.WriteLine($"{Name} израсходован");
            }
        }

        public void Use()
        {
            UseSpecial();
        }

        protected override string MakeDescription()
        {
            return $"{QuestItemType} - {EffectDescription} (Осталось: {Quantity}) - {Price} монет";
        }
    }
}