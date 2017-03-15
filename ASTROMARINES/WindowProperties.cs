namespace ASTROMARINES
{
    public struct WindowProperties
    {
        private static float windowWidth = SFML.Window.VideoMode.DesktopMode.Width;
        private static float windowHeight = SFML.Window.VideoMode.DesktopMode.Height;
        private static float scaleX = WindowWidth / 1920.0f;
        private static float scaleY = WindowHeight / 1080.0f;

        public static float WindowWidth { get => windowWidth; set { windowWidth = value; scaleX = WindowWidth / 1920.0f; } }
        public static float WindowHeight { get => windowHeight; set { windowHeight = value; scaleY = WindowHeight / 1080.0f; } }
        public static float ScaleX { get => scaleX; }
        public static float ScaleY { get => scaleY; }
    }
}
