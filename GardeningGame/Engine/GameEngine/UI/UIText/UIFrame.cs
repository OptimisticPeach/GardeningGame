using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace GardeningGame.Engine.Scenes.Game.UI.UIText
{
    public class UIFrame
    {
        #region Class
        public static SpriteFont[] fonts { get; private set; } 

        public void DrawImage(SpriteBatch SpriteBatch, GraphicsDevice GraphicsDevice, GameTime GameTime, MouseState ms, bool EndSound)
        {

            foreach(var Element in Elements)
            {
                switch (Element.Usage) {
                    case UIElement.UIElementUsage.Image:
                        {
                            SpriteBatch.Draw(Element.Data.Image, Element.Data.Position, Element.Data.Colour);
                            break;
                        }
                    case UIElement.UIElementUsage.Link:
                        {
                            SpriteBatch.DrawString(Element.Data.BoundingRect.Contains(ms.Position) ? fonts[(int)Element.Data.OnHoverFont] : fonts[(int)Element.Data.TextFont], Element.Data.Text, Element.Data.Position, Element.Data.BoundingRect.Contains(ms.Position) ? Color.RoyalBlue : Color.SkyBlue);
                            break;
                        }
                    case UIElement.UIElementUsage.Video:
                        {
                            throw new NotImplementedException();
                        }
                    case UIElement.UIElementUsage.Text:
                        {
                            SpriteBatch.DrawString(fonts[(int)Element.Data.TextFont], Element.Data.Text, Element.Data.Position, Element.Data.Colour);
                            break;
                        }
                }
            }
        }

        public void Update(MouseState ms)
        {
            foreach (var Element in Elements)
            {
                switch (Element.Usage)
                {
                    case UIElement.UIElementUsage.Link:
                        {
                            if (Element.Data.BoundingRect.Contains(ms.Position) && ms.LeftButton == ButtonState.Released && Element.PreviouslyClicked)
                            {
                                System.Diagnostics.Process.Start(Element.Data.Link);
                            }
                            Element.PreviouslyClicked = Element.Data.BoundingRect.Contains(ms.Position) && ms.LeftButton == ButtonState.Pressed;
                            break;
                        }
                    case UIElement.UIElementUsage.Sound:
                        {
                            if (Element.Data.SoundEffect.State != SoundState.Playing)
                            {
                                Element.Data.SoundEffect.Play();
                            }
                            break;
                        }
                }
            }
        }

        public List<UIElement> Elements = new List<UIElement>();

        #endregion

        public static void Initialize(ContentManager Content)
        {
            Overlay = new UIFrame();

            List<SpriteFont> sfs = new List<SpriteFont>();
            foreach (var b in Enum.GetValues(typeof(UIElement.Font)))
            {
                sfs.Add(Content.Load<SpriteFont>("UI\\" + b.ToString()));
            }
            fonts = sfs.ToArray();

            UIElement Title = new UIElement()
            {
                Usage = UIElement.UIElementUsage.Text
            };

            {
                UIElement.DataUnion TitleData = new UIElement.DataUnion();
                TitleData.Position = new Vector2(400, 10);
                TitleData.TextFont = UIElement.Font.Arial_Large;
                TitleData.Colour = Color.BurlyWood;
                TitleData.Text = "Title";
                TitleData.BoundingRect = new Rectangle(TitleData.Position.ToPoint(), fonts[(int)TitleData.TextFont].MeasureString(TitleData.Text).ToPoint());

                Title.Data = TitleData;
            }

            UIElement Image = new UIElement()
            {
                Usage = UIElement.UIElementUsage.Image
            };

            {
                UIElement.DataUnion ImageData = new UIElement.DataUnion();
                ImageData.Position = new Vector2(20, 200);
                ImageData.Image = Content.Load<Texture2D>(@"UI\NotFound");
                ImageData.Colour = Color.White;
                ImageData.BoundingRect = new Rectangle(ImageData.Position.ToPoint(), ImageData.Image.Bounds.Size);

                Image.Data = ImageData;
            }

            UIElement Link = new UIElement()
            {
                Usage = UIElement.UIElementUsage.Link
            };
            {
                UIElement.DataUnion LinkData = new UIElement.DataUnion();
                LinkData.Position = new Vector2(400, 200);
                LinkData.TextFont = UIElement.Font.Arial_Italic;
                LinkData.OnHoverFont = UIElement.Font.Arial_Bold;
                LinkData.Text = "Website!";
                LinkData.Link = "http://www.google.com";
                LinkData.BoundingRect = new Rectangle(LinkData.Position.ToPoint(), fonts[(int)LinkData.TextFont].MeasureString(LinkData.Text).ToPoint());
                Link.Data = LinkData;
            }

            Overlay.Elements.Add(Link);
            Overlay.Elements.Add(Image);
            Overlay.Elements.Add(Title);
        }

        public static UIFrame Overlay { get; private set; }

    }
}
