using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace ResourceLibrary
{
    public class WorkDataRow:Control
    {
          static WorkDataRow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WorkDataRow), 
                new FrameworkPropertyMetadata(typeof(WorkDataRow)));

        }
    }
}
