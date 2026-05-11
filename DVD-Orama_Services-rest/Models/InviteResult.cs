namespace DVD_Orama_Services_rest.Models
{
    public enum InviteResult
    {
        Success,
        Updated,
        SameRoleConflict,
        NotAuthorized,
        TargetNotFound,
        CollectionNotFound,
        CannotAssignHigherRole
    }
}
