using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Drawing;
using System.Reflection;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;
using Composite.C1Console.Workflow;

namespace Composite.Plugins.Elements.ElementProviders.PageElementProvider
{
    partial class AddNewPageWorkflow
    {
        #region Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCode]
        private void InitializeComponent()
        {
            this.CanModifyActivities = true;
            System.Workflow.Activities.CodeCondition codecondition1 = new System.Workflow.Activities.CodeCondition();
            System.Workflow.Activities.CodeCondition codecondition2 = new System.Workflow.Activities.CodeCondition();
            System.Workflow.Activities.CodeCondition codecondition3 = new System.Workflow.Activities.CodeCondition();
            System.Workflow.Activities.CodeCondition codecondition4 = new System.Workflow.Activities.CodeCondition();
            System.Workflow.Activities.CodeCondition codecondition5 = new System.Workflow.Activities.CodeCondition();
            System.Workflow.Activities.CodeCondition codecondition6 = new System.Workflow.Activities.CodeCondition();
            System.Workflow.Activities.CodeCondition codecondition7 = new System.Workflow.Activities.CodeCondition();
            System.Workflow.Activities.CodeCondition codecondition8 = new System.Workflow.Activities.CodeCondition();
            System.Workflow.Activities.CodeCondition codecondition9 = new System.Workflow.Activities.CodeCondition();
            System.Workflow.Activities.CodeCondition codecondition10 = new System.Workflow.Activities.CodeCondition();
            this.setStateActivity10 = new System.Workflow.Activities.SetStateActivity();
            this.RuleDontAllowPageAddCodeActivity = new System.Workflow.Activities.CodeActivity();
            this.setStateActivity4 = new System.Workflow.Activities.SetStateActivity();
            this.stepInitialize_codeActivity = new System.Workflow.Activities.CodeActivity();
            this.ifElseBranchActivity14 = new System.Workflow.Activities.IfElseBranchActivity();
            this.ifElseBranchActivity13 = new System.Workflow.Activities.IfElseBranchActivity();
            this.setStateActivity9 = new System.Workflow.Activities.SetStateActivity();
            this.MissingPageTypeCodeActivity = new System.Workflow.Activities.CodeActivity();
            this.ifElse_RulesDontAllowPageAdd = new System.Workflow.Activities.IfElseActivity();
            this.ifElseBranchActivity12 = new System.Workflow.Activities.IfElseBranchActivity();
            this.ifElseBranchActivity11 = new System.Workflow.Activities.IfElseBranchActivity();
            this.setStateActivity6 = new System.Workflow.Activities.SetStateActivity();
            this.MissingActiveLanguageCodeActivity = new System.Workflow.Activities.CodeActivity();
            this.ifElse_CheckPageTypesExists = new System.Workflow.Activities.IfElseActivity();
            this.setStateActivity7 = new System.Workflow.Activities.SetStateActivity();
            this.setStateActivity_GoToFinalize = new System.Workflow.Activities.SetStateActivity();
            this.PresetCalculatedFields = new System.Workflow.Activities.CodeActivity();
            this.ifElseBranchActivity6 = new System.Workflow.Activities.IfElseBranchActivity();
            this.ifElseBranchActivity5 = new System.Workflow.Activities.IfElseBranchActivity();
            this.ifElseBranchActivity10 = new System.Workflow.Activities.IfElseBranchActivity();
            this.ifElseBranchActivity9 = new System.Workflow.Activities.IfElseBranchActivity();
            this.setStateActivity20 = new System.Workflow.Activities.SetStateActivity();
            this.MissingTemplateAlertActivity = new System.Workflow.Activities.CodeActivity();
            this.ifElse_CheckActiveLanguagesExists = new System.Workflow.Activities.IfElseActivity();
            this.setStateActivity_GoToStep2 = new System.Workflow.Activities.SetStateActivity();
            this.ifElseActivity4_ValidateUrlTitle = new System.Workflow.Activities.IfElseActivity();
            this.ifElseBranchActivity8 = new System.Workflow.Activities.IfElseBranchActivity();
            this.ifElseBranchActivity7 = new System.Workflow.Activities.IfElseBranchActivity();
            this.ifElseBranchActivity4 = new System.Workflow.Activities.IfElseBranchActivity();
            this.ifElseBranchActivity3 = new System.Workflow.Activities.IfElseBranchActivity();
            this.setStateActivity11 = new System.Workflow.Activities.SetStateActivity();
            this.ifElse_CheckTemplatesExists = new System.Workflow.Activities.IfElseActivity();
            this.setStateActivity14 = new System.Workflow.Activities.SetStateActivity();
            this.ifElseActivity_CanSkipStep2 = new System.Workflow.Activities.IfElseActivity();
            this.setStateActivity13 = new System.Workflow.Activities.SetStateActivity();
            this.setStateActivity3 = new System.Workflow.Activities.SetStateActivity();
            this.setStateActivity_redoStep2 = new System.Workflow.Activities.SetStateActivity();
            this.setStateActivity12 = new System.Workflow.Activities.SetStateActivity();
            this.ifElseBranchActivity16 = new System.Workflow.Activities.IfElseBranchActivity();
            this.ifElseCheckPageTypesExist = new System.Workflow.Activities.IfElseBranchActivity();
            this.ifElseBranchActivity19 = new System.Workflow.Activities.IfElseBranchActivity();
            this.ifElseBranchActivity18 = new System.Workflow.Activities.IfElseBranchActivity();
            this.ifElseBranchActivity17 = new System.Workflow.Activities.IfElseBranchActivity();
            this.ifElseBranchActivity15 = new System.Workflow.Activities.IfElseBranchActivity();
            this.ifElseBranchActivity2 = new System.Workflow.Activities.IfElseBranchActivity();
            this.ifElseBranchActivity1 = new System.Workflow.Activities.IfElseBranchActivity();
            this.ifElseActivity1 = new System.Workflow.Activities.IfElseActivity();
            this.setStateActivity2 = new System.Workflow.Activities.SetStateActivity();
            this.cancelHandleExternalEventActivity2 = new Composite.C1Console.Workflow.Activities.CancelHandleExternalEventActivity();
            this.ifElse_FirstPageValid = new System.Workflow.Activities.IfElseActivity();
            this.finishHandleExternalEventActivity1 = new Composite.C1Console.Workflow.Activities.FinishHandleExternalEventActivity();
            this.ifFirstPageValid = new System.Workflow.Activities.IfElseActivity();
            this.nextHandleExternalEventActivity1 = new Composite.C1Console.Workflow.Activities.NextHandleExternalEventActivity();
            this.step1WizzardFormActivity = new Composite.C1Console.Workflow.Activities.WizardFormActivity();
            this.setStateActivity1 = new System.Workflow.Activities.SetStateActivity();
            this.stepFinalize_codeActivity = new System.Workflow.Activities.CodeActivity();
            this.closeCurrentViewActivity1 = new Composite.C1Console.Workflow.Activities.CloseCurrentViewActivity();
            this.setStateActivity5 = new System.Workflow.Activities.SetStateActivity();
            this.cancelHandleExternalEventActivity3 = new Composite.C1Console.Workflow.Activities.CancelHandleExternalEventActivity();
            this.ifElseActivity__ValidateUrlTitle = new System.Workflow.Activities.IfElseActivity();
            this.handleExternalEventActivity5 = new System.Workflow.Activities.HandleExternalEventActivity();
            this.setStateActivity8 = new System.Workflow.Activities.SetStateActivity();
            this.handleExternalEventActivity2 = new System.Workflow.Activities.HandleExternalEventActivity();
            this.step2WizzardFormActivity = new Composite.C1Console.Workflow.Activities.WizardFormActivity();
            this.PrepareStep2 = new System.Workflow.Activities.CodeActivity();
            this.setStateActivity19 = new System.Workflow.Activities.SetStateActivity();
            this.cancelHandleExternalEventActivity1 = new Composite.C1Console.Workflow.Activities.CancelHandleExternalEventActivity();
            this.initializeStateInitializationActivity = new System.Workflow.Activities.StateInitializationActivity();
            this.step1_eventDrivenActivity_Cancel = new System.Workflow.Activities.EventDrivenActivity();
            this.step1_eventDrivenActivity_Finish = new System.Workflow.Activities.EventDrivenActivity();
            this.step1_eventDrivenActivity_Next = new System.Workflow.Activities.EventDrivenActivity();
            this.step1StateInitializationActivity = new System.Workflow.Activities.StateInitializationActivity();
            this.finalizeStateInitializationActivity = new System.Workflow.Activities.StateInitializationActivity();
            this.step2_eventDrivenActivity_Cancel = new System.Workflow.Activities.EventDrivenActivity();
            this.step2_eventDrivenActivity_Finish = new System.Workflow.Activities.EventDrivenActivity();
            this.step2_eventDrivenActivity_Previous = new System.Workflow.Activities.EventDrivenActivity();
            this.step2StateInitializationActivity = new System.Workflow.Activities.StateInitializationActivity();
            this.eventDrivenActivity_GlobalCancel = new System.Workflow.Activities.EventDrivenActivity();
            this.initializeActivity = new System.Workflow.Activities.StateActivity();
            this.step1State = new System.Workflow.Activities.StateActivity();
            this.finalizeActivity = new System.Workflow.Activities.StateActivity();
            this.finishState = new System.Workflow.Activities.StateActivity();
            this.step2State = new System.Workflow.Activities.StateActivity();
            // 
            // setStateActivity10
            // 
            this.setStateActivity10.Name = "setStateActivity10";
            this.setStateActivity10.TargetStateName = "finishState";
            // 
            // RuleDontAllowPageAddCodeActivity
            // 
            this.RuleDontAllowPageAddCodeActivity.Name = "RuleDontAllowPageAddCodeActivity";
            this.RuleDontAllowPageAddCodeActivity.ExecuteCode += new System.EventHandler(this.RuleDontAllowPageAddCodeActivity_ExecuteCode);
            // 
            // setStateActivity4
            // 
            this.setStateActivity4.Name = "setStateActivity4";
            this.setStateActivity4.TargetStateName = "step1State";
            // 
            // stepInitialize_codeActivity
            // 
            this.stepInitialize_codeActivity.Name = "stepInitialize_codeActivity";
            this.stepInitialize_codeActivity.ExecuteCode += new System.EventHandler(this.stepInitialize_codeActivity_ExecuteCode);
            // 
            // ifElseBranchActivity14
            // 
            this.ifElseBranchActivity14.Activities.Add(this.RuleDontAllowPageAddCodeActivity);
            this.ifElseBranchActivity14.Activities.Add(this.setStateActivity10);
            this.ifElseBranchActivity14.Name = "ifElseBranchActivity14";
            // 
            // ifElseBranchActivity13
            // 
            this.ifElseBranchActivity13.Activities.Add(this.stepInitialize_codeActivity);
            this.ifElseBranchActivity13.Activities.Add(this.setStateActivity4);
            codecondition1.Condition += new System.EventHandler<System.Workflow.Activities.ConditionalEventArgs>(this.CheckRulesAllowPageAddExists);
            this.ifElseBranchActivity13.Condition = codecondition1;
            this.ifElseBranchActivity13.Name = "ifElseBranchActivity13";
            // 
            // setStateActivity9
            // 
            this.setStateActivity9.Name = "setStateActivity9";
            this.setStateActivity9.TargetStateName = "finishState";
            // 
            // MissingPageTypeCodeActivity
            // 
            this.MissingPageTypeCodeActivity.Name = "MissingPageTypeCodeActivity";
            this.MissingPageTypeCodeActivity.ExecuteCode += new System.EventHandler(this.MissingPageTypeCodeActivity_ExecuteCode);
            // 
            // ifElse_RulesDontAllowPageAdd
            // 
            this.ifElse_RulesDontAllowPageAdd.Activities.Add(this.ifElseBranchActivity13);
            this.ifElse_RulesDontAllowPageAdd.Activities.Add(this.ifElseBranchActivity14);
            this.ifElse_RulesDontAllowPageAdd.Name = "ifElse_RulesDontAllowPageAdd";
            // 
            // ifElseBranchActivity12
            // 
            this.ifElseBranchActivity12.Activities.Add(this.MissingPageTypeCodeActivity);
            this.ifElseBranchActivity12.Activities.Add(this.setStateActivity9);
            this.ifElseBranchActivity12.Name = "ifElseBranchActivity12";
            // 
            // ifElseBranchActivity11
            // 
            this.ifElseBranchActivity11.Activities.Add(this.ifElse_RulesDontAllowPageAdd);
            codecondition2.Condition += new System.EventHandler<System.Workflow.Activities.ConditionalEventArgs>(this.CheckPageTypeExists);
            this.ifElseBranchActivity11.Condition = codecondition2;
            this.ifElseBranchActivity11.Name = "ifElseBranchActivity11";
            // 
            // setStateActivity6
            // 
            this.setStateActivity6.Name = "setStateActivity6";
            this.setStateActivity6.TargetStateName = "finishState";
            // 
            // MissingActiveLanguageCodeActivity
            // 
            this.MissingActiveLanguageCodeActivity.Name = "MissingActiveLanguageCodeActivity";
            this.MissingActiveLanguageCodeActivity.ExecuteCode += new System.EventHandler(this.MissingActiveLanguageCodeActivity_ExecuteCode);
            // 
            // ifElse_CheckPageTypesExists
            // 
            this.ifElse_CheckPageTypesExists.Activities.Add(this.ifElseBranchActivity11);
            this.ifElse_CheckPageTypesExists.Activities.Add(this.ifElseBranchActivity12);
            this.ifElse_CheckPageTypesExists.Name = "ifElse_CheckPageTypesExists";
            // 
            // setStateActivity7
            // 
            this.setStateActivity7.Name = "setStateActivity7";
            this.setStateActivity7.TargetStateName = "step1State";
            // 
            // setStateActivity_GoToFinalize
            // 
            this.setStateActivity_GoToFinalize.Name = "setStateActivity_GoToFinalize";
            this.setStateActivity_GoToFinalize.TargetStateName = "finalizeActivity";
            // 
            // PresetCalculatedFields
            // 
            this.PresetCalculatedFields.Name = "PresetCalculatedFields";
            this.PresetCalculatedFields.ExecuteCode += new System.EventHandler(this.PresetCalculatedFields_ExecuteCode);
            // 
            // ifElseBranchActivity6
            // 
            this.ifElseBranchActivity6.Activities.Add(this.MissingActiveLanguageCodeActivity);
            this.ifElseBranchActivity6.Activities.Add(this.setStateActivity6);
            this.ifElseBranchActivity6.Name = "ifElseBranchActivity6";
            // 
            // ifElseBranchActivity5
            // 
            this.ifElseBranchActivity5.Activities.Add(this.ifElse_CheckPageTypesExists);
            codecondition3.Condition += new System.EventHandler<System.Workflow.Activities.ConditionalEventArgs>(this.CheckActiveLanguagesExists);
            this.ifElseBranchActivity5.Condition = codecondition3;
            this.ifElseBranchActivity5.Name = "ifElseBranchActivity5";
            // 
            // ifElseBranchActivity10
            // 
            this.ifElseBranchActivity10.Activities.Add(this.setStateActivity7);
            this.ifElseBranchActivity10.Name = "ifElseBranchActivity10";
            // 
            // ifElseBranchActivity9
            // 
            this.ifElseBranchActivity9.Activities.Add(this.PresetCalculatedFields);
            this.ifElseBranchActivity9.Activities.Add(this.setStateActivity_GoToFinalize);
            codecondition4.Condition += new System.EventHandler<System.Workflow.Activities.ConditionalEventArgs>(this.ValidateUrlTitle);
            this.ifElseBranchActivity9.Condition = codecondition4;
            this.ifElseBranchActivity9.Name = "ifElseBranchActivity9";
            // 
            // setStateActivity20
            // 
            this.setStateActivity20.Name = "setStateActivity20";
            this.setStateActivity20.TargetStateName = "finishState";
            // 
            // MissingTemplateAlertActivity
            // 
            this.MissingTemplateAlertActivity.Name = "MissingTemplateAlertActivity";
            this.MissingTemplateAlertActivity.ExecuteCode += new System.EventHandler(this.MissingTemplateAlertActivity_ExecuteCode);
            // 
            // ifElse_CheckActiveLanguagesExists
            // 
            this.ifElse_CheckActiveLanguagesExists.Activities.Add(this.ifElseBranchActivity5);
            this.ifElse_CheckActiveLanguagesExists.Activities.Add(this.ifElseBranchActivity6);
            this.ifElse_CheckActiveLanguagesExists.Name = "ifElse_CheckActiveLanguagesExists";
            // 
            // setStateActivity_GoToStep2
            // 
            this.setStateActivity_GoToStep2.Name = "setStateActivity_GoToStep2";
            this.setStateActivity_GoToStep2.TargetStateName = "step2State";
            // 
            // ifElseActivity4_ValidateUrlTitle
            // 
            this.ifElseActivity4_ValidateUrlTitle.Activities.Add(this.ifElseBranchActivity9);
            this.ifElseActivity4_ValidateUrlTitle.Activities.Add(this.ifElseBranchActivity10);
            this.ifElseActivity4_ValidateUrlTitle.Description = "ValidateUrlTitle";
            this.ifElseActivity4_ValidateUrlTitle.Name = "ifElseActivity4_ValidateUrlTitle";
            // 
            // ifElseBranchActivity8
            // 
            this.ifElseBranchActivity8.Activities.Add(this.MissingTemplateAlertActivity);
            this.ifElseBranchActivity8.Activities.Add(this.setStateActivity20);
            this.ifElseBranchActivity8.Name = "ifElseBranchActivity8";
            // 
            // ifElseBranchActivity7
            // 
            this.ifElseBranchActivity7.Activities.Add(this.ifElse_CheckActiveLanguagesExists);
            codecondition5.Condition += new System.EventHandler<System.Workflow.Activities.ConditionalEventArgs>(this.CheckTemplatesExists);
            this.ifElseBranchActivity7.Condition = codecondition5;
            this.ifElseBranchActivity7.Name = "ifElseBranchActivity7";
            // 
            // ifElseBranchActivity4
            // 
            this.ifElseBranchActivity4.Activities.Add(this.setStateActivity_GoToStep2);
            this.ifElseBranchActivity4.Name = "ifElseBranchActivity4";
            // 
            // ifElseBranchActivity3
            // 
            this.ifElseBranchActivity3.Activities.Add(this.ifElseActivity4_ValidateUrlTitle);
            codecondition6.Condition += new System.EventHandler<System.Workflow.Activities.ConditionalEventArgs>(this.CanSkipStep2);
            this.ifElseBranchActivity3.Condition = codecondition6;
            this.ifElseBranchActivity3.Name = "ifElseBranchActivity3";
            // 
            // setStateActivity11
            // 
            this.setStateActivity11.Name = "setStateActivity11";
            this.setStateActivity11.TargetStateName = "finishState";
            // 
            // ifElse_CheckTemplatesExists
            // 
            this.ifElse_CheckTemplatesExists.Activities.Add(this.ifElseBranchActivity7);
            this.ifElse_CheckTemplatesExists.Activities.Add(this.ifElseBranchActivity8);
            this.ifElse_CheckTemplatesExists.Name = "ifElse_CheckTemplatesExists";
            // 
            // setStateActivity14
            // 
            this.setStateActivity14.Name = "setStateActivity14";
            this.setStateActivity14.TargetStateName = "step1State";
            // 
            // ifElseActivity_CanSkipStep2
            // 
            this.ifElseActivity_CanSkipStep2.Activities.Add(this.ifElseBranchActivity3);
            this.ifElseActivity_CanSkipStep2.Activities.Add(this.ifElseBranchActivity4);
            this.ifElseActivity_CanSkipStep2.Name = "ifElseActivity_CanSkipStep2";
            // 
            // setStateActivity13
            // 
            this.setStateActivity13.Name = "setStateActivity13";
            this.setStateActivity13.TargetStateName = "step1State";
            // 
            // setStateActivity3
            // 
            this.setStateActivity3.Name = "setStateActivity3";
            this.setStateActivity3.TargetStateName = "step2State";
            // 
            // setStateActivity_redoStep2
            // 
            this.setStateActivity_redoStep2.Name = "setStateActivity_redoStep2";
            this.setStateActivity_redoStep2.TargetStateName = "step2State";
            // 
            // setStateActivity12
            // 
            this.setStateActivity12.Name = "setStateActivity12";
            this.setStateActivity12.TargetStateName = "finalizeActivity";
            // 
            // ifElseBranchActivity16
            // 
            this.ifElseBranchActivity16.Activities.Add(this.setStateActivity11);
            this.ifElseBranchActivity16.Name = "ifElseBranchActivity16";
            // 
            // ifElseCheckPageTypesExist
            // 
            this.ifElseCheckPageTypesExist.Activities.Add(this.ifElse_CheckTemplatesExists);
            codecondition7.Condition += new System.EventHandler<System.Workflow.Activities.ConditionalEventArgs>(this.CheckPageTypesExist);
            this.ifElseCheckPageTypesExist.Condition = codecondition7;
            this.ifElseCheckPageTypesExist.Name = "ifElseCheckPageTypesExist";
            // 
            // ifElseBranchActivity19
            // 
            this.ifElseBranchActivity19.Activities.Add(this.setStateActivity14);
            this.ifElseBranchActivity19.Name = "ifElseBranchActivity19";
            // 
            // ifElseBranchActivity18
            // 
            this.ifElseBranchActivity18.Activities.Add(this.ifElseActivity_CanSkipStep2);
            codecondition8.Condition += new System.EventHandler<System.Workflow.Activities.ConditionalEventArgs>(this.ValidateFirstPage);
            this.ifElseBranchActivity18.Condition = codecondition8;
            this.ifElseBranchActivity18.Name = "ifElseBranchActivity18";
            // 
            // ifElseBranchActivity17
            // 
            this.ifElseBranchActivity17.Activities.Add(this.setStateActivity13);
            this.ifElseBranchActivity17.Name = "ifElseBranchActivity17";
            // 
            // ifElseBranchActivity15
            // 
            this.ifElseBranchActivity15.Activities.Add(this.setStateActivity3);
            codecondition9.Condition += new System.EventHandler<System.Workflow.Activities.ConditionalEventArgs>(this.ValidateFirstPage);
            this.ifElseBranchActivity15.Condition = codecondition9;
            this.ifElseBranchActivity15.Name = "ifElseBranchActivity15";
            // 
            // ifElseBranchActivity2
            // 
            this.ifElseBranchActivity2.Activities.Add(this.setStateActivity_redoStep2);
            this.ifElseBranchActivity2.Name = "ifElseBranchActivity2";
            // 
            // ifElseBranchActivity1
            // 
            this.ifElseBranchActivity1.Activities.Add(this.setStateActivity12);
            codecondition10.Condition += new System.EventHandler<System.Workflow.Activities.ConditionalEventArgs>(this.ValidateUrlTitle);
            this.ifElseBranchActivity1.Condition = codecondition10;
            this.ifElseBranchActivity1.Name = "ifElseBranchActivity1";
            // 
            // ifElseActivity1
            // 
            this.ifElseActivity1.Activities.Add(this.ifElseCheckPageTypesExist);
            this.ifElseActivity1.Activities.Add(this.ifElseBranchActivity16);
            this.ifElseActivity1.Name = "ifElseActivity1";
            // 
            // setStateActivity2
            // 
            this.setStateActivity2.Name = "setStateActivity2";
            this.setStateActivity2.TargetStateName = "finishState";
            // 
            // cancelHandleExternalEventActivity2
            // 
            this.cancelHandleExternalEventActivity2.EventName = "Cancel";
            this.cancelHandleExternalEventActivity2.InterfaceType = typeof(Composite.C1Console.Workflow.IFormsWorkflowEventService);
            this.cancelHandleExternalEventActivity2.Name = "cancelHandleExternalEventActivity2";
            // 
            // ifElse_FirstPageValid
            // 
            this.ifElse_FirstPageValid.Activities.Add(this.ifElseBranchActivity18);
            this.ifElse_FirstPageValid.Activities.Add(this.ifElseBranchActivity19);
            this.ifElse_FirstPageValid.Name = "ifElse_FirstPageValid";
            // 
            // finishHandleExternalEventActivity1
            // 
            this.finishHandleExternalEventActivity1.EventName = "Finish";
            this.finishHandleExternalEventActivity1.InterfaceType = typeof(Composite.C1Console.Workflow.IFormsWorkflowEventService);
            this.finishHandleExternalEventActivity1.Name = "finishHandleExternalEventActivity1";
            // 
            // ifFirstPageValid
            // 
            this.ifFirstPageValid.Activities.Add(this.ifElseBranchActivity15);
            this.ifFirstPageValid.Activities.Add(this.ifElseBranchActivity17);
            this.ifFirstPageValid.Name = "ifFirstPageValid";
            // 
            // nextHandleExternalEventActivity1
            // 
            this.nextHandleExternalEventActivity1.EventName = "Next";
            this.nextHandleExternalEventActivity1.InterfaceType = typeof(Composite.C1Console.Workflow.IFormsWorkflowEventService);
            this.nextHandleExternalEventActivity1.Name = "nextHandleExternalEventActivity1";
            // 
            // step1WizzardFormActivity
            // 
            this.step1WizzardFormActivity.ContainerLabel = "Add new";
            this.step1WizzardFormActivity.FormDefinitionFileName = "\\Administrative\\AddNewPageStep1.xml";
            this.step1WizzardFormActivity.Name = "step1WizzardFormActivity";
            // 
            // setStateActivity1
            // 
            this.setStateActivity1.Name = "setStateActivity1";
            this.setStateActivity1.TargetStateName = "finishState";
            // 
            // stepFinalize_codeActivity
            // 
            this.stepFinalize_codeActivity.Name = "stepFinalize_codeActivity";
            this.stepFinalize_codeActivity.ExecuteCode += new System.EventHandler(this.stepFinalize_codeActivity_ExecuteCode);
            // 
            // closeCurrentViewActivity1
            // 
            this.closeCurrentViewActivity1.Name = "closeCurrentViewActivity1";
            // 
            // setStateActivity5
            // 
            this.setStateActivity5.Name = "setStateActivity5";
            this.setStateActivity5.TargetStateName = "finishState";
            // 
            // cancelHandleExternalEventActivity3
            // 
            this.cancelHandleExternalEventActivity3.EventName = "Cancel";
            this.cancelHandleExternalEventActivity3.InterfaceType = typeof(Composite.C1Console.Workflow.IFormsWorkflowEventService);
            this.cancelHandleExternalEventActivity3.Name = "cancelHandleExternalEventActivity3";
            // 
            // ifElseActivity__ValidateUrlTitle
            // 
            this.ifElseActivity__ValidateUrlTitle.Activities.Add(this.ifElseBranchActivity1);
            this.ifElseActivity__ValidateUrlTitle.Activities.Add(this.ifElseBranchActivity2);
            this.ifElseActivity__ValidateUrlTitle.Name = "ifElseActivity__ValidateUrlTitle";
            // 
            // handleExternalEventActivity5
            // 
            this.handleExternalEventActivity5.EventName = "Finish";
            this.handleExternalEventActivity5.InterfaceType = typeof(Composite.C1Console.Workflow.IFormsWorkflowEventService);
            this.handleExternalEventActivity5.Name = "handleExternalEventActivity5";
            // 
            // setStateActivity8
            // 
            this.setStateActivity8.Name = "setStateActivity8";
            this.setStateActivity8.TargetStateName = "step1State";
            // 
            // handleExternalEventActivity2
            // 
            this.handleExternalEventActivity2.EventName = "Previous";
            this.handleExternalEventActivity2.InterfaceType = typeof(Composite.C1Console.Workflow.IFormsWorkflowEventService);
            this.handleExternalEventActivity2.Name = "handleExternalEventActivity2";
            // 
            // step2WizzardFormActivity
            // 
            this.step2WizzardFormActivity.ContainerLabel = "Add new";
            this.step2WizzardFormActivity.FormDefinitionFileName = "\\Administrative\\AddNewPageStep2.xml";
            this.step2WizzardFormActivity.Name = "step2WizzardFormActivity";
            // 
            // PrepareStep2
            // 
            this.PrepareStep2.Name = "PrepareStep2";
            this.PrepareStep2.ExecuteCode += new System.EventHandler(this.PrepareStep2_ExecuteCode);
            // 
            // setStateActivity19
            // 
            this.setStateActivity19.Name = "setStateActivity19";
            this.setStateActivity19.TargetStateName = "finishState";
            // 
            // cancelHandleExternalEventActivity1
            // 
            this.cancelHandleExternalEventActivity1.EventName = "Cancel";
            this.cancelHandleExternalEventActivity1.InterfaceType = typeof(Composite.C1Console.Workflow.IFormsWorkflowEventService);
            this.cancelHandleExternalEventActivity1.Name = "cancelHandleExternalEventActivity1";
            // 
            // initializeStateInitializationActivity
            // 
            this.initializeStateInitializationActivity.Activities.Add(this.ifElseActivity1);
            this.initializeStateInitializationActivity.Name = "initializeStateInitializationActivity";
            // 
            // step1_eventDrivenActivity_Cancel
            // 
            this.step1_eventDrivenActivity_Cancel.Activities.Add(this.cancelHandleExternalEventActivity2);
            this.step1_eventDrivenActivity_Cancel.Activities.Add(this.setStateActivity2);
            this.step1_eventDrivenActivity_Cancel.Name = "step1_eventDrivenActivity_Cancel";
            // 
            // step1_eventDrivenActivity_Finish
            // 
            this.step1_eventDrivenActivity_Finish.Activities.Add(this.finishHandleExternalEventActivity1);
            this.step1_eventDrivenActivity_Finish.Activities.Add(this.ifElse_FirstPageValid);
            this.step1_eventDrivenActivity_Finish.Name = "step1_eventDrivenActivity_Finish";
            // 
            // step1_eventDrivenActivity_Next
            // 
            this.step1_eventDrivenActivity_Next.Activities.Add(this.nextHandleExternalEventActivity1);
            this.step1_eventDrivenActivity_Next.Activities.Add(this.ifFirstPageValid);
            this.step1_eventDrivenActivity_Next.Name = "step1_eventDrivenActivity_Next";
            // 
            // step1StateInitializationActivity
            // 
            this.step1StateInitializationActivity.Activities.Add(this.step1WizzardFormActivity);
            this.step1StateInitializationActivity.Name = "step1StateInitializationActivity";
            // 
            // finalizeStateInitializationActivity
            // 
            this.finalizeStateInitializationActivity.Activities.Add(this.closeCurrentViewActivity1);
            this.finalizeStateInitializationActivity.Activities.Add(this.stepFinalize_codeActivity);
            this.finalizeStateInitializationActivity.Activities.Add(this.setStateActivity1);
            this.finalizeStateInitializationActivity.Name = "finalizeStateInitializationActivity";
            // 
            // step2_eventDrivenActivity_Cancel
            // 
            this.step2_eventDrivenActivity_Cancel.Activities.Add(this.cancelHandleExternalEventActivity3);
            this.step2_eventDrivenActivity_Cancel.Activities.Add(this.setStateActivity5);
            this.step2_eventDrivenActivity_Cancel.Name = "step2_eventDrivenActivity_Cancel";
            // 
            // step2_eventDrivenActivity_Finish
            // 
            this.step2_eventDrivenActivity_Finish.Activities.Add(this.handleExternalEventActivity5);
            this.step2_eventDrivenActivity_Finish.Activities.Add(this.ifElseActivity__ValidateUrlTitle);
            this.step2_eventDrivenActivity_Finish.Name = "step2_eventDrivenActivity_Finish";
            // 
            // step2_eventDrivenActivity_Previous
            // 
            this.step2_eventDrivenActivity_Previous.Activities.Add(this.handleExternalEventActivity2);
            this.step2_eventDrivenActivity_Previous.Activities.Add(this.setStateActivity8);
            this.step2_eventDrivenActivity_Previous.Name = "step2_eventDrivenActivity_Previous";
            // 
            // step2StateInitializationActivity
            // 
            this.step2StateInitializationActivity.Activities.Add(this.PrepareStep2);
            this.step2StateInitializationActivity.Activities.Add(this.step2WizzardFormActivity);
            this.step2StateInitializationActivity.Name = "step2StateInitializationActivity";
            // 
            // eventDrivenActivity_GlobalCancel
            // 
            this.eventDrivenActivity_GlobalCancel.Activities.Add(this.cancelHandleExternalEventActivity1);
            this.eventDrivenActivity_GlobalCancel.Activities.Add(this.setStateActivity19);
            this.eventDrivenActivity_GlobalCancel.Name = "eventDrivenActivity_GlobalCancel";
            // 
            // initializeActivity
            // 
            this.initializeActivity.Activities.Add(this.initializeStateInitializationActivity);
            this.initializeActivity.Name = "initializeActivity";
            // 
            // step1State
            // 
            this.step1State.Activities.Add(this.step1StateInitializationActivity);
            this.step1State.Activities.Add(this.step1_eventDrivenActivity_Next);
            this.step1State.Activities.Add(this.step1_eventDrivenActivity_Finish);
            this.step1State.Activities.Add(this.step1_eventDrivenActivity_Cancel);
            this.step1State.Name = "step1State";
            // 
            // finalizeActivity
            // 
            this.finalizeActivity.Activities.Add(this.finalizeStateInitializationActivity);
            this.finalizeActivity.Name = "finalizeActivity";
            // 
            // finishState
            // 
            this.finishState.Name = "finishState";
            // 
            // step2State
            // 
            this.step2State.Activities.Add(this.step2StateInitializationActivity);
            this.step2State.Activities.Add(this.step2_eventDrivenActivity_Previous);
            this.step2State.Activities.Add(this.step2_eventDrivenActivity_Finish);
            this.step2State.Activities.Add(this.step2_eventDrivenActivity_Cancel);
            this.step2State.Name = "step2State";
            // 
            // AddNewPageWorkflow
            // 
            this.Activities.Add(this.step2State);
            this.Activities.Add(this.finishState);
            this.Activities.Add(this.finalizeActivity);
            this.Activities.Add(this.step1State);
            this.Activities.Add(this.initializeActivity);
            this.Activities.Add(this.eventDrivenActivity_GlobalCancel);
            this.CompletedStateName = "finishState";
            this.DynamicUpdateCondition = null;
            this.InitialStateName = "initializeActivity";
            this.Name = "AddNewPageWorkflow";
            this.CanModifyActivities = false;

        }

