using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography;
using MMIM.Common;
using MMIM.Enum;

namespace MMIM.Subcomponents
{
    public class ItemsSelector : SubcomponentBase
    {
        #region Constructors

        public ItemsSelector(NumItemsSlider numItemsSlider, int numItems)
        {
            _numItemsSlider = numItemsSlider;
            for (var i = 1; i <= numItems; i++)
            {
                var item = new Item(@"items\" + i.ToString("00"));
                Items.Add(item);
            }
        }

        #endregion

        #region Fields

        private readonly NumItemsSlider _numItemsSlider;
        private readonly ObservableCollection<Item> _items = new ObservableCollection<Item>();

        #endregion

        #region Properties

        public ObservableCollection<Item> Items
        {
            get { return _items; }
        }

        private Item[] GroundItems
        {
            get
            {
                var category = ItemCategories.Ground.Select(c => c.ToString());
                return _items.Where(ItemMatches(category)).ToArray();
            }
        }

        private Item[] PowerupItems
        {
            get
            {
                var category = ItemCategories.Powerup.Select(c => c.ToString());
                return _items.Where(ItemMatches(category)).ToArray();
            }
        }

        private Item[] UtilityItems
        {
            get
            {
                var category = ItemCategories.Utility.Select(c => c.ToString());
                return _items.Where(ItemMatches(category)).ToArray();
            }
        }

        private Item[] EnemyItems
        {
            get
            {
                var category = ItemCategories.Enemy.Select(c => c.ToString());
                return _items.Where(ItemMatches(category)).ToArray();
            }
        }

        private Item[] HazardItems
        {
            get
            {
                var category = ItemCategories.Hazard.Select(c => c.ToString());
                return _items.Where(ItemMatches(category)).ToArray();
            }
        }

        private int TotalSelected
        {
            get { return Items.Count(item => item.ToggleState == ItemToggleStates.Selected || item.ToggleState == ItemToggleStates.Enabled); }
        }

        #endregion

        #region Methods

        private Func<Item, bool> ItemMatches(IEnumerable<string> category)
        {
            return item =>
            {
                var itemIdentifier = (Items.IndexOf(item) + 1).ToString();
                return category.Contains(itemIdentifier);
            };
        }

        public override void MakeRandomSelection(RNGCryptoServiceProvider rng)
        {
            //Select specific item categories
            SelectRandomNumItemsFrom(rng, _numItemsSlider.NumGround, GroundItems);
            SelectRandomNumItemsFrom(rng, _numItemsSlider.NumPowerups, PowerupItems);
            SelectRandomNumItemsFrom(rng, _numItemsSlider.NumUtilities, UtilityItems);
            SelectRandomNumItemsFrom(rng, _numItemsSlider.NumEnemies, EnemyItems);
            SelectRandomNumItemsFrom(rng, _numItemsSlider.NumHazards, HazardItems);

            //Select remaining items
            SelectRandomNumItemsFrom(rng, _numItemsSlider.NumItems, Items.ToArray());

            //Mark unset items
            Items.Where(x => x.ToggleState == ItemToggleStates.Unset)
                .ToList()
                .ForEach(item => item.ToggleState = ItemToggleStates.NotSelected);

            OnPropertyChanged();
        }

        private void SelectRandomNumItemsFrom(RNGCryptoServiceProvider rng, int itemsToSelect, Item[] items)
        {
            var numAlreadySelected = items.Count(item => item.ToggleState == ItemToggleStates.Selected || item.ToggleState == ItemToggleStates.Enabled);
            var itemsRemaning = itemsToSelect - numAlreadySelected;

            if (itemsRemaning > _numItemsSlider.NumItems - TotalSelected)
                itemsRemaning = _numItemsSlider.NumItems - TotalSelected;
            
            if (itemsRemaning <= 0) return;

            items.Where(x => x.ToggleState == ItemToggleStates.Unset)
                .OrderBy(x => rng.GetRandomInt(0, int.MaxValue))
                .Take(itemsRemaning)
                .ToList()
                .ForEach(x => x.ToggleState = ItemToggleStates.Selected);
        }

        public override void Reset()
        {
            Items.ToList().ForEach(item => item.Reset());
        }

        #endregion
    }
}
