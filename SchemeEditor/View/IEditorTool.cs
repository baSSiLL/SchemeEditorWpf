using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SchemeEditor.View
{
    interface IEditorTool
    {
        void Init(Editor editor);
        bool MouseDown(Point position);
        bool MouseMove(Point position);
        bool MouseUp(Point position);
        bool KeyDown(Key key);
        void StopEditing();
        void Accept();
        Visibility Visibility { get; set; }
    }
}
