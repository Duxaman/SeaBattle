﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SeaBattle
{
    public partial class FieldForm : Form
    {
        private Game GameObject;
        public FieldForm(Game GameObject)
        {
            InitializeComponent();
            this.GameObject = GameObject;
        }
    }
}
