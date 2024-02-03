namespace App.Domain.BuildingBlocks;

public interface IRepository
{
    Task SaveAsync();
}