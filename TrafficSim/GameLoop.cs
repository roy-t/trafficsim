using System.IO;
using ImGuiNET;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TrafficUI;

namespace TrafficSim
{
    public sealed class GameLoop : Game
    {
        private readonly GraphicsDeviceManager Graphics;
        private ImGuiRenderer gui;

        public GameLoop()
        {
            this.Graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = 1920,
                PreferredBackBufferHeight = 1080,
                PreferMultiSampling = true,
                SynchronizeWithVerticalRetrace = false,
                GraphicsProfile = GraphicsProfile.HiDef
            };

            this.Content.RootDirectory = Directory.GetCurrentDirectory() + @"\..\Content";
            this.IsMouseVisible = true;
        }

        protected override void LoadContent()
        {
            var uiEffect = this.Content.Load<Effect>("UIEffect");
            this.gui = new ImGuiRenderer(this, new UIEffect(uiEffect));

            base.LoadContent();
        }

        protected override void Draw(GameTime gameTime)
        {
            this.GraphicsDevice.Clear(Color.CornflowerBlue);

            this.gui.BeginLayout(gameTime);

            ImGui.ShowDemoWindow();

            this.gui.EndLayout();

            base.Draw(gameTime);
        }
    }
}
