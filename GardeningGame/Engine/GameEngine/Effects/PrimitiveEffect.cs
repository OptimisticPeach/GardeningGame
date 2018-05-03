using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace GardeningGame.Engine.GameEngine.Effects
{
    public class PrimitiveEffect
    {
        
        Effect internalEffect;
        public Effect InternalEffect
        {
            get
            {
                if(!SetWorldViewProjParams)
                {
                    internalEffect.Parameters["World"].SetValue(World);
                    internalEffect.Parameters["WorldViewProj"].SetValue(World * View * Projection);
                    internalEffect.Parameters["WorldInverseTranspose"].SetValueTranspose(Matrix.Invert(World));
                    internalEffect.Parameters["EyePosition"].SetValue(Matrix.Invert(View).Translation);
                    SetWorldViewProjParams = true;
                }
                return internalEffect;
            }
            set
            {
                internalEffect = value;
                SetWorldViewProjParams = false;
            }
        }

        private bool SetWorldViewProjParams = false;

        /*
        float3x3 WorldInverseTranspose; 
        Matrix WorldViewProj;    
        Matrix World;     
        Vector4 FogVector;  
        Vector3 FogColor;
        Vector3 EyePosition;
        Vector3 LightDirection;   
        Vector3 LightDiffuseColor;    
        Vector3 LightSpecularColor;   
        Vector4 DiffuseColor;        
        Vector3 EmissiveColor;     
        Vector3 SpecularColor;     
        float  SpecularPower;                  */

        #region fog
        public Vector3 FogColor { get => internalEffect.Parameters["FogColor"].GetValueVector3(); set => internalEffect.Parameters["FogColor"].SetValue(value); }

        public bool FogEnabled { get; set; }

        private float fogStart;
        private float fogEnd;

        public float FogEnd { get => fogEnd;
            set {
                fogEnd = value;
                SetFogVector();
            } }
        public float FogStart { get => fogStart;
            set {
                fogStart = value;
                SetFogVector();
            } }


        void SetFogVector()
        {
            if (!FogEnabled)
            {
                internalEffect.Parameters["FogVector"].SetValue(Vector4.Zero);
            }
            else if (fogStart == fogEnd)
            {
                // Degenerate case: force everything to 100% fogged if start and end are the same.
                internalEffect.Parameters["FogVector"].SetValue(new Vector4(0, 0, 0, 1));
            }
            else
            {
                // We want to transform vertex positions into view space, take the resulting
                // Z value, then scale and offset according to the fog start/end distances.
                // Because we only care about the Z component, the shader can do all this
                // with a single dot product, using only the Z row of the world+view matrix.

                float scale = 1f / (fogStart - fogEnd);

                Vector4 fogVector = new Vector4();

                var worldView = World * View;

                fogVector.X = worldView.M13 * scale;
                fogVector.Y = worldView.M23 * scale;
                fogVector.Z = worldView.M33 * scale;
                fogVector.W = (worldView.M43 + fogStart) * scale;

                internalEffect.Parameters["FogVector"].SetValue(fogVector);
            }
        }
        #endregion fog

        public Vector4 DiffuseColor
        {
            get => internalEffect.Parameters["DiffuseColor"].GetValueVector4();
            set => internalEffect.Parameters["DiffuseColor"].SetValue(value);
        }

        public Vector3 EmissiveColor
        {
            get => internalEffect.Parameters["EmissiveColor"].GetValueVector3();
            set => internalEffect.Parameters["EmissiveColor"].SetValue(value);
        }

        public Vector3 SpecularColor
        {
            get => internalEffect.Parameters["SpecularColor"].GetValueVector3();
            set => internalEffect.Parameters["SpecularColor"].SetValue(value);
        }

        public float SpecularPower
        {
            get => internalEffect.Parameters["SpecularPower"].GetValueSingle();
            set => internalEffect.Parameters["SpecularPower"].SetValue(value);
        }

        public Vector3 LightDirection
        {
            get => internalEffect.Parameters["LightDirection"].GetValueVector3();
            set => internalEffect.Parameters["LightDirection"].SetValue(value);
        }

        public Vector3 LightDiffuseColor
        {
            get => internalEffect.Parameters["LightDiffuseColor"].GetValueVector3();
            set => internalEffect.Parameters["LightDiffuseColor"].SetValue(value);
        }

        public Vector3 LightSpecularColor
        {
            get => internalEffect.Parameters["LightSpecularColor"].GetValueVector3();
            set => internalEffect.Parameters["LightSpecularColor"].SetValue(value);
        }

        public Matrix Projection { get; set; }
        public Matrix View { get; set; }
        public Matrix World { get; set; }

        public static explicit operator Effect(PrimitiveEffect source)
        {
            return source.InternalEffect;
        }
    }
}