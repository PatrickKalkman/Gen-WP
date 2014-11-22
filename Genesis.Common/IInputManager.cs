using Microsoft.Xna.Framework;

namespace Genesis.Common
{
    public interface IInputManager
    {
        bool DragAvailable { get; set; }

        Vector2 Delta { get; set; }

        bool ShouldMoveUp();

        bool ShouldMoveDown();

        bool ShouldMoveLeft();

        bool ShouldMoveRight();

        bool NoInput();

        bool HasDrawCurveKeyBeenPressed();

        bool HasBulletToggleButtonBeenPress();

        bool HasDebugKeyBeenPressed();

        bool ShouldStart();

        void Initialize();

        void GetTouchInput();
    }
}