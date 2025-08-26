using System;
using System.Collections.Generic;
using System.Linq;

namespace Dynastio.Net
{
    /// <summary>
    /// Represents a player's personal chest (inventory storage) in Dynast.io.
    /// Holds a collection of <see cref="PersonalChestItem"/> objects and
    /// provides utility methods for quick lookup.
    /// </summary>
    public class PersonalChest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PersonalChest"/> class with the given items.
        /// </summary>
        /// <param name="items">A list of items stored in the personal chest.</param>
        public PersonalChest(List<PersonalChestItem> items)
        {
            Items = items ?? new List<PersonalChestItem>();
        }

        /// <summary>
        /// The list of items currently in the chest.
        /// </summary>
        public List<PersonalChestItem> Items { get; set; } = new List<PersonalChestItem>();

        /// <summary>
        /// Converts the chest's item list into a dictionary keyed by the item's slot index.
        /// </summary>
        /// <returns>
        /// A dictionary where:
        /// <list type="bullet">
        ///     <item><description>Key = Item slot index</description></item>
        ///     <item><description>Value = The <see cref="PersonalChestItem"/> in that slot</description></item>
        /// </list>
        /// </returns>
        public Dictionary<int, PersonalChestItem> GetAsDictionary()
        {
            var chestItems = new Dictionary<int, PersonalChestItem>();

            foreach (var item in Items)
            {
                // Use safe PascalCase property reference
                // Avoids exception if an index is duplicated
                if (!chestItems.ContainsKey(item.Index))
                {
                    chestItems.Add(item.Index, item);
                }
            }

            return chestItems;
        }
    }
}
