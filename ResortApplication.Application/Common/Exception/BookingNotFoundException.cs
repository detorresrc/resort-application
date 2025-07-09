namespace ResortApplication.Application.Common.Exception;

public class BookingNotFoundException : System.Exception
{
    public BookingNotFoundException()
    {
    }

    public BookingNotFoundException(string message)
        : base(message)
    {
    }

    public BookingNotFoundException(string message, System.Exception inner)
        : base(message, inner)
    {
    }
}