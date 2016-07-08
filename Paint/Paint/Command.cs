using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Paint
{
    public class Command
    {
      

        public enum DrawAction
        {
            NONE,
            ADD,
            REMOVE,
            CHANGE
        };
        public Canvas canvas;
        public UIElement uiElement;
        public DrawAction action;

        public Command(Canvas canvas, UIElement uiElement, DrawAction action)
        {
            this.canvas = canvas;
            this.uiElement = uiElement;
            this.action = action;
            //commands.Push(this);
        }

        public void Undo()
        {
            switch (action)
            {
                case DrawAction.ADD:
                    canvas.Children.Remove(uiElement);
                    break;
                case DrawAction.REMOVE:

                    break;
                case DrawAction.NONE:
                    break;
                case DrawAction.CHANGE:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    public class FullCommand
    {
        public static Stack<FullCommand> Commands = new Stack<FullCommand>();

        public static void UndoAction()
        {
            if (Commands.Count == 0)
                return;
            Commands.Pop().Undo();
        }

        public List<Command> commands = new List<Command>();

        public FullCommand(List<Command> list)
        {
            commands.AddRange(list);
            Commands.Push(this);
        }

        public void Undo()
        {
            commands.ForEach(command => command.Undo());
        }
    }
}
