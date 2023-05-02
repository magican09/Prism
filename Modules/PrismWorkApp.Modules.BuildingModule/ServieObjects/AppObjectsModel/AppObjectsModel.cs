using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using Prism.Events;
using Prism.Regions;
using Prism.Services.Dialogs;
using PrismWorkApp.Core;
using PrismWorkApp.Core.Commands;
using PrismWorkApp.Core.Dialogs;
using PrismWorkApp.Modules.BuildingModule.Core;
using PrismWorkApp.OpenWorkLib.Data;
using PrismWorkApp.OpenWorkLib.Data.Service;
using PrismWorkApp.Services.Repositories;
using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace PrismWorkApp.Modules.BuildingModule
{
    public class AppObjectsModel : BindableBase, IAppObjectsModel//,IActiveAware
    {
        private NameableObservableCollection<IEntityObject> _allModels = new NameableObservableCollection<IEntityObject>();

        public NameableObservableCollection<IEntityObject> AllModels
        {
            get { return _allModels; }
        }
        private bldDocumentsGroup _documentation = new bldDocumentsGroup();
        public bldDocumentsGroup Documentation
        {
            get { return _documentation; }
            set { SetProperty(ref _documentation, value); }
        }

        #region Commands 
        public NotifyCommand<object> CreateBasedOnCommand { get; set; }
        public NotifyCommand<object> CreateNewCommand { get; set; }
        public NotifyCommand<object> RemoveObjCommand { get; set; }
        public NotifyCommand<object> CloseObjCommand { get; set; }

        public NotifyCommand<object> OpenDocumentImageFileCommand { get; private set; }
        public NotifyCommand<object> SaveDocumentImageFileToDiskCommand { get; private set; }
        public NotifyCommand<object> LoadDocumentImageFileFromDiskCommand { get; private set; }

        public NotifyCommand SaveAllToDBCommand { get; set; }
       
        #endregion
        #region Contructors
        private IApplicationCommands _applicationCommands;
        public IBuildingUnitsRepository _buildingUnitsRepository { get; }
        private readonly IRegionManager _regionManager;
        private IDialogService _dialogService;

        IUnDoReDoSystem UnDoReDo;
        public AppObjectsModel(IRegionManager regionManager, IEventAggregator eventAggregator,
                                           IBuildingUnitsRepository buildingUnitsRepository, IDialogService dialogService, IApplicationCommands applicationCommands, IUnDoReDoSystem unDoReDoSystem)
        {
            _regionManager = regionManager;
            _buildingUnitsRepository = buildingUnitsRepository;
            _dialogService = dialogService;
            _applicationCommands = applicationCommands;
            UnDoReDo = unDoReDoSystem;
            UnDoReDo.Register(AllModels, true, false);
            Documentation.Name = "Документация";

            CreateBasedOnCommand = new NotifyCommand<object>(OnCreateBasedOn);
            CreateBasedOnCommand.Name = "Создать новый  на основании..";
            CreateNewCommand = new NotifyCommand<object>(OnCreateNew);
            CreateNewCommand.Name = "Создать новый...";
            RemoveObjCommand = new NotifyCommand<object>(OnRemove);
            RemoveObjCommand.Name = "Удалить";
            CloseObjCommand = new NotifyCommand<object>(OnCloseObj);
            CloseObjCommand.Name = "Закрыть";

            SaveAllToDBCommand = new NotifyCommand(OnSaveAllToDB);
            
            OpenDocumentImageFileCommand = new NotifyCommand<object>(OnOpenImageFile);
            OpenDocumentImageFileCommand.Name = "Открыть файл";
            SaveDocumentImageFileToDiskCommand  = new NotifyCommand<object>(OnSaveImageFileToDisk);
            SaveDocumentImageFileToDiskCommand.Name = "Сохранить файл";
            LoadDocumentImageFileFromDiskCommand = new NotifyCommand<object>(OnLoadImageFileFromDisk);
            LoadDocumentImageFileFromDiskCommand.Name = "Загрузить файл";
            
            //LoadAggregationDocumentFromDBCommand = new NotifyCommand<object>(OnLoadAggregationDocumentFromDB);
            //LoadAggregationDocumentFromDBCommand.Name = "Загрузить ведомость документов из БД";
            // CreateNewAggregationDocumentCommand = new NotifyCommand<object>(OnCreateNewAggregationDocument);
            //CreateNewAggregationDocumentCommand.Name = "Добавить новую ведомость документов";
            //RemoveAggregationDocumentCommand = new NotifyCommand<object>(OnRemoveAggregationDocument);
            //RemoveAggregationDocumentCommand.Name = "Удалить ведомость документов";

            //CreateNewMaterialCertificateCommand = new NotifyCommand<object>(OnCreateNewMaterialCertificate, (ob) => SelectedDocument is bldMaterialCertificate || SelectedDocument is bldMaterialCertificate).ObservesProperty(()=>SelectedDocument);
            //CreateBasedOnMaterialCertificateCommand = new NotifyCommand<object>(OnCreateBasedOnMaterialCertificate, (ob) => SelectedDocument is bldMaterialCertificate).ObservesProperty(() => SelectedDocument);
            //RemoveMaterialCertificateCommand = new NotifyCommand<object>(OnRemoveMaterialCertificate, (ob) => SelectedDocument is bldMaterialCertificate).ObservesProperty(() => SelectedDocument);

            _applicationCommands.SaveAllToDBCommand.RegisterCommand(SaveAllToDBCommand);
        }
        private void OnLoadImageFileFromDisk(object document)
        {
            bldDocument selected_document = document as bldDocument;
            string image_file_name = "";
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Файлы скана (PDF,JPG,PNG,GIF)|*.PDF;*.JPG;*.PNG;*.GIF";
            if (openFileDialog.ShowDialog() == true)
                image_file_name = openFileDialog.FileName;
            string file_extention = "*"+Path.GetExtension(image_file_name);
            if (image_file_name != "")
                try
                {
                    using (FileStream fs = File.OpenRead(image_file_name))
                    {
                        byte[] buffer = new byte[fs.Length];
                        fs.ReadAsync(buffer, 0, buffer.Length);
                        Picture new_image_file = new Picture();
                        new_image_file.FileData = new FileData();
                        new_image_file.FileType =_buildingUnitsRepository.TypesOfFileRepository.Select().Where(tf=>tf.Extention==file_extention).FirstOrDefault();
                        new_image_file.FileData.Data = buffer;
                        new_image_file.FileName = openFileDialog.SafeFileName;
                         selected_document.ImageFile = new_image_file;
                        selected_document.IsHaveImageFile = true;
                    //    _buildingUnitsRepository.PicturesReposytory.Add(new_image_file);
                    }
                }
                catch
                {
                    throw new Exception("Не удается считать файл!");
                }

        }
        private void OnSaveImageFileToDisk(object document)
        {
            bldDocument selected_document = document as bldDocument;
            if (selected_document.ImageFile.FileData == null)
                _buildingUnitsRepository.PicturesReposytory.Select().Where(fd => fd.Id == selected_document.ImageFile.Id).Include(fd => fd.FileData).FirstOrDefault();

            CommonOpenFileDialog dialog = new CommonOpenFileDialog();

            dialog.IsFolderPicker = true;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                string BD_FilesDir = dialog.FileName; ;
                if (!Directory.Exists(BD_FilesDir))
                    Directory.CreateDirectory(BD_FilesDir);

                string s = Path.Combine(BD_FilesDir, selected_document.ImageFile.FileName);
                using (System.IO.FileStream fs = new System.IO.FileStream(s, FileMode.OpenOrCreate))
                {
                    fs.Write(selected_document.ImageFile.FileData.Data);
                }
            }
        }
        private void OnOpenImageFile(object document)
        {
            bldDocument selected_document = document as bldDocument;
            string BD_FilesDir = Path.GetTempPath();

            if (!Directory.Exists(BD_FilesDir))
                Directory.CreateDirectory(BD_FilesDir);
           
            if (selected_document.ImageFile.FileData == null)
              _buildingUnitsRepository.PicturesReposytory.Select().Where(fd => fd.Id == selected_document.ImageFile.Id).Include(fd => fd.FileData).FirstOrDefault();
            string s = Path.Combine(BD_FilesDir, selected_document.ImageFile.FileName );

            using (System.IO.FileStream fs = new System.IO.FileStream(s, FileMode.OpenOrCreate))
            {
                fs.Write(selected_document.ImageFile.FileData.Data);
            }
            ProcessStartInfo info = new ProcessStartInfo(s);
            info.UseShellExecute = true;
            using (var proc = Process.Start(info)) { }
        }

        private void OnRemove(object obj)
        {
            if (obj is IList list)
            {
                IEntityObject selected_object = list[0] as IEntityObject;
                INameableObservableCollection parent_collection = list[1] as INameableObservableCollection;
                if (selected_object != null && parent_collection != null)
                {
                    CoreFunctions.ConfirmChangesDialog(_dialogService, "хотите удалить объект?!\n Объект будет удален безвозвратно!!!",
                   (result) =>
                   {
                       if (result.Result == ButtonResult.Yes || result.Result == ButtonResult.No)
                       {
                           CoreFunctions.ConfirmChangesDialog(_dialogService, "хотите удалить объект?!",
                             (result_2) =>
                             {
                                 if (result_2.Result == ButtonResult.Yes)
                                 {
                                     parent_collection.Remove(selected_object);

                                     if (result.Result == ButtonResult.Yes)
                                     {
                                         _buildingUnitsRepository.Remove(selected_object);
                                     }
                                     var dialog_par = new DialogParameters();
                                     dialog_par.Add("message", $"{selected_object.Name} успешно удален!");
                                     _dialogService.ShowDialog(nameof(MessageDialog), dialog_par, (result) => { });
                                 }

                             }, "Всё равно удалить", "Не удалять");
                       }

                   }, "Удалить", "Не удалять");

                    selected_object = null;
                    _buildingUnitsRepository.Complete();
                }
            }

        }
        private void OnCloseObj(object obj)
        {
            if (obj is IList list)
            {
                IEntityObject selected_object = list[0] as IEntityObject;
                INameableObservableCollection parent_collection = list[1] as INameableObservableCollection;
                if (selected_object != null && parent_collection != null)
                {
                    int ch_namber = UnDoReDo.GetChangesNamber(selected_object); //Отнимает 1, так как в изменениях 
                    if (ch_namber != 0)
                    {
                        CoreFunctions.ConfirmChangesDialog(_dialogService, "сохранить изменения",
                            (result) =>
                            {
                                if (result.Result == ButtonResult.Yes || result.Result == ButtonResult.No)
                                {
                                    parent_collection.Remove(selected_object);

                                    if (result.Result == ButtonResult.Yes)
                                    {
                                        UnDoReDo.Save(selected_object);
                                        UnDoReDo.UnRegister(selected_object);
                                        _buildingUnitsRepository.Complete();
                                        var dialog_par = new DialogParameters();
                                        dialog_par.Add("message", $"{ch_namber.ToString()} изменения(й) сохранено!");
                                        _dialogService.ShowDialog(nameof(MessageDialog), dialog_par, (result) => { });
                                    }
                                    if (result.Result == ButtonResult.No)
                                    {
                                        UnDoReDo.UnDoAll(selected_object);
                                        UnDoReDo.UnRegister(selected_object);
                                        parent_collection.Remove(selected_object);
                                        var dialog_par = new DialogParameters();
                                        dialog_par.Add("message", $"{ch_namber.ToString()} изменения(й) сброшено!");
                                        _dialogService.ShowDialog(nameof(MessageDialog), dialog_par, (result) => { });
                                    }
                                }
                                if (result.Result == ButtonResult.Cancel)
                                {
                                }
                            });
                    }
                    else
                    {
                        parent_collection.Remove(selected_object);
                    }

                    if (_buildingUnitsRepository.Contains(selected_object))
                        _buildingUnitsRepository.SetAsDetached(selected_object);

                    parent_collection = null;

                }
            }
        }
        private void OnCreateBasedOn(object obj)
        {
            if (obj is IList list)
            {
                IEntityObject selected_object = list[0] as IEntityObject;
                IEntityObject selected_object_parent = list[1] as IEntityObject;
                Type type_for_created_obj =null;
                if(((IList)obj)[2]!=null)
                    type_for_created_obj = ((IList)obj)[2] as Type;

                if (selected_object_parent != null)
                {
                    IEntityObject new_object = (IEntityObject)selected_object.Clone();
                    if (!selected_object_parent.IsDbBranch)
                    {
                        _buildingUnitsRepository.Add(new_object);
                    }
                    if (new_object is IJornalable jornable_new_object) jornable_new_object.IsDbBranch = true;
                    if (selected_object_parent is IList new_object_list_parent)
                        new_object_list_parent.Add(new_object);
                }
            }
        }
        private void OnCreateNew(object obj)
        {
            if (obj is IList list)
            {
                IEntityObject selected_object = list[0] as IEntityObject;
                IEntityObject selected_object_parent = list[1] as IEntityObject;
                Type type_for_created_obj= null;
                if (((IList)obj)[2] != null)
                    type_for_created_obj = ((IList)obj)[2] as Type;
               
                if ( type_for_created_obj != null)
                {
                    IEntityObject new_obj = (IEntityObject)Activator.CreateInstance(type_for_created_obj);
                    INameableObservableCollection coll_for_add = null;
                    if (selected_object is INameableObservableCollection nameable_selected_coll)
                        coll_for_add = nameable_selected_coll;
                    else 
                        if (selected_object_parent is INameableObservableCollection nameable_parent_selected_coll)
                                coll_for_add = nameable_parent_selected_coll;
                    if(coll_for_add!=null)
                    {
                        if (!coll_for_add.Owner.IsDbBranch)
                        {
                            _buildingUnitsRepository.Add(new_obj);
                            new_obj.IsDbBranch = true;
                        }
                        coll_for_add.Add(new_obj);
                    }
                  

                }
            }
        }
 
        

        #endregion
        #region Model data 
        #region Documentation
        /// <summary>
        /// Коллекция для хранения документации
        /// </summary>
        #endregion

        #endregion

        #region Save 
        public void OnSaveAllToDB()
        {
            CoreFunctions.ConfirmActionDialog("Сохранить все изменения в БД?", "",
                "Сохранить", "Отмена", (result) =>
                  {
                      if (result.Result == ButtonResult.Yes)
                      {
                          var all_changed_objects = UnDoReDo._RegistedModels.Keys.Where(ob => ob.IsDbBranch && ob.State != OpenWorkLib.Data.Service.EntityState.Unchanged).ToList();
                          UnDoReDo.SaveAll();
                          _buildingUnitsRepository.Complete(UnDoReDo);
                          var res_massage = result.Parameters.GetValue<string>("confirm_dialog_param");
                          var p = new DialogParameters();
                          p.Add("message", $"Готово!");
                          _dialogService.ShowDialog(typeof(MessageDialog).Name, p, (r) => { });
                      }
                  }, _dialogService);

        }

        #endregion
    }
}
