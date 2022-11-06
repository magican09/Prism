using System;
using System.Collections.Generic;
using System.Text;

namespace PrismWorkApp.Core
{
    class ExamplesCode
    {
        /* Почулить делегат методв set свойства 
        var member_prop_info = body.Member as PropertyInfo;
        MethodInfo set_method = member_prop_info.GetSetMethod();
        Type setterGenericType = typeof(Action<,>);
        Type delegateType = setterGenericType.MakeGenericType(new Type[] { typeof(T), member_prop_info.PropertyType });
        var untypedDelegate = Delegate.CreateDelegate(typeof(Action<T>), null, set_method);*/
    }
}
