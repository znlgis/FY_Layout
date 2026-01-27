
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QdLayout
{
    public interface IAppearanceEdit : IWindow
    {
        //public event Action<PlateBuildGroup, List<PlateBuildingMath>> Save;
        public event Action<PlateArrangeGroup, List<PlateBuildingMath>> Save;
    }
    public class AppearanceEditRuntime()
    {
        public IAppearanceEdit Control { get; set; }
        // public PlateArrangeGroup PlateArrangeGroup {  get; set; }
        private PlateBuildGroup PlateBuildGroup {  get; set; }
        //public static List<PlateBuildingMath> PlateBuildingMaths {  get; set; }
        public DocumentRuntime docRt { get; set; }
        public void ShowWindow(PlateBuildGroup plateBuildGroup, DocumentRuntime docRt)
        {
            if (Control == null)
            {
                this.docRt = docRt;
                PlateBuildGroup = plateBuildGroup;
                Control = new AppearanceEditWindow(PlateBuildGroup);
                Control.Save += Control_Save;
                AppRuntime.UISystem.ShowWindow(Control, AppRuntime.App.ActiveWindow);
            }
        }
        //private void Control_Save(PlateBuildGroup plateBuildGroup, List<PlateBuildingMath> plateBuildingMaths)
        //{
        //    PlateBuildGroup = plateBuildGroup;
        //    PlateBuildGroup.ResetPlateBuildings(plateBuildingMaths);
        //}
        private void Control_Save(PlateArrangeGroup plateBuildGroup, List<PlateBuildingMath> plateBuildingMaths)
        {
            plateBuildGroup= ArrangementRuntime.RefreshPlateBuildRooms(new List<PlateArrangeGroup> { plateBuildGroup }).First();
            PlateBuildGroup.ResetPlateBuildings(this.docRt, plateBuildGroup, plateBuildingMaths);
        }
    }
}