        #endregion

        private StateInitializationActivity step1StateInitializationActivity;

        private EventDrivenActivity step1_eventDrivenActivity_Next;

        private StateInitializationActivity step2StateInitializationActivity;

        private StateActivity step2State;

        private EventDrivenActivity step2_eventDrivenActivity_Previous;

        private HandleExternalEventActivity handleExternalEventActivity2;

        private SetStateActivity setStateActivity8;

        private SetStateActivity setStateActivity3;

        private EventDrivenActivity step2_eventDrivenActivity_Finish;

        private StateActivity finishState;

        private HandleExternalEventActivity handleExternalEventActivity5;

        private StateActivity finalizeActivity;

        private StateInitializationActivity finalizeStateInitializationActivity;

        private CodeActivity stepFinalize_codeActivity;

        private SetStateActivity setStateActivity4;

        private StateInitializationActivity initializeStateInitializationActivity;

        private StateActivity initializeActivity;

        private CodeActivity stepInitialize_codeActivity;

        private SetStateActivity setStateActivity12;

        private C1Console.Workflow.Activities.WizardFormActivity step1WizzardFormActivity;

        private C1Console.Workflow.Activities.WizardFormActivity step2WizzardFormActivity;

        private SetStateActivity setStateActivity1;

        private SetStateActivity setStateActivity19;

