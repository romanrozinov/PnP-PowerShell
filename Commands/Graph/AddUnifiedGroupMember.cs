﻿#if !ONPREMISES
using OfficeDevPnP.Core.Entities;
using OfficeDevPnP.Core.Framework.Graph;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base;
using SharePointPnP.PowerShell.Commands.Base.PipeBinds;
using System.Management.Automation;

namespace SharePointPnP.PowerShell.Commands.Graph
{
    [Cmdlet(VerbsCommon.Add, "PnPUnifiedGroupMember")]
    [CmdletHelp("Adds members to a particular Microsoft 365 Group (aka Unified Group)",
        Category = CmdletHelpCategory.Graph,
        OutputTypeLink = "https://docs.microsoft.com/graph/api/group-post-owners",
        SupportedPlatform = CmdletSupportedPlatform.Online)]
    [CmdletExample(
       Code = @"PS:> Add-PnPUnifiedGroupMember -Identity ""Project Team"" -Users ""john@contoso.onmicrosoft.com"",""jane@contoso.onmicrosoft.com""",
       Remarks = @"Adds the provided two users as additional members to the Microsoft 365 Group named ""Project Team""",
       SortOrder = 1)]
    [CmdletExample(
       Code = @"PS:> Add-PnPUnifiedGroupMember -Identity ""Project Team"" -Users ""john@contoso.onmicrosoft.com"",""jane@contoso.onmicrosoft.com"" -RemoveExisting",
       Remarks = @"Sets the provided two users as the only members of the Microsoft 365 Group named ""Project Team"" by removing any current existing members first",
       SortOrder = 2)]
    [CmdletMicrosoftGraphApiPermission(MicrosoftGraphApiPermission.User_ReadWrite_All)]
    [CmdletMicrosoftGraphApiPermission(MicrosoftGraphApiPermission.Group_ReadWrite_All)]
    public class AddUnifiedGroupMember : PnPGraphCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, HelpMessage = "The Identity of the Microsoft 365 Group to add members to")]
        public UnifiedGroupPipeBind Identity;

        [Parameter(Mandatory = true, HelpMessage = "The UPN(s) of the user(s) to add to the Microsoft 365 Group as a member")]
        public string[] Users;

        [Parameter(Mandatory = false, HelpMessage = "If provided, all existing members will be removed and only those provided through Users will become members")]
        public SwitchParameter RemoveExisting;

        protected override void ExecuteCmdlet()
        {
            UnifiedGroupEntity group = null;

            if (Identity != null)
            {
                group = Identity.GetGroup(AccessToken);
            }

            if (group != null)
            {
                UnifiedGroupsUtility.AddUnifiedGroupMembers(group.GroupId, Users, AccessToken, RemoveExisting.ToBool());
            }
        }
    }
}
#endif