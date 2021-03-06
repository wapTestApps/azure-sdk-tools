// ----------------------------------------------------------------------------------
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

namespace Microsoft.WindowsAzure.Management.Tools.Vhd.Model.Persistence
{
    using System;
    using System.IO;

    public class DiskTypeFactory
    {
        public DiskType Create(uint diskType)
        {
            switch (diskType)
            {
                case 0: return DiskType.None;
                case 2: return DiskType.Fixed;
                case 3: return DiskType.Dynamic;
                case 4: return DiskType.Differencing;
                default:
                    throw new VhdParsingException(String.Format("Unsupported format: DiskType is {0}", diskType));
            }
        }
    }
}