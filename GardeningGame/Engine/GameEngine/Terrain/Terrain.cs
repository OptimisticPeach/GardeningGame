﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using GardeningGame.Engine.Scenes.Common;

namespace GardeningGame.Engine.Scenes.Game.Terrain
{
    public abstract class Terrain
    {
        /// <summary>
        /// The vertex buffer used for drawing
        /// </summary>
        public virtual VertexBuffer VertexBuffer { get; protected set; }
        /// <summary>
        /// The index buffer to specify indices in the vertex buffer
        /// </summary>

        public Color ID { get; set; }

        public Terrain()
        {
        }

        /// <summary>
        /// A generic drawing function for terrain
        /// </summary>
        /// <param name="GD"> The graphics device to be drawn to </param>
        /// <param name="Position"> A position for the terrain </param>
        public virtual void Draw(GraphicsDevice GD, Vector3 Position, bool UseID, PrimitiveEffect PEffect, Camera Cam)
        {
            PEffect.World = Cam.worldMatrix * Matrix.CreateTranslation(Position);
            PEffect.View = Cam.viewMatrix;
            PEffect.Projection = Cam.projectionMatrix;

            if (UseID)
            {
                PEffect.InternalEffect.CurrentTechnique = PEffect.InternalEffect.Techniques["Plain"];
                PEffect.DiffuseColor = ID.ToVector4(); //1, 1, 1 by default
            }
            else
            {
                PEffect.InternalEffect.CurrentTechnique = PEffect.InternalEffect.Techniques["Colored"];
                //PEffect.DiffuseColor = new Vector4(0.35f, 0.35f, 0.35f, 1); //1, 1, 1 by default
                PEffect.DiffuseColor = new Vector4(0.192f, 0.192f, 0.192f, 1); //1, 1, 1 by default
                PEffect.LightSpecularColor = new Vector3(0f);
                PEffect.SpecularColor = new Vector3(1f);
                PEffect.SpecularPower = 0.14f;
                PEffect.LightDirection = new Vector3(1, .71f, 1);
                PEffect.EmissiveColor = new Vector3(0.125f);
            }


            //Utils.PrimitivesEffect.AmbientLightColor = new Vector3(0, .25f, .75f);
            //Utils.PrimitivesEffect.EmissiveColor = new Vector3(0, 0, 1);

            //Utils.PrimitivesEffect.SpecularColor = new Vector3(0, .9f, 0.25f);

            GD.SetVertexBuffer(VertexBuffer);

            foreach (EffectPass pass in PEffect.InternalEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                GD.DrawPrimitives(PrimitiveType.TriangleList, 0, VertexBuffer.VertexCount / 3);
            }

            GD.SetVertexBuffer(null);
            GD.Indices = null;
        }

        /// <summary>
        /// Updates the terrain if necessary
        /// </summary>
        /// <param name="GT"> The current Gametime </param>
        /// <param name="MS"> The current Mousestate </param>
        /// <param name="KS"> The current Keyboardstate</param>
        public abstract void Update(GraphicsDevice GD, GameSceneVariables GSV, GameTime GT = null, MouseState? MS = null, KeyboardState? KS = null);

        /// <summary>
        /// Generates the terrain for the first time and sets both the index buffer and the vertex buffer
        /// </summary>
        public abstract void Generate(int TerrainDepth, int TerrainWidth, int Spacing, GraphicsDevice gd);
    }
}
