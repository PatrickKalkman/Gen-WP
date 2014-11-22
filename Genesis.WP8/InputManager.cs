using Genesis.Common;

namespace Genesis.WP8
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Input;
    using Microsoft.Xna.Framework.Input.Touch;

    public class InputManager : IInputManager
    {
        private bool cDownIsAvailable;

        private bool bDownIsAvailable;

        private bool dDownIsAvailable;

        private bool tapped;

        public bool DragAvailable { get; set; }

        public void Initialize()
        {
            TouchPanel.EnabledGestures = GestureType.Tap | GestureType.FreeDrag;
        }

        public void GetTouchInput()
        {
            Delta = Vector2.Zero;
            DragAvailable = false;
            tapped = false;
            while (TouchPanel.IsGestureAvailable)
            {
                GestureSample gesture = TouchPanel.ReadGesture();
                if (gesture.GestureType == GestureType.Tap)
                {
                    tapped = true;
                }

                if (gesture.GestureType == GestureType.FreeDrag)
                {
                    Delta = new Vector2(gesture.Delta.X, gesture.Delta.Y);
                    DragAvailable = true;
                }
            }
        }

        public Vector2 Delta { get; set; }

        public bool ShouldStart()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Space) || tapped)
            {
                tapped = !tapped;
                return true;
            }

            return false;
        }

        public bool ShouldMoveUp()
        {
            return Keyboard.GetState().IsKeyDown(Keys.Up);
        }

        public bool ShouldMoveDown()
        {
            return Keyboard.GetState().IsKeyDown(Keys.Down);
        }

        public bool ShouldMoveLeft()
        {
            return Keyboard.GetState().IsKeyDown(Keys.Left);
        }

        public bool ShouldMoveRight()
        {
            return Keyboard.GetState().IsKeyDown(Keys.Right);
        }

        public bool NoInput()
        {
            return !(this.ShouldMoveDown() || this.ShouldMoveUp() || this.ShouldMoveLeft() || this.ShouldMoveRight());
        }

        public bool HasDrawCurveKeyBeenPressed()
        {
            if (cDownIsAvailable)
            {
                if (Keyboard.GetState().IsKeyUp(Keys.C))
                {
                    cDownIsAvailable = false;
                    return true;
                }
            }

            if (Keyboard.GetState().IsKeyDown(Keys.C))
            {
                cDownIsAvailable = true;
            }

            return false;
        }

        public bool HasBulletToggleButtonBeenPress()
        {
            if (bDownIsAvailable)
            {
                if (Keyboard.GetState().IsKeyUp(Keys.B))
                {
                    bDownIsAvailable = false;
                    return true;
                }
            }

            if (Keyboard.GetState().IsKeyDown(Keys.B))
            {
                bDownIsAvailable = true;
            }

            return false;
        }

        public bool HasDebugKeyBeenPressed()
        {
            if (dDownIsAvailable)
            {
                if (Keyboard.GetState().IsKeyUp(Keys.D))
                {
                    dDownIsAvailable = false;
                    return true;
                }
            }

            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                dDownIsAvailable = true;
            }

            return false;
        }
    }
}
