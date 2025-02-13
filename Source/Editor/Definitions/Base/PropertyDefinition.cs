﻿// Copyright © 2018 Alex Leendertsen

using System.Collections.ObjectModel;
using Editor.Interfaces;

namespace Editor.Definitions.Base
{
    internal abstract class ElementDefinition : IElementDefinition
    {
        private readonly ObservableCollection<IProperty> mProperties;

        protected ElementDefinition()
        {
            mProperties = new ObservableCollection<IProperty>();
            Properties = new ReadOnlyObservableCollection<IProperty>(mProperties);
        }

        protected void AddProperty(IProperty property)
        {
            mProperties.Add(property);
        }

        protected void AddProperty(int index, IProperty property)
        {
            mProperties.Insert(index, property);
        }

        protected void RemoveProperty(IProperty property)
        {
            mProperties.Remove(property);
        }

        public abstract string Name { get; }

        public abstract string Icon { get; }

        public ReadOnlyObservableCollection<IProperty> Properties { get; }

        public abstract void Initialize();

        public IMessageBoxService MessageBoxService { get; set; }
    }
}
