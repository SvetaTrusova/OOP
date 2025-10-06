using System;
using System.Collections.Generic;
using System.Linq;

namespace VendingMachineApp
{
    public class CoinRegister
    {
        private readonly SortedDictionary<int, int> coinInventory;

        public CoinRegister()
        {
            coinInventory = new SortedDictionary<int, int>(Comparer<int>.Create((a, b) => b.CompareTo(a)));
            foreach (var d in Coins.CoinRegistry)
                coinInventory[d] = 0;
        }

        public void LoadCoins(int denomination, int count)
        {
            if (!coinInventory.ContainsKey(denomination))
                coinInventory[denomination] = 0;
            coinInventory[denomination] += count;
        }

        public IReadOnlyDictionary<int, int> Snapshot() => coinInventory;

        public void AcceptInserted(IDictionary<int, int> inserted)
        {
            foreach (var kv in inserted)
            {
                if (!coinInventory.ContainsKey(kv.Key)) coinInventory[kv.Key] = 0;
                coinInventory[kv.Key] += kv.Value;
            }
        }
        public bool TryMakeChange(int amount, IDictionary<int, int> optionalAdded, out Dictionary<int, int> change)
        {
            change = new Dictionary<int, int>();
            if (amount == 0) return true;

            var temp = coinInventory.ToDictionary(k => k.Key, v => v.Value);
            if (optionalAdded != null)
            {
                foreach (var kv in optionalAdded)
                {
                    if (!temp.ContainsKey(kv.Key)) temp[kv.Key] = 0;
                    temp[kv.Key] += kv.Value;
                }
            }

            foreach (var denom in temp.Keys.OrderByDescending(x => x))
            {
                int need = amount / denom;
                if (need <= 0) continue;
                int take = Math.Min(need, temp[denom]);
                if (take > 0)
                {
                    change[denom] = take;
                    amount -= take * denom;
                }
                if (amount == 0) break;
            }

            if (amount != 0)
            {
                change.Clear();
                return false;
            }

            return true;
        }

        public void DispenseChange(IDictionary<int, int> change)
        {
            foreach (var kv in change)
            {
                if (!coinInventory.ContainsKey(kv.Key) || coinInventory[kv.Key] < kv.Value)
                    throw new InvalidOperationException("Попытка выдать несуществующую сдачу");
                coinInventory[kv.Key] -= kv.Value;
            }
        }

        public int CollectAll(out Dictionary<int, int> collectedCoins)
        {
            collectedCoins = new Dictionary<int, int>(coinInventory);
            int total = coinInventory.Sum(kv => kv.Key * kv.Value);
            foreach (var k in coinInventory.Keys.ToList()) coinInventory[k] = 0;
            return total;
        }

        public override string ToString()
        {
            return string.Join(", ", coinInventory.OrderByDescending(k => k.Key).Select(kv => $"{kv.Key}₽ x{kv.Value}"));
        }
    }
}
