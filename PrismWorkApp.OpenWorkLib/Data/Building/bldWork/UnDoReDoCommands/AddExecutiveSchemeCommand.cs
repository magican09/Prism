using PrismWorkApp.OpenWorkLib.Data.Service;
using System;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public class AddExecutiveSchemeCommand : IUnDoRedoCommand
    {
        private bldWork _CurrentWork;
        private bldExecutiveScheme _AddedScheme;

        public string Name { get; set; } = "Добавлена схема к работе";

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            throw new NotImplementedException();
        }

        public void Execute(object parameter = null)
        {
            _CurrentWork.ExecutiveSchemes.Add(_AddedScheme);
            _CurrentWork.AOSRDocument.AttachedDocuments.Add(_AddedScheme);
        }

        public void UnExecute()
        {
            _CurrentWork.ExecutiveSchemes.Remove(_AddedScheme);
            _CurrentWork.AOSRDocument.AttachedDocuments.Remove(_AddedScheme);
        }
        public AddExecutiveSchemeCommand(bldWork work, bldExecutiveScheme scheme)
        {
            _CurrentWork = work;
            _AddedScheme = scheme;

            _CurrentWork.ExecutiveSchemes.Add(_AddedScheme);
            _CurrentWork.AOSRDocument.AttachedDocuments.Add(_AddedScheme);


        }
    }
}
