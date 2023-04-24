﻿namespace PrismWorkApp.Core.Commands
{
    public interface IApplicationCommands
    {
        NotifyCompositeCommand SaveAllCommand { get; }
        NotifyCompositeCommand SaveAllToDBCommand { get; }
        NotifyCompositeCommand UnDoCommand { get; }
        NotifyCompositeCommand ReDoCommand { get; }
        NotifyCompositeCommand CreateNewCommand { get; }
        NotifyCompositeCommand CreateBasedOnCommand { get; }
        NotifyCompositeCommand DeleteCommand { get; }
        NotifyCompositeCommand MoveToCommand { get; }
        NotifyCompositeCommand AddFromCommand { get; }
        NotifyCompositeCommand AddToCommand { get; }
        NotifyCompositeCommand AddCommand { get; }
        NotifyCompositeCommand AddNewAggregationDocumentCommand { get; }
        NotifyCompositeCommand AddNewMaterialCertificateCommand { get; }

        //NotifyCompositeCommand CreateWorkCommand { get; }
        //NotifyCompositeCommand CreateWorkFromTemplateCommand { get; }
        //NotifyCompositeCommand DeleteWorkCommand { get; }
        //NotifyCompositeCommand MoveWorkCommand { get; }
        //NotifyCompositeCommand AddWorkCommand { get; }

        NotifyCompositeCommand SaveExecutiveDocumentsCommand { get; }
        NotifyCompositeCommand LoadMaterialsFromAccessCommand { get; }
        NotifyCompositeCommand LoadAggregationDocumentsFromDBCommand { get; }
        NotifyCompositeCommand OpenAppSettingsDialogCommand { get; }

        NotifyCompositeCommand LoadUnitsOfMeasurementsCommand { get; }
        NotifyCompositeCommand SaveUnitsOfMeasurementsCommand { get; }
        
    }
    public class ApplicationCommands : IApplicationCommands
    {
        private NotifyCompositeCommand _saveAllCommand = new NotifyCompositeCommand();
        public NotifyCompositeCommand SaveAllCommand
        {
            get { return _saveAllCommand; }
        }
        private NotifyCompositeCommand _saveAllToDBCommand = new NotifyCompositeCommand();
        public NotifyCompositeCommand SaveAllToDBCommand
        {
            get { return _saveAllToDBCommand; }
        }


        private NotifyCompositeCommand _unDoCommand = new NotifyCompositeCommand(true);
        public NotifyCompositeCommand UnDoCommand
        {
            get { return _unDoCommand; }
        }
        private NotifyCompositeCommand _reDoCommand = new NotifyCompositeCommand(true);
        public NotifyCompositeCommand ReDoCommand
        {
            get { return _reDoCommand; }
        }
        //private NotifyCompositeCommand _createWorkCommand = new NotifyCompositeCommand(true);
        //public NotifyCompositeCommand CreateWorkCommand
        //{
        //    get { return _createWorkCommand; }
        //}
        //private NotifyCompositeCommand _createWorkFromTemplateCommand = new NotifyCompositeCommand(true);
        //public NotifyCompositeCommand CreateWorkFromTemplateCommand
        //{
        //    get { return _createWorkFromTemplateCommand; }
        //}
        //private NotifyCompositeCommand _deleteWorkCommand = new NotifyCompositeCommand(true);
        //public NotifyCompositeCommand DeleteWorkCommand
        //{
        //    get { return _deleteWorkCommand; }
        //}

        private NotifyCompositeCommand _moveWorkCommand = new NotifyCompositeCommand(true);
        public NotifyCompositeCommand MoveWorkCommand
        {
            get { return _moveWorkCommand; }
        }
        private NotifyCompositeCommand _addWorkCommand = new NotifyCompositeCommand(true);
        public NotifyCompositeCommand AddWorkCommand
        {
            get { return _addWorkCommand; }
        }

        private NotifyCompositeCommand _saveExecutiveDocumentsCommand = new NotifyCompositeCommand(true);
        public NotifyCompositeCommand SaveExecutiveDocumentsCommand
        {
            get { return _saveExecutiveDocumentsCommand; }
        }
        private NotifyCompositeCommand _loadMaterialsFromAccessCommand = new NotifyCompositeCommand(true);
        public NotifyCompositeCommand LoadMaterialsFromAccessCommand
        {
            get { return _loadMaterialsFromAccessCommand; }
        }
        private NotifyCompositeCommand _loadAggregationDocumentsFromDBCommand = new NotifyCompositeCommand(true);
        public NotifyCompositeCommand LoadAggregationDocumentsFromDBCommand
        {
            get { return _loadAggregationDocumentsFromDBCommand; }
        }

        private NotifyCompositeCommand _openAppSettingsDialogCommand = new NotifyCompositeCommand(true);
        public NotifyCompositeCommand OpenAppSettingsDialogCommand
        {
            get { return _openAppSettingsDialogCommand; }
        }

        private NotifyCompositeCommand _createNewCommand = new NotifyCompositeCommand(true);
        public NotifyCompositeCommand CreateNewCommand
        {
            get { return _createNewCommand; }
        }

        private NotifyCompositeCommand _createBasedOnCommand = new NotifyCompositeCommand(true);
        public NotifyCompositeCommand CreateBasedOnCommand
        {
            get { return _createBasedOnCommand; }
        }

        private NotifyCompositeCommand _deleteCommand = new NotifyCompositeCommand(true);
        public NotifyCompositeCommand DeleteCommand
        {
            get { return _deleteCommand; }
        }

        private NotifyCompositeCommand _moveToCommand = new NotifyCompositeCommand(true);
        public NotifyCompositeCommand MoveToCommand
        {
            get { return _moveToCommand; }
        }
        private NotifyCompositeCommand _addFromCommand = new NotifyCompositeCommand(true);
        public NotifyCompositeCommand AddFromCommand
        {
            get { return _addFromCommand; }
        }
        private NotifyCompositeCommand _addToCommand = new NotifyCompositeCommand(true);
        public NotifyCompositeCommand AddToCommand
        {
            get { return _addToCommand; }
        }
        
        private NotifyCompositeCommand _addCommand = new NotifyCompositeCommand(true);
        public NotifyCompositeCommand AddCommand
        {
            get { return _addCommand; }
        }
        private NotifyCompositeCommand _addNewAggregationDocumentCommand = new NotifyCompositeCommand(true);
        public NotifyCompositeCommand AddNewAggregationDocumentCommand
        {
            get { return _addNewAggregationDocumentCommand; }

        }
         private NotifyCompositeCommand _addNewMaterialCertificateCommand = new NotifyCompositeCommand(true);
        public NotifyCompositeCommand AddNewMaterialCertificateCommand
        {
            get { return _addNewMaterialCertificateCommand; }
        }
        
        private NotifyCompositeCommand _loadUnitsOfMeasurementsCommand = new NotifyCompositeCommand(true);
        public NotifyCompositeCommand LoadUnitsOfMeasurementsCommand
        {
            get { return _loadUnitsOfMeasurementsCommand; }
        }
        private NotifyCompositeCommand _saveUnitsOfMeasurementsCommand = new NotifyCompositeCommand(true);
        public NotifyCompositeCommand SaveUnitsOfMeasurementsCommand
        {
            get { return _saveUnitsOfMeasurementsCommand; }
        }
        
    }



}