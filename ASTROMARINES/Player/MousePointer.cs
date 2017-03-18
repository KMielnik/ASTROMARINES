﻿using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace ASTROMARINES
{
    public class MousePointer
    {
        CircleShape target;
        RectangleShape targetLineX;
        RectangleShape targetLineY;
        public MousePointer()
        {
            target = new CircleShape(3);
            target.Origin = new Vector2f(3, 3);
            target.FillColor = new Color(Color.Red);
            target.OutlineThickness = 1;
            target.OutlineColor = new Color(Color.Black);

            targetLineX = new RectangleShape(new Vector2f(2 * WindowProperties.WindowWidth, 1));
            targetLineX.Origin = new Vector2f(WindowProperties.WindowWidth, 0.5f);
            targetLineX.FillColor = new Color(Color.Red);

            targetLineY = new RectangleShape(new Vector2f(1, 2 * WindowProperties.WindowHeight));
            targetLineX.Origin = new Vector2f(0.5f, WindowProperties.WindowHeight);
            targetLineX.FillColor = new Color(Color.Red);
        }

        void Draw(RenderWindow window)
        {
            Vector2f mousePosition = (Vector2f)Mouse.GetPosition();
            target.Position = mousePosition;
            targetLineX.Position = mousePosition;
            targetLineY.Position = mousePosition;

            window.Draw(target);
            window.Draw(targetLineX);
            window.Draw(targetLineY);
        }

        void HoversOverItemON()
        {
            target.FillColor = new Color(Color.Green);
            targetLineX.FillColor = new Color(Color.Green);
            targetLineY.FillColor = new Color(Color.Green);
        }

        void HoversOverItemOFF()
        {
            target.FillColor = new Color(Color.Red);
            targetLineX.FillColor = new Color(Color.Red);
            targetLineY.FillColor = new Color(Color.Red);
        }
    }
}