namespace GalleriaService.Api.Abstracts.Interfaces;

public interface IService
{
    Task ExecuteAsync();
}
public interface IService<T>
{
    Task<T> ExecuteAsync();
}