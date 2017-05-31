using SFML.Window;

namespace ASTROMARINES.Other
{
    public struct WindowProperties
    {
        private static float windowWidth = VideoMode.DesktopMode.Width;
        private static float windowHeight = VideoMode.DesktopMode.Height;
        private static float scaleX = WindowWidth / 1920.0f;
        private static float scaleY = WindowHeight / 1080.0f;

        public static float WindowWidth { get => windowWidth; set { windowWidth = value; scaleX = value / 1920.0f; } }
        public static float WindowHeight { get => windowHeight; set { windowHeight = value; scaleY = value / 1080.0f; } }
        public static float ScaleX => scaleX;
        public static float ScaleY => scaleY;
    }
}
