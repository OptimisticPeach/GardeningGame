using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GardeningGame;
using GardeningGame.Engine;
using GardeningGame.Engine.Scenes.Game.UI;
using GardeningGame.Engine.Scenes.Game.UI.Elements;

namespace GardeningGame.Engine.Scenes.Game.UI
{
    public static class ButtonStatus
    {
        public static Button ClickedButton { get; set; }
        public static Button PreviouslyClickedButton { get; set; }
        public static Button DraggedButton { get; set; }
        public static Button PreviouslyDraggedButton { get; set; }
    }
}
