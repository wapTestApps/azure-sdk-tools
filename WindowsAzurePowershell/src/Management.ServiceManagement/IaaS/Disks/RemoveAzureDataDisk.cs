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

namespace Microsoft.WindowsAzure.Management.ServiceManagement.IaaS
{
    using System;
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.Linq;
    using System.Management.Automation;
    using Microsoft.WindowsAzure.ServiceManagement;
    using Model;

    [Cmdlet(VerbsCommon.Remove, "AzureDataDisk"), OutputType(typeof(IPersistentVM))]
    public class RemoveAzureDataDiskCommand : VirtualMachineConfigurationCmdletBase
    {
        [Parameter(Position = 0, Mandatory = true, HelpMessage = "Numerical value that defines the slot where the data drive is mounted in the virtual machine.")]
        [ValidateNotNullOrEmpty]
        public int LUN
        {
            get;
            set;
        }

        internal void ExecuteCommand()
        {
            var dataDisks = GetDataDisks();
            var dataDisk = dataDisks.SingleOrDefault(disk => disk.Lun == LUN);
            if (dataDisk == null)
            {
                ThrowTerminatingError(
                    new ErrorRecord(
                            new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "A data disk is not currently assigned to LUN #{0} in the configuration of this VM.", LUN)),
                            string.Empty,
                            ErrorCategory.InvalidData,
                            null));
            }

            dataDisks.Remove(dataDisk);

            WriteObject(VM, true);
        }

        protected override void ProcessRecord()
        {
            try
            {
                base.ProcessRecord();
                ExecuteCommand();
            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex, string.Empty, ErrorCategory.CloseError, null));
            }
        }

        protected Collection<DataVirtualHardDisk> GetDataDisks()
        {
            var role = VM.GetInstance();

            if (role.DataVirtualHardDisks == null)
            {
                role.DataVirtualHardDisks = new Collection<DataVirtualHardDisk>();
            }

            return role.DataVirtualHardDisks;
        }
    }
}