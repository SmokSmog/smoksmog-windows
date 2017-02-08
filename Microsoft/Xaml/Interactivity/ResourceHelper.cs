#if !WINDOWS_UWP

// Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license. See LICENSE file in
// the project root for full license information.
namespace Microsoft.Xaml.Interactivity
{
    internal static class ResourceHelper
    {
        public static string GetString(string resourceName)
        {
            //ResourceLoader strings = ResourceLoader.GetForCurrentView("Microsoft.Xaml.Interactivity/Strings");
            //return strings.GetString(resourceName);
            return resourceName;
        }

        public static string CallMethodActionValidMethodNotFoundExceptionMessage
            => ResourceHelper.GetString("CallMethodActionValidMethodNotFoundExceptionMessage");

        public static string ChangePropertyActionCannotFindPropertyNameExceptionMessage
            => ResourceHelper.GetString("ChangePropertyActionCannotFindPropertyNameExceptionMessage");

        public static string ChangePropertyActionCannotSetValueExceptionMessage
            => ResourceHelper.GetString("ChangePropertyActionCannotSetValueExceptionMessage");

        public static string ChangePropertyActionPropertyIsReadOnlyExceptionMessage
            => ResourceHelper.GetString("ChangePropertyActionPropertyIsReadOnlyExceptionMessage");

        public static string GoToStateActionTargetHasNoStateGroups
            => ResourceHelper.GetString("GoToStateActionTargetHasNoStateGroups");

        public static string CannotAttachBehaviorMultipleTimesExceptionMessage
            => ResourceHelper.GetString("CannotAttachBehaviorMultipleTimesExceptionMessage");

        public static string CannotFindEventNameExceptionMessage
            => ResourceHelper.GetString("CannotFindEventNameExceptionMessage");

        public static string InvalidLeftOperand
            => ResourceHelper.GetString("InvalidLeftOperand");

        public static string InvalidRightOperand
            => ResourceHelper.GetString("InvalidRightOperand");

        public static string InvalidOperands
            => ResourceHelper.GetString("InvalidOperands");
    }
}

#endif