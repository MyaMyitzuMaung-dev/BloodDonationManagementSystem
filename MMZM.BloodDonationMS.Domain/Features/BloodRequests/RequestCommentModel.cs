namespace MMZM.BloodDonationMS.Domain.Features.BloodRequests;

public class AddCommentRequest
{
    public int RequestId { get; set; }
    public string CommentText { get; set; } = "";
}

public class AddCommentResponse
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; } = "";
}

public class GetCommentsResponse
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; } = "";
    public List<CommentDto> Data { get; set; } = new();
}

public class CommentDto
{
    public int CommentId { get; set; }
    public string UserName { get; set; } = "";
    public string CommentText { get; set; } = "";
    public DateTime? CreatedAt { get; set; }
}

public class DeleteCommentRequest
{
    public int CommentId { get; set; }
}

public class DeleteCommentResponse
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; } = "";
}
