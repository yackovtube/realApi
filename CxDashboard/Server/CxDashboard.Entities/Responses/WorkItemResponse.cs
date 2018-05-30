using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace CxDashboard.Entities.Responses
{
    public class WorkItemResponse
    {
        public int count { get; set; }
        public List<Value> value { get; set; }
    }

    public class Value
    {
        public int id { get; set; }
        public int rev { get; set; }
        public Fields fields { get; set; }
        public string url { get; set; }
    }

    public class Fields
    {
        [JsonProperty(PropertyName = "System.AreaPath")]
        public string SystemAreaPath { get; set; }
        [JsonProperty(PropertyName = "System.TeamProject")]
        public string SystemTeamProject { get; set; }
        [JsonProperty(PropertyName = "System.IterationPath")]
        public string SystemIterationPath { get; set; }
        [JsonProperty(PropertyName = "System.WorkItemType")]
        public string SystemWorkItemType { get; set; }
        [JsonProperty(PropertyName = "System.State")]
        public string SystemState { get; set; }
        [JsonProperty(PropertyName = "System.Reason")]
        public string SystemReason { get; set; }
        [JsonProperty(PropertyName = "System.AssignedTo")]
        public string SystemAssignedTo { get; set; }
        [JsonProperty(PropertyName = "System.CreatedDate")]
        public DateTime SystemCreatedDate { get; set; }
        [JsonProperty(PropertyName = "System.CreatedBy")]
        public string SystemCreatedBy { get; set; }
        [JsonProperty(PropertyName = "System.ChangedDate")]
        public DateTime SystemChangedDate { get; set; }
        [JsonProperty(PropertyName = "System.ChangedBy")]
        public string SystemChangedBy { get; set; }
        [JsonProperty(PropertyName = "System.Title")]
        public string SystemTitle { get; set; }
        [JsonProperty(PropertyName = "System.BoardColumn")]
        public string SystemBoardColumn { get; set; }
        [JsonProperty(PropertyName = "Microsoft.VSTS.Common.StateChangeDate")]
        public DateTime MicrosoftVSTSCommonStateChangeDate { get; set; }
        [JsonProperty(PropertyName = "Microsoft.VSTS.Common.ActivatedDate")]
        public DateTime MicrosoftVSTSCommonActivatedDate { get; set; }
        [JsonProperty(PropertyName = "Microsoft.VSTS.Common.ActivatedBy")]
        public string MicrosoftVSTSCommonActivatedBy { get; set; }
        [JsonProperty(PropertyName = "Microsoft.VSTS.Common.Priority")]
        public int MicrosoftVSTSCommonPriority { get; set; }
        [JsonProperty(PropertyName = "Microsoft.VSTS.Scheduling.Effort")]
        public double MicrosoftVSTSSchedulingEffort { get; set; }
        [JsonProperty(PropertyName = "Microsoft.VSTS.Common.Severity")]
        public string MicrosoftVSTSCommonSeverity { get; set; }
        [JsonProperty(PropertyName = "Cx.CustomerName")]
        public string CxCustomerName { get; set; }
        [JsonProperty(PropertyName = "Cx.Classification")]
        public string CxClassification { get; set; }
        [JsonProperty(PropertyName = "WEF_FC798E1CE03C49F6BF2A1AB7B0B08336_Kanban.Column")]
        public string WEF_FC798E1CE03C49F6BF2A1AB7B0B08336_KanbanColumn { get; set; }
        [JsonProperty(PropertyName = "Microsoft.VSTS.Common.BacklogPriority")]
        public float MicrosoftVSTSCommonBacklogPriority { get; set; }
        [JsonProperty(PropertyName = "Microsoft.VSTS.Scheduling.TargetDate")]
        public DateTime MicrosoftVSTSSchedulingTargetDate { get; set; }
        [JsonProperty(PropertyName = "WEF_67E2731F46664265A1431A15A1108E2A_Kanban.Column")]
        public string WEF_67E2731F46664265A1431A15A1108E2A_KanbanColumn { get; set; }
        [JsonProperty(PropertyName = "Checkmarx.Hotfix.Type")]
        public string CheckmarxHotfixType { get; set; }
        [JsonProperty(PropertyName = "Checkmarx.Hotfix.BaselineVersion")]
        public string CheckmarxHotfixBaselineVersion { get; set; }
        [JsonProperty(PropertyName = "Checkmarx.Hotfix.Files")]
        public string CheckmarxHotfixFiles { get; set; }
        [JsonProperty(PropertyName = "WEF_85457C0DEDC34C91B7DC1EADB008E127_Kanban.Column")]
        public string WEF_85457C0DEDC34C91B7DC1EADB008E127_KanbanColumn { get; set; }
        [JsonProperty(PropertyName = "Checkmarx.Hotfix.EngineBranchPath")]
        public string CheckmarxHotfixEngineBranchPath { get; set; }
        [JsonProperty(PropertyName = "Checkmarx.Hotfix.AppBranchPath")]
        public string CheckmarxHotfixAppBranchPath { get; set; }
        [JsonProperty(PropertyName = "Microsoft.VSTS.TCM.ReproSteps")]
        public string MicrosoftVSTSTCMReproSteps { get; set; }
        [JsonProperty(PropertyName = "Microsoft.VSTS.Build.IntegrationBuild")]
        public string MicrosoftVSTSBuildIntegrationBuild { get; set; }
        [JsonProperty(PropertyName = "WEF_DFA0D3DBB36A463D9052E5430F34BCA9_Kanban.Column")]
        public string WEF_DFA0D3DBB36A463D9052E5430F34BCA9_KanbanColumn { get; set; }
        [JsonProperty(PropertyName = "Microsoft.VSTS.CMMI.ProposedFix")]
        public string MicrosoftVSTSCMMIProposedFix { get; set; }
        [JsonProperty(PropertyName = "Cx.ProductComponent")]
        public string CxProductComponent { get; set; }
    }
}
