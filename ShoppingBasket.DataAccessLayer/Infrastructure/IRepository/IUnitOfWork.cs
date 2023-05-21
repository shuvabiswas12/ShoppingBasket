namespace ShoppingBasket.DataAccessLayer.Infrastructure.IRepository;

public interface IUnitOfWork
{
    ICategoryRepository CategoryRepository { get; }
    void Save();
}