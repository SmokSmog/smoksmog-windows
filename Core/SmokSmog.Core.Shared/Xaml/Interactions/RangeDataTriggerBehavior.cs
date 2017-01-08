using Microsoft.Xaml.Interactions.Core;
using Microsoft.Xaml.Interactivity;
using System;
using System.Globalization;
using Windows.UI.Xaml;

namespace SmokSmog.Xaml.Interactions
{
    public enum LogicalConditionType
    {
        And,
        Or
    }

    /// <summary>
    /// A behavior that performs actions when the bound data meets a specified condition.
    /// </summary>
    public sealed class RangeDataTriggerBehavior : Trigger
    {
        /// <summary>
        /// Identifies the <seealso cref="Binding"/> dependency property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly DependencyProperty BindingProperty = DependencyProperty.Register(
            nameof(Binding),
            typeof(object),
            typeof(RangeDataTriggerBehavior),
            new PropertyMetadata(null, new PropertyChangedCallback(RangeDataTriggerBehavior.OnValueChanged)));

        /// <summary>
        /// Gets or sets the bound object that the <see cref="RangeDataTriggerBehavior"/> will listen
        /// to. This is a dependency property.
        /// </summary>
        public object Binding
        {
            get
            {
                return (object)this.GetValue(RangeDataTriggerBehavior.BindingProperty);
            }
            set
            {
                this.SetValue(RangeDataTriggerBehavior.BindingProperty, value);
            }
        }

