using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GardeningGame.Engine.Scenes.Game.UI.Elements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GardeningGame.Engine.Scenes.Game.UI
{
    public abstract class GUI
    {
        public List<Element> Elements { get; set; }

        public GUI() => Elements = new List<Element>();

        public abstract void Initialize(ContentManager Content);

        public Button GetClicked()
        {
            foreach (var b in Elements)
            {
                if (b as Button != null)
                {
                    if ((b as Button).Clicked)
                    {
                        return b as Button;
                    }
                }
            }
            return null;
        }

        public void GenerateButtonStatus(MouseState ms)
        {
            ButtonStatus.PreviouslyClickedButton = ButtonStatus.ClickedButton;
            ButtonStatus.ClickedButton = GetClicked();
            ButtonStatus.PreviouslyDraggedButton = ButtonStatus.DraggedButton;
            if ((ButtonStatus.ClickedButton == null && ButtonStatus.PreviouslyClickedButton != null))
                ButtonStatus.DraggedButton = ButtonStatus.PreviouslyClickedButton;

            if(ButtonStatus.ClickedButton != ButtonStatus.DraggedButton && ButtonStatus.ClickedButton != null
                || (ms.LeftButton == ButtonState.Released && ButtonStatus.DraggedButton != null)
                || (ButtonStatus.DraggedButton == ButtonStatus.ClickedButton))
                ButtonStatus.DraggedButton = null;
        }

        public virtual void Draw(GameTime gt, SpriteBatch sb)
        {
            sb.Begin();
            float Layer = 0.0f;
            foreach(var Element in Elements)
            {
                Element.Draw(gt, sb, Layer);
                Layer += 0.1f;
            }
            sb.End();
        }

        public abstract void Update(GameTime gt, MouseState? ms, KeyboardState? ks);
    }
}