        private C1Console.Workflow.Activities.CancelHandleExternalEventActivity cancelHandleExternalEventActivity1;

        private EventDrivenActivity eventDrivenActivity_GlobalCancel;

        private IfElseBranchActivity ifElseBranchActivity8;

        private IfElseBranchActivity ifElseBranchActivity7;

        private IfElseActivity ifElse_CheckTemplatesExists;

        private CodeActivity MissingTemplateAlertActivity;

        private SetStateActivity setStateActivity20;

        private SetStateActivity setStateActivity_redoStep2;

        private IfElseBranchActivity ifElseBranchActivity2;

        private IfElseBranchActivity ifElseBranchActivity1;

        private IfElseActivity ifElseActivity__ValidateUrlTitle;

        private EventDrivenActivity step1_eventDrivenActivity_Finish;

        private IfElseBranchActivity ifElseBranchActivity4;

        private IfElseBranchActivity ifElseBranchActivity3;

        private IfElseActivity ifElseActivity_CanSkipStep2;

        private C1Console.Workflow.Activities.FinishHandleExternalEventActivity finishHandleExternalEventActivity1;

        private SetStateActivity setStateActivity_GoToStep2;

        private SetStateActivity setStateActivity_GoToFinalize;

        private CodeActivity PresetCalculatedFields;

