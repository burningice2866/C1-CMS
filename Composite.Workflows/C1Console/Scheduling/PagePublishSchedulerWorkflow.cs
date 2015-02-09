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
using System.ComponentModel;
using System.Globalization;
using System.Linq;

using Composite.Core;
using Composite.Data;
using Composite.Data.ProcessControlled;
using Composite.Data.ProcessControlled.ProcessControllers.GenericPublishProcessController;
using Composite.Data.Transactions;
using Composite.Data.Types;

namespace Composite.C1Console.Scheduling
{
    public sealed class PagePublishSchedulerWorkflow : BaseSchedulerWorkflow
    {
        private static readonly string LogTitle = typeof(PagePublishSchedulerWorkflow).Name;

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Guid PageId { get; set; }

        protected override void Execute()
        {
            using (new DataScope(DataScopeIdentifier.Administrated, CultureInfo.CreateSpecificCulture(LocaleName)))
            {
                IPage page;

                using (var transaction = TransactionsFacade.CreateNewScope())
                {
                    var pagePublishSchedule =
                        (from ps in DataFacade.GetData<IPublishSchedule>()
                         where ps.DataType == typeof(IPage).FullName &
                         ps.DataId == PageId.ToString() &&
                               ps.LocaleCultureName == LocaleName
                         select ps).Single();

                    DataFacade.Delete(pagePublishSchedule);

                    page = DataFacade.GetData<IPage>(p => p.Id == PageId).FirstOrDefault();


                    Verify.IsNotNull(page, "The page with the id {0} does not exist", PageId);

                    var transitions = ProcessControllerFacade.GetValidTransitions(page).Keys;
                    if (transitions.Contains(GenericPublishProcessController.Published))
                    {
                        page.PublicationStatus = GenericPublishProcessController.Published;

                        DataFacade.Update(page);

                        Log.LogVerbose(LogTitle, "Scheduled publishing of page with title '{0}' is complete", page.Title);
                    }
                    else
                    {
                        Log.LogWarning(LogTitle, "Scheduled publishing of page with title '{0}' could not be done because the page is not in a publisheble state", page.Title);
                    }

                    transaction.Complete();
                }

                PublishControlledHelper.ReloadPageElementInConsole(page);
            }
        }
    }
}
