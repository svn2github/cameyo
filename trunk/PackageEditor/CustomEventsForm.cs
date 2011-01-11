using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using VirtPackageAPI;

namespace PackageEditor
{
    public partial class CustomEventsForm : Form
    {
        private VirtPackage virtPackage;
        List<CustomEvent> onStartupUnvirtualized;
        List<CustomEvent> onStartupVirtualized;
        List<CustomEvent> onExitVirtualized;
        List<CustomEvent> onExitUnvirtualized;
        List<CustomEvent> curCustomEvents;

        public CustomEventsForm(VirtPackage virtPackage)
        {
            this.virtPackage = virtPackage;
            onStartupUnvirtualized = new List<CustomEvent>();
            onStartupVirtualized = new List<CustomEvent>();
            onExitVirtualized = new List<CustomEvent>();
            onExitUnvirtualized = new List<CustomEvent>();
            InitializeComponent();
        }

        private void CustomEventsForm_Load(object sender, EventArgs e)
        {
            curCustomEvents = onStartupUnvirtualized;
            comboBox.SelectedIndex = 0;
            RefreshDisplay();
        }

        private void RefreshDisplay()
        {
            listBox.Items.Clear();
            for (int i = 0; i < curCustomEvents.Count(); i++)
            {
                String cmdDisplay = curCustomEvents[i].cmd;
                if (curCustomEvents[i].args != "")
                    cmdDisplay += " [" + curCustomEvents[i].args + "]";
                listBox.Items.Add(cmdDisplay);
            }

            listBox_SelectedIndexChanged(null, null);
        }

        private void btnAddSave_Click(object sender, EventArgs e)
        {
            if (txtCmd.Text == "")
            {
                MessageBox.Show("Please enter a command to execute");
                return;
            }
            int selectedIndex = listBox.SelectedIndex;
            if (listBox.SelectedIndex == -1)
            {
                CustomEvent customEvent = new CustomEvent(txtCmd.Text, txtArgs.Text);
                curCustomEvents.Add(customEvent);
            }
            else
            {
                CustomEvent customEvent = curCustomEvents[listBox.SelectedIndex];
                customEvent.cmd = txtCmd.Text;
                customEvent.args = txtArgs.Text;
            }
            RefreshDisplay();
            listBox.SelectedIndex = selectedIndex;
        }

        private void listBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox.SelectedIndex == -1)
                return;
            CustomEvent customEvent = curCustomEvents[listBox.SelectedIndex];
            txtCmd.Text = customEvent.cmd;
            txtArgs.Text = customEvent.args;
        }

        private void btnErase_Click(object sender, EventArgs e)
        {
            if (listBox.SelectedIndex == -1)
                return;
            curCustomEvents.RemoveAt(listBox.SelectedIndex);
            listBox.Items.RemoveAt(listBox.SelectedIndex);
            listBox.SelectedIndex = -1;
            RefreshDisplay();
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            if (listBox.SelectedIndex == -1 || listBox.SelectedIndex == 0)
                return;
            curCustomEvents.Reverse(listBox.SelectedIndex - 1, 2);
            listBox.Items.Insert(listBox.SelectedIndex + 1, listBox.Items[listBox.SelectedIndex - 1]);
            listBox.Items.Remove(listBox.SelectedIndex - 1);
            int selectedIndex = listBox.SelectedIndex;
            RefreshDisplay();
            listBox.SelectedIndex = selectedIndex - 1;
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            if (listBox.SelectedIndex == -1 || listBox.SelectedIndex == listBox.Items.Count - 1)
                return;
            int selectedIndex = listBox.SelectedIndex;
            curCustomEvents.Reverse(listBox.SelectedIndex, 2);
            listBox.Items.Insert(listBox.SelectedIndex, listBox.Items[listBox.SelectedIndex + 1]);
            listBox.Items.Remove(selectedIndex + 2);
            RefreshDisplay();
            listBox.SelectedIndex = selectedIndex + 1;
        }
    }

    public class CustomEvent
    {
        public CustomEvent(String cmd, String args) { this.cmd = cmd; this.args = args; }
        public String cmd;
        public String args;
    }
}
