﻿// Copyright © 2018 Alex Leendertsen

using System.Collections.ObjectModel;
using System.Net;
using Editor.ConfigProperties.Base;
using Editor.Interfaces;

namespace Editor.ConfigProperties
{
    internal class RemoteAddress : StringValueProperty
    {
        internal RemoteAddress(ReadOnlyCollection<IProperty> container)
            : base(container, "Remote Address:", "remoteAddress")
        {
        }

        public override bool TryValidate(IMessageBoxService messageBoxService)
        {
            if (!IPAddress.TryParse(Value, out _))
            {
                messageBoxService.ShowError("Remote address must be a valid IP address.");
                return false;
            }

            return base.TryValidate(messageBoxService);
        }
    }
}
