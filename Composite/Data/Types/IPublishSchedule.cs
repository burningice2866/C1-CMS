/*
 * The contents of this web application are subject to the Mozilla Public License Version 
 * 1.1 (the "License"); you may not use this web application except in compliance with 
 * the License. You may obtain a copy of the License at http://www.mozilla.org/MPL/.
 * 
 * Software distributed under the License is distributed on an "AS IS" basis, 
 * WITHOUT WARRANTY OF ANY KIND, either express or implied. See the License 
 * for the specific language governing rights and limitations under the License.
 * 
 * The Original Code is owned by and the Initial Developer of the Original Code is 
 * Composite A/S (Danish business reg.no. 21744409). All Rights Reserved
 * 
 * Section 11 of the License is EXPRESSLY amended to include a provision stating 
 * that any dispute, including but not limited to disputes related to the enforcement 
 * of the License, to which Composite A/S as owner of the Original Code, as Initial 
 * Developer or in any other role, becomes a part to shall be governed by Danish law 
 * and be initiated before the Copenhagen City Court ("K�benhavns Byret")            
 */

using System;

namespace Composite.Data.Types
{
    /// <summary>    
    /// </summary>
    /// <exclude />
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)] 
    [ImmutableTypeId("db542d7e-5cba-42e1-8a67-ad129941215c")]
    public interface IPublishSchedule : IUnpublishSchedule
	{
        /// <exclude />
        [StoreFieldType(PhysicalStoreFieldType.DateTime)]
        [ImmutableFieldId("d829eec8-76b0-46c7-9fdc-e93716451c91")]
        DateTime PublishDate { get; set; }
	}
}
