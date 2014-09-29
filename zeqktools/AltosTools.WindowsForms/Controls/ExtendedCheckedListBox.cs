using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace AltosTools.WindowsForms.Controls
{
    public partial class ExtendedCheckedListBox : CheckedListBox
    {
        private const string ALLITEMSSTRING = "(All)";

        private bool _autocheking = false;


        List<object> items;

        public ExtendedCheckedListBox()
        {
            InitializeComponent();
        }

        public ExtendedCheckedListBox(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        #region Properties

        public ICollection DataSource
        {
            get { return items; }
            set
            {
                this.ClearSelected();
                this.Items.Clear();
                items = new List<object>();
                if (value != null && value.Count > 0)
                {
                    this.Items.Add(ALLITEMSSTRING);

                    foreach (var item in value)
                    {
                        this.Items.Add(item);
                        items.Add(item);
                    }
                }
            }
        }

        public List<object> CheckedItemsValues
        {
            get
            {
                List<object> rv = new List<object>();
                if (!string.IsNullOrEmpty(this.ValueMember))
                {
                    foreach (var item in this.CheckedItems)
                    {
                        if (!item.Equals(ALLITEMSSTRING))
                        {
                            object value;
                            value = item.GetType().GetProperty(this.ValueMember).GetValue(item, null);
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
                if (!string.IsNullOrEmpty(this.ValueMember))
                {
                    foreach (var item in this.Items)
                    {
                        if (!item.Equals(ALLITEMSSTRING))
                        {
                            object value;
                            value = item.GetType().GetProperty(this.ValueMember).GetValue(item, null);
                            rv.Add(value);
                        }
                    }
                }
                return rv;
            }
        }

        #endregion

        #region public methods

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
                int index = this.Items.IndexOf(myObject);
                this.SetItemChecked(index, true);
            }

        }

        public void CheckAllItems()
        {
            for (int i = 0; i < this.Items.Count; i++)
            {
                if (this.Items[i].ToString() != ALLITEMSSTRING)
                {
                    this.SetItemChecked(i, true);
                }
            }
            if (!_autocheking)
                if (this.Items.Contains(ALLITEMSSTRING))
                {
                    int index = this.Items.IndexOf(ALLITEMSSTRING);
                    this.SetItemChecked(index, true);
                }
        }

        public void UncheckAllItems()
        {
            for (int i = 0; i < this.Items.Count; i++)
            {
                if (this.Items[i].ToString() != ALLITEMSSTRING)
                {
                    this.SetItemCheckState(i, CheckState.Unchecked);
                }
            }
            if (!_autocheking)
                if (this.Items.Contains(ALLITEMSSTRING))
                {
                    int index = this.Items.IndexOf(ALLITEMSSTRING);
                    this.SetItemChecked(index, false);
                }
        }
        #endregion

        private void ExtendedCheckedListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            var item = this.Items[e.Index];

            if (item.ToString() == ALLITEMSSTRING)
            {
                _autocheking = true;
                if (e.NewValue == CheckState.Checked)
                    CheckAllItems();
                else
                    UncheckAllItems();
                _autocheking = false;
            }
            //this.OnItemCheck(e);
        }

    }
}
