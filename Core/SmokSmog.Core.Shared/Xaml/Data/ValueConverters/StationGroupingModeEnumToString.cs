using System;
using System.Globalization;

namespace SmokSmog.Xaml.Data.ValueConverters
{
    using Resources;

    public class StationGroupingModeEnumToString : ValueConverterBase
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo cultureOrlanguage)

        {
            if (value is ViewModel.StationGroupingModeEnum)
            {
                switch ((ViewModel.StationGroupingModeEnum)value)
                {
                    case ViewModel.StationGroupingModeEnum.Name:
                        return LocalizedStrings.GetString("StationSortModeEnumName");

                    //case ViewModel.StationGroupingModeEnum.City:
                    //    return Globalization.GetString("StationSortModeEnumCity");

                    case ViewModel.StationGroupingModeEnum.Province:
                        return LocalizedStrings.GetString("StationSortModeEnumProvince");
                }
            }
            return "String not found";
        }
    }
}