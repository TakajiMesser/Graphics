﻿using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpiceEngine.Entities;

namespace SauceEditor.Controls.Transform
{
    public class TransformChangedEventArgs : EventArgs
    {
        public string Name { get; private set; }
        public Vector3 Transform { get; private set; }

        public TransformChangedEventArgs(string name, Vector3 transform)
        {
            Name = name;
            Transform = transform;
        }
    }
}
