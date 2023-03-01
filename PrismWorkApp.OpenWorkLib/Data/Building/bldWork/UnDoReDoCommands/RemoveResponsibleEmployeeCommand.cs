using PrismWorkApp.OpenWorkLib.Data.Service;
using System;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public class RemoveResponsibleEmployeeCommand : IUnDoRedoCommand
    {
        private bldResponsibleEmployeesGroup _ResponsibleEmployees;
        private bldResponsibleEmployee _RemovedResp_Empl;
        private bldWork _CurrentWork;
        public string Name { get; set; } = "Удален документ из работы";

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            throw new NotImplementedException();
        }

        public void Execute(object parameter = null)
        {
            _CurrentWork.ResponsibleEmployees.Remove(_RemovedResp_Empl);

        }
        public void UnExecute()
        {
            _CurrentWork.ResponsibleEmployees.Add(_RemovedResp_Empl);
        }
        public RemoveResponsibleEmployeeCommand(bldWork work, bldResponsibleEmployeesGroup bldResponsibleEmployees, bldResponsibleEmployee resp_empl)
        {
            if (bldResponsibleEmployees == null)
            {

                bldResponsibleEmployeesGroup temp_resp_employees = new bldResponsibleEmployeesGroup();
                foreach (bldResponsibleEmployee employee in work.ResponsibleEmployees)
                    temp_resp_employees.Add(employee);
                work.ResponsibleEmployees = temp_resp_employees;
            }

            _CurrentWork = work;
            _RemovedResp_Empl = resp_empl;
            // _ResponsibleEmployees = bldResponsibleEmployees;


            _CurrentWork.ResponsibleEmployees.Remove(_RemovedResp_Empl);
            


        }
    }
}
