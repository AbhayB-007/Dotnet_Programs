using TaskFlow.Domain.Common;
using TaskFlow.Domain.Enums;

namespace TaskFlow.Domain.Entities;

// Core aggregate root for the app. Deliberately framework-agnostic (no EF/Identity
// references here) so the domain stays independent of infrastructure concerns.
public class TaskItem : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public WorkStatus Status { get; set; } = WorkStatus.ToDo;
    public Priority Priority { get; set; } = Priority.Medium;
    public DateTime? DueDateUtc { get; set; }

    // Owner = the user who created / owns this task.
    public Guid OwnerId { get; set; }

    // Optional: task can be assigned to a different user by an Admin.
    public Guid? AssignedToUserId { get; set; }

    public bool IsDeleted { get; set; } = false; // soft delete

    public void MarkComplete() => Status = WorkStatus.Done;

    public void Reassign(Guid newAssigneeId) => AssignedToUserId = newAssigneeId;
}
