﻿// ----------------------------------------------------------------------------------
//
// Copyright Microsoft Corporation
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// http://www.apache.org/licenses/LICENSE-2.0
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// ----------------------------------------------------------------------------------

namespace Microsoft.WindowsAzure.Management.Websites.Cmdlets
{
    using System;
    using System.Management.Automation;
    using Management.Utilities;
    using Properties;
    using Services;
    using Services.WebEntities;
    using Websites.Cmdlets.Common;

    /// <summary>
    /// Stops an azure website.
    /// </summary>
    [Cmdlet(VerbsLifecycle.Stop, "AzureWebsite"), OutputType(typeof(bool))]
    public class StopAzureWebsiteCommand : WebsiteContextBaseCmdlet
    {
        [Parameter(Mandatory = false)]
        public SwitchParameter PassThru { get; set; }

        /// <summary>
        /// Initializes a new instance of the StopAzureWebsiteCommand class.
        /// </summary>
        public StopAzureWebsiteCommand()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the StopAzureWebsiteCommand class.
        /// </summary>
        /// <param name="channel">
        /// Channel used for communication with Azure's service management APIs.
        /// </param>
        public StopAzureWebsiteCommand(IWebsitesServiceManagement channel)
        {
            Channel = channel;
        }

        public override void ExecuteCmdlet()
        {
            Site website = null;

            InvokeInOperationContext(() =>
            {
                website = RetryCall(s => Channel.GetSite(s, Name, null));
            });

            if (website == null)
            {
                throw new Exception(string.Format(Resources.InvalidWebsite, Name));
            }

            InvokeInOperationContext(() =>
            {
                Site websiteUpdate = new Site
                                        {
                                            Name = Name,
                                            HostNames = new [] { Name + General.AzureWebsiteHostNameSuffix },
                                            State = "Stopped"
                                        };

                RetryCall(s => Channel.UpdateSite(s, website.WebSpace, Name, websiteUpdate));
            });

            if (PassThru.IsPresent)
            {
                WriteObject(true);
            }
        }
    }
}
