using SFML.Window;

namespace ASTROMARINES.Other
{
    public struct WindowProperties
    {
        private static float _windowWidth = VideoMode.DesktopMode.Width;
        private static float _windowHeight = VideoMode.DesktopMode.Height;

        public static float WindowWidth { get => _windowWidth; set { _windowWidth = value; ScaleX = value / 1920.0f; } }
        public static float WindowHeight { get => _windowHeight; set { _windowHeight = value; ScaleY = value / 1080.0f; } }
        public static float ScaleX { get; private set; } = WindowWidth / 1920.0f;

        public static float ScaleY { get; private set; } = WindowHeight / 1080.0f;
    }
}
