using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography;
using MMIM.Common;
using MMIM.Enum;

namespace MMIM.Subcomponents
{
    public class CourseThemeSelector : SubcomponentBase
    {
        #region Constructor 

        public CourseThemeSelector()
        {
            System.Enum.GetValues(typeof (CourseThemes))
                .OfType<CourseThemes>()
                .Select(x => new Item(@"themes\" + x.ToString()))
                .ToList()
                .ForEach(item => CourseThemeItems.Add(item));
        }

        #endregion

        #region Fields

        private readonly ObservableCollection<Item> _courseThemeItems = new ObservableCollection<Item>();

        #endregion

        #region Properties

        public ObservableCollection<Item> CourseThemeItems
        {
            get { return _courseThemeItems; }
        }

        #endregion

        #region Methods

        public override void MakeRandomSelection(RNGCryptoServiceProvider rng)
        {
            if (CourseThemeItems.All(x => x.ToggleState != ItemToggleStates.Selected && x.ToggleState != ItemToggleStates.Enabled))
            {
                var rand = rng.GetRandomInt(1, 56);
                var numToPick = rand <= 21 ? 1
                    : rand <= 36 ? 2
                    : rand <= 46 ? 3
                    : rand <= 52 ? 4
                    : rand <= 55 ? 5
                    : 56;

                CourseThemeItems.Where(x => x.ToggleState == ItemToggleStates.Unset)
                    .OrderBy(x => rng.GetRandomInt(0, int.MaxValue))
                    .Take(numToPick)
                    .ToList()
                    .ForEach(x => x.ToggleState = ItemToggleStates.Selected);
            }

            CourseThemeItems.Where(x => x.ToggleState == ItemToggleStates.Unset)
                .ToList()
                .ForEach(item => item.ToggleState = ItemToggleStates.NotSelected);

            OnPropertyChanged();
        }

        public override void Reset()
        {
            CourseThemeItems.ToList().ForEach(item => item.Reset());
        }

        #endregion
    }
}
