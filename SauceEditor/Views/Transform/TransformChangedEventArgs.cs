﻿using OpenTK;
using System;

namespace SauceEditor.Views.Transform
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
