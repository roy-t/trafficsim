using System;
using System.IO;
using System.Linq;
using ImGuiNET;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TrafficSim.Extensions;
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
        private PathFinderResult pathFinderResult;

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

            this.GenerateManhattan();

            base.LoadContent();
        }

        private void GenerateManhattan()
        {
            var random = new Random();

            var offset = new Vector2(-3.75f, -3.75f);

            var columns = 10;
            var rows = 8;
            var junctions = new Junction[columns, rows];

            for (var x = 0; x < columns; x++)
            {
                for (var y = 0; y < rows; y++)
                {
                    junctions[x, y] = new Junction(offset + new Vector2(x, y));
                }
            }

            for (var x = 0; x < columns - 1; x++)
            {
                for (var y = 0; y < rows - 1; y++)
                {
                    if (random.NextBool(0.9f))
                    {
                        var a = junctions[x, y].ConnectWith(junctions[x + 1, y], random.NextFloat(Road.MinSpeedLimit, Road.MaxSpeedLimit));
                        this.networkVisualization.Add(a);
                    }

                    if (random.NextBool(0.8f))
                    {
                        var b = junctions[x, y].ConnectWith(junctions[x, y + 1], random.NextFloat(Road.MinSpeedLimit, Road.MaxSpeedLimit));
                        this.networkVisualization.Add(b);
                    }
                }
            }

            for (var x = 0; x < columns - 1; x++)
            {
                if (random.NextBool(0.8f))
                {
                    var c = junctions[x, rows - 1].ConnectWith(junctions[x + 1, rows - 1], random.NextFloat(Road.MinSpeedLimit, Road.MaxSpeedLimit));
                    this.networkVisualization.Add(c);
                }
            }

            for (var y = 0; y < rows - 1; y++)
            {
                if (random.NextBool(0.8f))
                {
                    var c = junctions[columns - 1, y].ConnectWith(junctions[columns - 1, y + 1], random.NextFloat(Road.MinSpeedLimit, Road.MaxSpeedLimit));
                    this.networkVisualization.Add(c);
                }
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
                    if (ImGui.Button("Generate Grid"))
                    {
                        this.networkVisualization.Clear();
                        this.currentItem = 0;
                        this.GenerateManhattan();
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
                        this.pathFinderResult = PathFinder.FindPath(roads[0].StartJunction, roads[this.currentItem].StartJunction);
                        if (this.pathFinderResult.FoundPath)
                        {
                            for (var i = 0; i < this.pathFinderResult.Path.Count; i++)
                            {
                                this.networkVisualization.Highlight(this.pathFinderResult.Path[i]);
                            }
                        }
                    }

                    if (this.pathFinderResult != null && this.pathFinderResult.FoundPath)
                    {
                        ImGui.Text($"Cost: {this.pathFinderResult.Cost}s");
                        ImGui.Text($"Steps: {this.pathFinderResult.Path.Count}");
                        ImGui.Text($"Average Speed: {this.pathFinderResult.Path.Average(x => x.SpeedLimit):F2}Km/h");
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
