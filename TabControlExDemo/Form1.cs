using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
namespace TabControlExDemo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            }

        private void button1_Click(object sender, EventArgs e)
        {
            tabControlEx2.IsCloseShow = true;
            tabControlEx2.TabPages.Add("1");
        }
    }
}