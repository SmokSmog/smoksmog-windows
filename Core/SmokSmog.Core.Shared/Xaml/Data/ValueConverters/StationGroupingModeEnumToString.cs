using System;
using System.Globalization;

namespace SmokSmog.Xaml.Data.ValueConverters
{
    public class StationGroupingModeEnumToString : ValueConverterBase
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo cultureOrlanguage)

        {
            if (value is ViewModel.StationGroupingModeEnum)
            {
                switch ((ViewModel.StationGroupingModeEnum)value)
                {
                    case ViewModel.StationGroupingModeEnum.Name:
                        return Globalization.LocalizedStrings.LocalizedString("StationSortModeEnumName");

                    //case ViewModel.StationGroupingModeEnum.City:
                    //    return Globalization.LocalizedStrings("StationSortModeEnumCity");

                    case ViewModel.StationGroupingModeEnum.Province:
                        return Globalization.LocalizedStrings.LocalizedString("StationSortModeEnumProvince");
                }
            }
            return "String not found";
        }
    }
}