using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QdLayout
{
    public interface IEditArrangeWindow : IWindow
    {
         event Action<PlateArrangeGroup> Save;
    }
    public class EditArrangeRuntime()
    {
        public IEditArrangeWindow Control { get; set; }
        public Action<EditArrangeRuntime> Action;
        public PlateArrangeGroup group;
        public void ShowWindow(Action<EditArrangeRuntime> action)
        {
            this.Action = action;
            if (Control == null)
            {
                Control = new EditArrangeWindow();
                Control.Save += Control_Save;
                AppRuntime.UISystem.ShowWindow(Control, AppRuntime.App.ActiveWindow);
            }
        }

        private void Control_Save(PlateArrangeGroup group)
        {
            this.group = group;
            this.Action(this);
        }
    }
}
