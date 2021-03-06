﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AltosTools.WindowsForms.Controls
{
    public partial class CheckedListComboBox : PopupComboBox
    {
        private const string ALLITEMSSTRING = "(All)";        

        #region Fields
        private string _concatChar;
        private string _displayMember;
        private string _valueMember;
        private bool _autocheking = false;
        #endregion

        List<object> items;

        #region Properties

        public ICollection DataSource
        {
            get { return checkedListBox.Items; }
            set 
            {
                UncheckAllItems();
                checkedListBox.Items.Clear();
                if (value != null && value.Count > 0)
                {
                    int h = 22 * (value.Count + 1);

                    ResizeCheckedListBox(this.Width, h);
                    items = new List<object>();

                    checkedListBox.Items.Add(ALLITEMSSTRING);

                    foreach (var item in value)
                    {
                        checkedListBox.Items.Add(item);
                        items.Add(item);
                    }
                    int aux = checkedListBox.ItemHeight;
                }
            }
        }

        public string DisplayMember
        {
            get { return _displayMember; }
            set { _displayMember = value; }
        }
        
        public string ValueMember
        {
            get { return _valueMember; }
            set { _valueMember = value; }
        }

        public CheckedListBox.CheckedItemCollection CheckedItems
        {
            get { return checkedListBox.CheckedItems; }
        }

        public CheckedListBox.ObjectCollection Items
        {
            get 
            {
                CheckedListBox.ObjectCollection rv = checkedListBox.Items;
                rv.Remove(ALLITEMSSTRING);
                return rv; 
            }
        }

        public List<object> CheckedItemsValues
        {
            get 
            {         
                List<object> rv = new List<object>();
                if(!string.IsNullOrEmpty(_valueMember))
                {
                    foreach (var item in checkedListBox.CheckedItems)
                    {
                        if (!item.Equals(ALLITEMSSTRING))
                        {
                            object value;
                            value = item.GetType().GetProperty(_valueMember).GetValue(item, null);
                            rv.Add(value);
                        }
                    }
                }
                return rv; 
            }
        }

        public List<object> ItemsValues
        {
            get
            {
                List<object> rv = new List<object>();
                if (!string.IsNullOrEmpty(_valueMember))
                {
                    foreach (var item in checkedListBox.Items)
                    {
                        if (!item.Equals(ALLITEMSSTRING))
                        {
                            object value;
                            value = item.GetType().GetProperty(_valueMember).GetValue(item, null);
                            rv.Add(value);
                        }
                    }
                }
                return rv;
            }
        }
	

        /// <summary>
        /// A character to concatenate the list of checked items. Default value: comma
        /// </summary>
        public string ConcatChar
        {
            get { return _concatChar; }
            set { _concatChar = value; }
        }
        #endregion

        #region Contructors
        public CheckedListComboBox()
        {
            InitializeComponent();

            this.DropDownControl = checkedListBox;


            _concatChar = ", ";
        }

        public CheckedListComboBox(IContainer container)
        {
            container.Add(this);
            InitializeComponent();

            this.DropDownControl = checkedListBox;
            _concatChar = ", ";
        }
        #endregion

        #region Events
        public event ItemCheckEventHandler ItemCheck;

        protected virtual void OnItemCheck(ItemCheckEventArgs e)
        {
            // Make a temporary copy of the event to avoid possibility of
            // a race condition if the last subscriber unsubscribes
            // immediately after the null check and before the event is raised.
            ItemCheckEventHandler handler = ItemCheck;
            if (handler != null)
            {
                handler(this, e);
            }
        }


        #endregion


        #region Public methods

        public void Check(object value, string member)
        {
            if (items.Select(i => i.GetType().GetProperty(member)) != null)
            {
                object myObject = null;

                foreach (var item in items)
                {
                    object propValue = item.GetType().GetProperty(member).GetValue(item, null);
                    if (propValue.ToString() == value.ToString())
                    {
                        myObject = item;
                        break;
                    }
                }
                int index = checkedListBox.Items.IndexOf(myObject);
                checkedListBox.SetItemChecked(index, true);
            }
            
        }


        public void CheckAllItems()
        {
            for (int i = 0; i < checkedListBox.Items.Count; i++)
            {
                if (checkedListBox.Items[i].ToString() != ALLITEMSSTRING)
                {
                    checkedListBox.SetItemChecked(i, true);
                }
            }
            if(!_autocheking)
                if (checkedListBox.Items.Contains(ALLITEMSSTRING))
                {
                    int index = checkedListBox.Items.IndexOf(ALLITEMSSTRING);
                    checkedListBox.SetItemChecked(index, true);
                }
        }

        public void UncheckAllItems()
        {
            for (int i = 0; i < checkedListBox.Items.Count; i++)
            {
                if (checkedListBox.Items[i].ToString() != ALLITEMSSTRING)
                {
                    checkedListBox.SetItemCheckState(i, CheckState.Unchecked);
                }
            }
            if(!_autocheking)
                if (checkedListBox.Items.Contains(ALLITEMSSTRING))
                {
                    int index = checkedListBox.Items.IndexOf(ALLITEMSSTRING);
                    checkedListBox.SetItemChecked(index, false);
                }
        }

        #endregion

        private void ResizeCheckedListBox(int? w, int? h)
        {
            checkedListBox = new CheckedListBox();
            if (w.HasValue)
                checkedListBox.Width = w.Value;
            if (h.HasValue)
            {
                if (h.Value >= SystemInformation.PrimaryMonitorSize.Height)
                    h = SystemInformation.PrimaryMonitorSize.Height - SystemInformation.PrimaryMonitorSize.Height / 2;

                checkedListBox.Height = h.Value;
            }
            checkedListBox.AutoSize = true;
            checkedListBox.DrawMode = DrawMode.OwnerDrawVariable;
            checkedListBox.DisplayMember = _displayMember;
            
            
            checkedListBox.CheckOnClick = true;
            checkedListBox.ItemCheck += new ItemCheckEventHandler(this.checkedListBox_ItemCheck);            
            this.DropDownControl = checkedListBox;
        }

        private void CheckedListComboBox_Resize(object sender, EventArgs e)
        {
            ResizeCheckedListBox(this.Width, null);
        }


        private void checkedListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            var item = checkedListBox.Items[e.Index];

            if (item.ToString() == ALLITEMSSTRING)
            {
                _autocheking = true;
                if (e.NewValue == CheckState.Checked)
                    CheckAllItems();
                else
                    UncheckAllItems();
                _autocheking = false;
            }
            else
            {
                string checkedStr = item.GetType().GetProperty(checkedListBox.DisplayMember).GetValue(item, null).ToString();

                if (e.NewValue == CheckState.Checked)
                {
                    if (this.Text != "")
                        checkedStr = _concatChar + checkedStr;
                    this.Text += checkedStr;
                }

                if (e.NewValue == CheckState.Unchecked)
                {
                    int index = -1;

                    string textToRemove = _concatChar + checkedStr;
                    index = this.Text.IndexOf(textToRemove);

                    if (index < 0)
                    {
                        textToRemove = checkedStr + _concatChar;
                        index = this.Text.IndexOf(textToRemove);
                    }
                    if (index < 0)
                    {
                        textToRemove = checkedStr;
                        index = this.Text.IndexOf(checkedStr);
                    }
                    this.Text = this.Text.Remove(index, textToRemove.Length);
                }
            }
            this.OnItemCheck(e);
        }

        private void ListBox1_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            e.ItemHeight = 100;

            //// Cast the sender object back to ListBox type.
            //ListBox theListBox = (ListBox)sender;

            //// Get the string contained in each item.
            //string itemString = (string)theListBox.Items[e.Index];

            //// Split the string at the " . "  character.
            //string[] resultStrings = itemString.Split('.');

            //// If the string contains more than one period, increase the 
            //// height by ten pixels; otherwise, increase the height by 
            //// five pixels.
            //if (resultStrings.Length > 2)
            //{
            //    e.ItemHeight += 10;
            //}
            //else
            //{
            //    e.ItemHeight += 5;
            //}

        }

        //Text property non editable
        private void CheckedListComboBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }
    }
}
