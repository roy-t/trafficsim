using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ImGuiNET;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TrafficSim.Network;
using TrafficSim.PathFinding;
using TrafficUI;

namespace TrafficSim
{
    public sealed class GameLoop : Game
    {
        private readonly GraphicsDeviceManager Graphics;
        private NetworkVisualization networkVisualization;
        private ImGuiRenderer gui;

        private int currentItem;

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

            this.Content.RootDirectory = Path.Combine(Directory.GetCurrentDirectory(), @"..\Content");
            this.IsMouseVisible = true;
        }

        protected override void LoadContent()
        {
            var uiEffect = this.Content.Load<Effect>("UIEffect");
            this.gui = new ImGuiRenderer(this, new UIEffect(uiEffect));
            this.networkVisualization = new NetworkVisualization(this.GraphicsDevice);

            this.GenerateRoad();

            base.LoadContent();
        }


        private void GenerateRoad()
        {
            var random = new Random();

            var parts = random.Next(3, 15);

            var start = new Road(new Vector2(0, -3.75f), new Vector2(0, -2.75f));
            this.networkVisualization.Add(start);

            var queue = new Queue<Road>();
            queue.Enqueue(start);
            for (var i = 0; i < parts; i++)
            {
                var previous = queue.Dequeue();

                var angle = (random.NextDouble() * MathHelper.PiOver2) + MathHelper.PiOver4;

                var x = (float)Math.Cos(angle);
                var y = (float)Math.Sin(angle);

                var next = previous.Add(previous.End + new Vector2(x, y));
                this.networkVisualization.Add(next);

                if (random.NextDouble() > 0.5)
                {
                    var alt = previous.Add(previous.End + new Vector2(-x, y));
                    this.networkVisualization.Add(alt);
                    queue.Enqueue(alt);
                    i++;
                }

                queue.Enqueue(next);
            }
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                this.Exit();
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            this.GraphicsDevice.Clear(Color.CornflowerBlue);

            this.networkVisualization.Draw();

            this.gui.BeginLayout(gameTime);

            ImGui.BeginMainMenuBar();
            {
                if (ImGui.BeginMenu("Exit"))
                {
                    this.Exit();
                    ImGui.EndMenu();
                }

                if (ImGui.Begin("Network Overview"))
                {
                    if (ImGui.Button("Generate"))
                    {
                        this.networkVisualization.Clear();
                        this.currentItem = 0;
                        this.GenerateRoad();
                    }

                    var roads = this.networkVisualization.All();
                    if (ImGui.ListBox("Roads", ref this.currentItem, roads.Select(x => $"{x.Start} -> {x.End}").ToArray(), roads.Count, Math.Min(roads.Count, 15)))
                    {
                        this.networkVisualization.ClearHighLights();
                        this.networkVisualization.Highlight(roads[this.currentItem]);
                    }

                    if (ImGui.Button("Plan Route"))
                    {
                        this.networkVisualization.ClearHighLights();
                        var path = PathFinder.FindPath(roads[0], roads[this.currentItem]);
                        for (var i = 0; i < path.Count; i++)
                        {
                            this.networkVisualization.Highlight(path[i]);
                        }
                    }

                    ImGui.End();
                }
            }
            ImGui.EndMainMenuBar();
            //ImGui.ShowDemoWindow();

            this.gui.EndLayout();

            base.Draw(gameTime);
        }
    }
}
