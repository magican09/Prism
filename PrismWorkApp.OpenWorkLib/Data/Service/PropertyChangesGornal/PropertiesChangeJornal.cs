using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace PrismWorkApp.OpenWorkLib.Data.Service
{
    public delegate void  PropertiesChangeJornalChangedEventHandler(object sender, PropertyStateRecord _propertyStateRecord);
    public delegate void ObjectStateChangeEventHandler(object sender, ObjectStateChangedEventArgs e);

    public class PropertiesChangeJornal : ObservableCollection<PropertyStateRecord>, IPropertiesChangeJornal
    {
        public event PropertiesChangeJornalChangedEventHandler JornalChangedNotify;
        public ObservableCollection<Guid> ContextIdHistory { get; set; } = new ObservableCollection<Guid>();

        public PropertiesChangeJornal()
        {

            CollectionChanged += PropertiesChangeJornal_CollectionChanged;
        }

        private void PropertiesChangeJornal_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (PropertyStateRecord stateRecord in e.NewItems)
                {
                    stateRecord.ParentJornal = this;
                    if (JornalChangedNotify != null)
                    {
                        /*  if (this.Where(pr=>pr.ContextId== stateRecord.ContextId).Count()==1)//Если изменеия в текущем контексе первые..
                         {
                             PropertyStateRecord st_record = new PropertyStateRecord(stateRecord.Value,stateRecord.Status, stateRecord.Name, ParentObject.CurrentContextId);

                             ParentObject.PropertiesChangeJornal.Add(st_record);
                         }    */
                        //   if (!ContextIdHistory.Contains(stateRecord.ContextId))
                        //     ContextIdHistory.Add(stateRecord.ContextId);


                        //31.10.22    JornalChangedNotify(this, stateRecord);

                    }
                    else
                        ;
                }
            }


        }
        public IJornalable ParentObject { get; set; }
        private bool IsContainsRecord(Guid currentContextId)
        {
            return this.Count > 0;
        }
        public void RegisterObject(IJornalable obj)
        {
            if (obj is INotifyCollectionChanged)
                (obj as INotifyCollectionChanged).CollectionChanged += OnCollectionObjectChanged;
            else if (obj is INotifyPropertyChanged)
                (obj as INotifyPropertyChanged).PropertyChanged += OnPropertyObjectChanged;
             var  properties_list = obj.GetType().GetProperties().Where(pr=>pr.GetIndexParameters().Length==0);

             foreach (PropertyInfo propertyInfo in properties_list)
             {
                 var prop_value = propertyInfo.GetValue(obj);

             }
      
        }
      /*  public void RegisterObject<T>(Expression<Func<T>> objectExpression) where T : IJornalable
        {
            var lamda_expression = (LambdaExpression)objectExpression;
            var member_expression = (MemberExpression)lamda_expression.Body;
            var view_model_expression = (ConstantExpression) member_expression.Expression;
            var view_model = view_model_expression.Value;
            PropertyInfo object_propI_info = member_expression.Member as PropertyInfo;
            if (view_model is INotifyCollectionChanged)
                (view_model as INotifyCollectionChanged).CollectionChanged += OnCollectionObjectChanged;
            else if (view_model is INotifyPropertyChanged)
                (view_model as INotifyPropertyChanged).PropertyChanged += OnObjectChanged;
        }
        */
       /* private  static void ParseExpression(Expression  expression)
        {
            if(expression.NodeType== ExpressionType.Lambda)
            {
                var lamda_expression = (LambdaExpression)expression;
                ParseExpression(lamda_expression.Body); 
            }
            if(expression.NodeType== ExpressionType.MemberAccess)
            {
                var member_expression = (MemberExpression)expression;
                ParseExpression(member_expression.Expression);
            }
            if(expression.NodeType== ExpressionType.Constant)
            {
                
            }
        }*/

        private void OnPropertyObjectChanged(object sender, PropertyChangedEventArgs e)
        {
            
        }

        private void OnCollectionObjectChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
           
        }
    }
    public class ObjectStateChangedEventArgs
    {
       public  string ObjectName { get; set; }
       public IJornalable Object { get; set; }
       public PropertyStateRecord PropertyStateRecord { get; set; }
        public ObjectStateChangedEventArgs()
        {

        }
        public ObjectStateChangedEventArgs(string name, IJornalable obj, PropertyStateRecord propertyStateRecord)
        {
            ObjectName = name;
            Object = obj;
            PropertyStateRecord = propertyStateRecord;
        }
    }

    

}