        private SetStateActivity setStateActivity2;

        private C1Console.Workflow.Activities.CancelHandleExternalEventActivity cancelHandleExternalEventActivity2;

        private SetStateActivity setStateActivity5;

        private C1Console.Workflow.Activities.CancelHandleExternalEventActivity cancelHandleExternalEventActivity3;

        private EventDrivenActivity step1_eventDrivenActivity_Cancel;

        private EventDrivenActivity step2_eventDrivenActivity_Cancel;

        private C1Console.Workflow.Activities.CloseCurrentViewActivity closeCurrentViewActivity1;

        private IfElseBranchActivity ifElseBranchActivity6;

        private IfElseBranchActivity ifElseBranchActivity5;

        private IfElseActivity ifElse_CheckActiveLanguagesExists;

        private CodeActivity MissingActiveLanguageCodeActivity;

        private SetStateActivity setStateActivity6;

        private SetStateActivity setStateActivity7;

        private IfElseBranchActivity ifElseBranchActivity10;

        private IfElseBranchActivity ifElseBranchActivity9;

        private IfElseActivity ifElseActivity4_ValidateUrlTitle;

        private CodeActivity PrepareStep2;

        private SetStateActivity setStateActivity9;

        private CodeActivity MissingPageTypeCodeActivity;

