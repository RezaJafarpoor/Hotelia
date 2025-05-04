namespace Hotelia.Shared.Common;

public record Result<T>
{
    public bool IsSuccessful { get; set; }
    public T Data { get; set; }
    public List<string> Errors { get; set; } = [];
    private Result(T Data, bool IsSuccessful)
    {
        this.IsSuccessful = IsSuccessful;
        this.Data = Data;
    }
    private Result(bool isSuccessful)
    {
        IsSuccessful = isSuccessful;
    }

    private Result(List<string> errors)
    {
        IsSuccessful = false;
        Errors = errors;
    }
   

    public static Result<T> Success(T data) => new(data, true);
    public static Result<T> Failed() => new(false);
    public static Result<T> Failed(List<string> errors) => new(errors);
    public static Result<T> Failed(string error) => new([error]);


    
}