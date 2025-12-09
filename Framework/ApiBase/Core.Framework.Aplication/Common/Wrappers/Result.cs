namespace Core.Framework.Aplication.Common.Wrappers;

[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
public class ResultResponse
{
    public bool Success { get; set; }
    public List<Error> Errors { get; set; } = [];

    public static ResultResponse Ok()
        => new() { Success = true };

    public static ResultResponse Failure()
    => new() { Success = false };

    public static ResultResponse Failure(Error error)
        => new() { Success = false, Errors = [error] };

    public static ResultResponse Failure(IEnumerable<Error> errors)
        => new() { Success = false, Errors = errors.ToList() };

    public static implicit operator ResultResponse(Error error)
        => new() { Success = false, Errors = [error] };

    public static implicit operator ResultResponse(List<Error> errors)
        => new() { Success = false, Errors = errors };

    public ResultResponse AddError(Error error)
    {
        Errors ??= [];
        Errors.Add(error);
        Success = false;
        return this;
    }
}

public class ResultResponse<TData> : ResultResponse
{
    public TData Data { get; set; } = default!;

    public static ResultResponse<TData> Ok(TData data)
        => new() { Success = true, Data = data };
    public new static ResultResponse<TData> Failure()
        => new() { Success = false };

    public new static ResultResponse<TData> Failure(Error error)
        => new() { Success = false, Errors = [error] };

    public new static ResultResponse<TData> Failure(IEnumerable<Error> errors)
        => new() { Success = false, Errors = errors.ToList() };

    public static implicit operator ResultResponse<TData>(TData data)
        => new() { Success = true, Data = data };

    public static implicit operator ResultResponse<TData>(Error error)
        => new() { Success = false, Errors = [error] };

    public static implicit operator ResultResponse<TData>(List<Error> errors)
        => new() { Success = false, Errors = errors };
}
