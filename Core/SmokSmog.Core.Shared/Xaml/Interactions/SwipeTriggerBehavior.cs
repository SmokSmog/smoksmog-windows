using Microsoft.Xaml.Interactivity;
using SmokSmog.Extensions;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;

namespace SmokSmog.Xaml.Interactions
{
    public enum SwipeDirection
    {
        Left,
        Right,
        Up,
        Down,
        LeftDown,
        LeftUp,
        RightDown,
        RightUp,
    }

    public class SwipeTriggerBehavior : Trigger<UIElement>
    {
        // Using a DependencyProperty as the backing store for SwipeDirection.
        public static readonly DependencyProperty DirectionProperty =
            DependencyProperty.Register("Direction", typeof(SwipeDirection), typeof(SwipeTriggerBehavior), new PropertyMetadata(SwipeDirection.Right, OnChange));

        public SwipeDirection Direction
        {
            get { return (SwipeDirection)GetValue(DirectionProperty); }
            set { SetValue(DirectionProperty, value); }
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.ManipulationMode = AssociatedObject.ManipulationMode | ManipulationModes.TranslateX | ManipulationModes.TranslateY;
            AssociatedObject.ManipulationCompleted += OnManipulationCompleted;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.ManipulationCompleted -= OnManipulationCompleted;
            base.OnDetaching();
        }

        private static void OnChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SwipeTriggerBehavior stb = d as SwipeTriggerBehavior;
        }

        private void OnManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            bool isRight = e.Velocities.Linear.X.Between(0.3, 100);
            bool isLeft = e.Velocities.Linear.X.Between(-100, -0.3);

            bool isUp = e.Velocities.Linear.Y.Between(-100, -0.3);
            bool isDown = e.Velocities.Linear.Y.Between(0.3, 100);

            bool excecute = false;

            switch (this.Direction)
            {
                case SwipeDirection.Left: excecute = (isLeft && !(isUp || isDown)); break;
                case SwipeDirection.Right: excecute = (isRight && !(isUp || isDown)); break;
                case SwipeDirection.Up: excecute = (isUp && !(isRight || isLeft)); break;
                case SwipeDirection.Down: excecute = (isDown && !(isRight || isLeft)); break;
                case SwipeDirection.LeftDown: excecute = (isLeft && isDown); break;
                case SwipeDirection.LeftUp: excecute = (isLeft && isUp); break;
                case SwipeDirection.RightDown: excecute = (isRight && isDown); break;
                case SwipeDirection.RightUp: excecute = (isRight && isUp); break;
                default: break;
            }

            if (excecute)
                Interaction.ExecuteActions(AssociatedObject, Actions, null);
        }
    }
}