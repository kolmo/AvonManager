﻿
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace AvonManager.Common.Helpers
{

    /// <summary>
    /// Ergaenzt eine Listbox um Funktionalitaet
    /// </summary>
    public class ListBoxBehavoir :  Behavior<ListBox>
    {
        public object CheckedItem
        {
            get { return (object)GetValue(CheckedItemProperty); }
            set { SetValue(CheckedItemProperty, value); }
        }
        public bool ScrollAlwaysFirst { get; set; } = true;
        // Using a DependencyProperty as the backing store for CheckedItem.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CheckedItemProperty =
            DependencyProperty.Register("CheckedItem", typeof(object), typeof(ListBoxBehavoir), new PropertyMetadata(null, CheckedItemCallback));

        /// <summary>
        /// Aendert sich das CheckedItem, wird in der Listbox immer zum ersten Eintrag gescrollt.
        /// </summary>
        private static void CheckedItemCallback(DependencyObject dp, DependencyPropertyChangedEventArgs args)
        {
            if (dp is ListBoxBehavoir)
            {
                ListBoxBehavoir behavior = dp as ListBoxBehavoir;
                ListBox lb = behavior.AssociatedObject;
                if (lb.Items.Count > 0)
                {
                    lb.UpdateLayout();
                    lb.ScrollIntoView(behavior.ScrollAlwaysFirst ? lb.Items[0] : behavior.CheckedItem);                 
                }
            }
        }
    }
}