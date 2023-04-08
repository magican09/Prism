using PrismWorkApp.OpenWorkLib.Data;
using PrismWorkApp.OpenWorkLib.Data.Service;
using System;
using System.Collections;
using System.Collections.ObjectModel;

namespace PrismWorkApp.Core.Commands
{
    public class CopyCutPasteCommands<T>
    {
        public NotifyCommand<object> CopyCommand { get; private set; }
        public NotifyCommand<object> CutCommand { get; private set; }
        public NotifyCommand<object> PasteCommand { get; private set; }
        public ObservableCollection<CopiedCutedObject<T>> _objectsBuffer { get; set; } = new ObservableCollection<CopiedCutedObject<T>>();
        private UnDoReDoSystem _unDoReDoSystem;
        public CopyCutPasteCommands(Func<object, bool> canCopyExecuteMehtod, Func<object, bool> canCutExecuteMehtod, UnDoReDoSystem unDoReDoSystem)
        {
            _unDoReDoSystem = unDoReDoSystem;
            CopyCommand = new NotifyCommand<object>(OnCopy, canCopyExecuteMehtod);
            CopyCommand.Name = "Копировать";
            CutCommand = new NotifyCommand<object>(OnCut, canCutExecuteMehtod);
            CutCommand.Name = "Вырезать";
            PasteCommand = new NotifyCommand<object>(OnPaste, (ob) => _objectsBuffer.Count > 0);
            PasteCommand.Name = "Вставить";
        }

        private void OnCopy(object obj)
        {
            if (obj == null) return;
            bldWork selected_work = ((Tuple<object, object>)obj).Item2 as bldWork;
            _objectsBuffer.Clear();
            foreach (T tpl_obj in ((Tuple<object, object>)obj).Item1 as IList)
            {
                if (!(tpl_obj is IEntityObject)) break;
                _objectsBuffer.Add(new CopiedCutedObject<T>(selected_work, tpl_obj, CopyCutPaste.COPIED));
            }
        }
        private void OnCut(object obj)
        {
            if (obj == null) return;

            bldWork selected_work = ((Tuple<object, object>)obj).Item2 as bldWork;
            _objectsBuffer.Clear();
            foreach (T tpl_obj in ((Tuple<object, object>)obj).Item1 as IList)
            {
                if (!(tpl_obj is IEntityObject)) break;
                _objectsBuffer.Add(new CopiedCutedObject<T>(selected_work, tpl_obj, CopyCutPaste.CUTED));
            }
        }
        private void OnPaste(object obj)
        {
            if (obj == null) return;
            bldWork selected_work = ((Tuple<object, object>)obj).Item2 as bldWork;
            UnDoReDoSystem localUnDoReDo = new UnDoReDoSystem();
            _unDoReDoSystem.SetChildrenUnDoReDoSystem(localUnDoReDo);
            localUnDoReDo.Register(selected_work);
            foreach (CopiedCutedObject<T> copy_cut_obj in _objectsBuffer)
            {
                switch (copy_cut_obj.Element?.GetType()?.Name)
                {
                    case (nameof(bldMaterial)):
                        if (copy_cut_obj.ActionType == CopyCutPaste.COPIED)
                        {
                            bldMaterial new_obj = (copy_cut_obj.Element as bldMaterial).Clone() as bldMaterial;
                            selected_work.AddMaterial(new_obj);
                        }
                        if (copy_cut_obj.ActionType == CopyCutPaste.CUTED)
                        {
                            bldWork from_work = copy_cut_obj.FromObject as bldWork;
                            localUnDoReDo.Register(from_work);
                            (copy_cut_obj.FromObject as bldWork).RemoveMaterial(copy_cut_obj.Element as bldMaterial);
                            selected_work.AddMaterial(copy_cut_obj.Element as bldMaterial);
                        }
                        break;
                    case (nameof(bldLaboratoryReport)):
                        if (copy_cut_obj.ActionType == CopyCutPaste.COPIED)
                        {
                            bldLaboratoryReport new_obj = (copy_cut_obj.Element as bldLaboratoryReport).Clone() as bldLaboratoryReport;
                            selected_work.AddLaboratoryReport(new_obj);
                        }
                        if (copy_cut_obj.ActionType == CopyCutPaste.CUTED)
                        {
                            bldWork from_work = copy_cut_obj.FromObject as bldWork;
                            localUnDoReDo.Register(from_work);
                            (copy_cut_obj.FromObject as bldWork).RemoveLaboratoryReport(copy_cut_obj.Element as bldLaboratoryReport);
                            selected_work.AddLaboratoryReport(copy_cut_obj.Element as bldLaboratoryReport);
                        }
                        break;
                    case (nameof(bldExecutiveScheme)):
                        if (copy_cut_obj.ActionType == CopyCutPaste.COPIED)
                        {
                            bldExecutiveScheme new_obj = (copy_cut_obj.Element as bldExecutiveScheme).Clone() as bldExecutiveScheme;
                            selected_work.AddExecutiveScheme(new_obj);
                        }
                        if (copy_cut_obj.ActionType == CopyCutPaste.CUTED)
                        {
                            bldWork from_work = copy_cut_obj.FromObject as bldWork;
                            localUnDoReDo.Register(from_work);
                            (copy_cut_obj.FromObject as bldWork).RemoveExecutiveScheme(copy_cut_obj.Element as bldExecutiveScheme);
                            selected_work.AddExecutiveScheme(copy_cut_obj.Element as bldExecutiveScheme);
                        }
                        break;
                }

            }
            _objectsBuffer.Clear();
            _unDoReDoSystem.AddUnDoReDo(localUnDoReDo);
            _unDoReDoSystem.UnSetChildrenUnDoReDoSystem(localUnDoReDo);
        }
    }
}
