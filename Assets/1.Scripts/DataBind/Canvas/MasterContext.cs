//Data Bind
/*
    - 모든 컨텍스트들을 가지고 있는 마스터 컨텍스트
     -> 추후에 접근 할때 사용?
*/

namespace DataBind.UI 
{
    using Slash.Unity.DataBind.Core.Data;
    public class MasterContext : Context
    {
        private readonly Property<FlexGameDepartmentContext> _FlexGameDepartmentContext = new Property<FlexGameDepartmentContext>();

        public FlexGameDepartmentContext FlexGameDepartmentContext
        {
            get => _FlexGameDepartmentContext.Value;
            set => _FlexGameDepartmentContext.Value = value;
        }

        public MasterContext()
        {
            FlexGameDepartmentContext = new FlexGameDepartmentContext();
        }

        
    }

}