        private IfElseBranchActivity ifElseBranchActivity12;

        private IfElseBranchActivity ifElseBranchActivity11;

        private IfElseActivity ifElse_CheckPageTypesExists;

        private IfElseBranchActivity ifElseBranchActivity14;

        private IfElseBranchActivity ifElseBranchActivity13;

        private IfElseActivity ifElse_RulesDontAllowPageAdd;

        private SetStateActivity setStateActivity10;

        private CodeActivity RuleDontAllowPageAddCodeActivity;

        private SetStateActivity setStateActivity11;

        private IfElseBranchActivity ifElseBranchActivity16;

        private IfElseBranchActivity ifElseCheckPageTypesExist;

        private IfElseActivity ifElseActivity1;

        private SetStateActivity setStateActivity13;

        private IfElseBranchActivity ifElseBranchActivity17;

        private IfElseBranchActivity ifElseBranchActivity15;

        private IfElseActivity ifFirstPageValid;

        private C1Console.Workflow.Activities.NextHandleExternalEventActivity nextHandleExternalEventActivity1;

        private SetStateActivity setStateActivity14;

        private IfElseBranchActivity ifElseBranchActivity19;

        private IfElseBranchActivity ifElseBranchActivity18;

        private IfElseActivity ifElse_FirstPageValid;

        private StateActivity step1State;



























































































































































































    }
}
