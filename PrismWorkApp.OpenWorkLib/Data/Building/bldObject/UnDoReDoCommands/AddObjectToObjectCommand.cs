using PrismWorkApp.OpenWorkLib.Data.Service;
using PrismWorkApp.OpenWorkLib.Data.Service.UnDoReDo;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrismWorkApp.OpenWorkLib.Data
{
   
    public class AddObjectToObjectCommand : IUnDoRedoCommand
    {
        private bldObject _CurrentObject;
        private bldObject _AddedObject;

        private bldProject _AddObjectLastParentProject;
        private bldObject _AddObjectLastParentObject;
        
        public string Name { get; set; }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {

            throw new NotImplementedException();
        }

        public void Execute(object parameter = null)
        {
            _AddObjectLastParentProject = _AddedObject?.bldProject;
            _AddObjectLastParentObject = _AddedObject?.ParentObject;
            _AddedObject?.bldProject?.BuildingObjects.Remove(_AddedObject);
            _AddedObject?.ParentObject?.BuildingObjects?.Remove(_AddedObject);

            _CurrentObject.BuildingObjects.Add(_AddedObject);
        }

        public void UnExecute()
        {
             _AddedObject.bldProject= _AddObjectLastParentProject;
             _AddedObject.ParentObject= _AddObjectLastParentObject;
            _AddedObject?.bldProject?.BuildingObjects.Add(_AddedObject);
            _AddedObject?.ParentObject?.BuildingObjects?.Add(_AddedObject);

            _CurrentObject.BuildingObjects.Remove(_AddedObject);
        }
        public AddObjectToObjectCommand(bldObject bld_object, bldObject add_object)
        {
            _CurrentObject = bld_object;
            _AddedObject = add_object;

            _AddObjectLastParentProject = _AddedObject?.bldProject;
            _AddObjectLastParentObject = _AddedObject?.ParentObject;
            _AddedObject?.bldProject?.BuildingObjects.Remove(_AddedObject);
            _AddedObject?.ParentObject?.BuildingObjects?.Remove(_AddedObject);

            _CurrentObject.BuildingObjects.Add(_AddedObject);


        }
    }
}