        /// <summary>
        /// Identifies the <seealso cref="ComparisonCondition"/> dependency property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly DependencyProperty FirstComparisonConditionProperty = DependencyProperty.Register(
            nameof(FirstComparisonCondition),
            typeof(ComparisonConditionType),
            typeof(RangeDataTriggerBehavior),
            new PropertyMetadata(ComparisonConditionType.Equal,
                new PropertyChangedCallback(RangeDataTriggerBehavior.OnValueChanged)));

        /// <summary>
        /// Gets or sets the type of comparison to be performed between
        /// <see cref="RangeDataTriggerBehavior.Binding"/> and
        /// <see cref="RangeDataTriggerBehavior.FirstValue"/>. This is a dependency property.
        /// </summary>
        public ComparisonConditionType FirstComparisonCondition
        {
            get
            {
                return (ComparisonConditionType)this.GetValue(RangeDataTriggerBehavior.FirstComparisonConditionProperty);
            }
            set
            {
                this.SetValue(RangeDataTriggerBehavior.FirstComparisonConditionProperty, value);
            }
        }

        /// <summary>
        /// Identifies the <seealso cref="ComparisonCondition"/> dependency property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly DependencyProperty SecondComparisonConditionProperty = DependencyProperty.Register(
            nameof(SecondComparisonCondition),
            typeof(ComparisonConditionType),
            typeof(RangeDataTriggerBehavior),
            new PropertyMetadata(ComparisonConditionType.Equal,
                new PropertyChangedCallback(RangeDataTriggerBehavior.OnValueChanged)));

        /// <summary>
        /// Gets or sets the type of comparison to be performed between
        /// <see cref="RangeDataTriggerBehavior.Binding"/> and
        /// <see cref="RangeDataTriggerBehavior.SecondValue"/>. This is a dependency property.
        /// </summary>
        public ComparisonConditionType SecondComparisonCondition
        {
            get
            {
                return (ComparisonConditionType)this.GetValue(RangeDataTriggerBehavior.SecondComparisonConditionProperty);
            }
            set
            {
                this.SetValue(RangeDataTriggerBehavior.SecondComparisonConditionProperty, value);
            }
        }

        /// <summary>
        /// Identifies the <seealso cref="FirstValue"/> dependency property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly DependencyProperty FirstValueProperty = DependencyProperty.Register(
            nameof(FirstValue),
            typeof(object),
            typeof(RangeDataTriggerBehavior),
            new PropertyMetadata(null, new PropertyChangedCallback(RangeDataTriggerBehavior.OnValueChanged)));

        /// <summary>
        /// Gets or sets the value to be compared with the value of
        /// <see cref="RangeDataTriggerBehavior.Binding"/>. This is a dependency property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1721:PropertyNamesShouldNotMatchGetMethods")]
        public object FirstValue
        {
            get
            {
                return (object)this.GetValue(RangeDataTriggerBehavior.FirstValueProperty);
            }
            set
            {
                this.SetValue(RangeDataTriggerBehavior.FirstValueProperty, value);
            }
        }

        /// <summary>
        /// Identifies the <seealso cref="SecondValue"/> dependency property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly DependencyProperty SecondValueProperty = DependencyProperty.Register(
            nameof(SecondValue),
            typeof(object),
            typeof(RangeDataTriggerBehavior),
            new PropertyMetadata(null, new PropertyChangedCallback(RangeDataTriggerBehavior.OnValueChanged)));

        /// <summary>
        /// Gets or sets the value to be compared with the value of
        /// <see cref="RangeDataTriggerBehavior.Binding"/>. This is a dependency property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1721:PropertyNamesShouldNotMatchGetMethods")]
        public object SecondValue
        {
            get
            {
                return (object)this.GetValue(RangeDataTriggerBehavior.SecondValueProperty);
            }
            set
            {
                this.SetValue(RangeDataTriggerBehavior.SecondValueProperty, value);
            }
        }

        public LogicalConditionType LogicalCondition
        {
            get { return (LogicalConditionType)GetValue(LogicalConditionProperty); }
            set { SetValue(LogicalConditionProperty, value); }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1721:PropertyNamesShouldNotMatchGetMethods")]
        public static readonly DependencyProperty LogicalConditionProperty = DependencyProperty.Register(
            nameof(LogicalCondition),
            typeof(LogicalConditionType),
            typeof(RangeDataTriggerBehavior),
            new PropertyMetadata(LogicalConditionType.And, new PropertyChangedCallback(RangeDataTriggerBehavior.OnValueChanged)));

        private static void OnValueChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            RangeDataTriggerBehavior dataTriggerBehavior = (RangeDataTriggerBehavior)dependencyObject;
            if (dataTriggerBehavior.AssociatedObject == null)
            {
                return;
            }

            DataBindingHelper.RefreshDataBindingsOnActions(dataTriggerBehavior.Actions);

            // Some value has changed--either the binding value, reference value, or the comparison
            // condition. Re-evaluate the equation.
            if (RangeDataTriggerBehavior.Compare(
                dataTriggerBehavior.Binding,
                dataTriggerBehavior.FirstComparisonCondition, dataTriggerBehavior.FirstValue,
                dataTriggerBehavior.SecondComparisonCondition, dataTriggerBehavior.SecondValue,
                dataTriggerBehavior.LogicalCondition))
            {
                Interaction.ExecuteActions(dataTriggerBehavior.AssociatedObject, dataTriggerBehavior.Actions, args);
            }
        }

        private static bool Compare(
            object leftOperand,
            ComparisonConditionType firstEqualityOperatorType, object firstRightOperand,
            ComparisonConditionType secondEqualityOperatorType, object secondRightOperand,
            LogicalConditionType logicalOperatorType)
        {
            bool firstOperation = Compare(leftOperand, firstEqualityOperatorType, firstRightOperand);
            bool secondOperation = Compare(leftOperand, secondEqualityOperatorType, secondRightOperand);

            switch (logicalOperatorType)
            {
                case LogicalConditionType.And:
                    return firstOperation && secondOperation;

                case LogicalConditionType.Or:
                    return firstOperation || secondOperation;

                default:
                    return false;
            }
        }

        private static bool Compare(object leftOperand, ComparisonConditionType operatorType, object rightOperand)
        {
            if (leftOperand != null && rightOperand != null)
            {
                rightOperand = TypeConverterHelper.Convert(rightOperand.ToString(), leftOperand.GetType().FullName);
            }

            IComparable leftComparableOperand = leftOperand as IComparable;
            IComparable rightComparableOperand = rightOperand as IComparable;
            if ((leftComparableOperand != null) && (rightComparableOperand != null))
            {
                return RangeDataTriggerBehavior.EvaluateComparable(leftComparableOperand, operatorType, rightComparableOperand);
            }

            switch (operatorType)
            {
                case ComparisonConditionType.Equal:
                    return object.Equals(leftOperand, rightOperand);

                case ComparisonConditionType.NotEqual:
                    return !object.Equals(leftOperand, rightOperand);

                case ComparisonConditionType.LessThan:
                case ComparisonConditionType.LessThanOrEqual:
                case ComparisonConditionType.GreaterThan:
                case ComparisonConditionType.GreaterThanOrEqual:
                    {
                        if (leftComparableOperand == null && rightComparableOperand == null)
                        {
                            throw new ArgumentException(string.Format(
                                CultureInfo.CurrentCulture,
                                "InvalidOperands",
                                leftOperand != null ? leftOperand.GetType().Name : "null",
                                rightOperand != null ? rightOperand.GetType().Name : "null",
                                operatorType.ToString()));
                        }
                        else if (leftComparableOperand == null)
                        {
                            throw new ArgumentException(string.Format(
                                CultureInfo.CurrentCulture,
                                "InvalidLeftOperand",
                                leftOperand != null ? leftOperand.GetType().Name : "null",
                                operatorType.ToString()));
                        }
                        else
                        {
                            throw new ArgumentException(string.Format(
                                CultureInfo.CurrentCulture,
                                "InvalidRightOperand",
                                rightOperand != null ? rightOperand.GetType().Name : "null",
                                operatorType.ToString()));
                        }
                    }
            }

            return false;
        }

        /// <summary>
        /// Evaluates both operands that implement the IComparable interface.
        /// </summary>
        private static bool EvaluateComparable(IComparable leftOperand, ComparisonConditionType operatorType, IComparable rightOperand)
        {
            object convertedOperand = null;
            try
            {
                convertedOperand = Convert.ChangeType(rightOperand, leftOperand.GetType(), CultureInfo.CurrentCulture);
            }
            catch (FormatException)
            {
                // FormatException: Convert.ChangeType("hello", typeof(double), ...);
            }
            catch (InvalidCastException)
            {
                // InvalidCastException: Convert.ChangeType(4.0d, typeof(Rectangle), ...);
            }

            if (convertedOperand == null)
            {
                return operatorType == ComparisonConditionType.NotEqual;
            }

            int comparison = leftOperand.CompareTo((IComparable)convertedOperand);
            switch (operatorType)
            {
                case ComparisonConditionType.Equal:
                    return comparison == 0;

                case ComparisonConditionType.NotEqual:
                    return comparison != 0;

                case ComparisonConditionType.LessThan:
                    return comparison < 0;

                case ComparisonConditionType.LessThanOrEqual:
                    return comparison <= 0;

                case ComparisonConditionType.GreaterThan:
                    return comparison > 0;

                case ComparisonConditionType.GreaterThanOrEqual:
                    return comparison >= 0;
            }

            return false;
        }
    }
}