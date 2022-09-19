using HaberEcommerceSite.Data.Entities;
using HaberEcommerceSite.Data.Repositories;
using HaberEcommerceSite.ViewModels;

namespace HaberEcommerceSite.Interfaces
{
    public interface IUnitOfWork
    {
        BookRepository BookRepository { get; }

        BookPriceListViewModelRepository BookPriceListViewModelRepository { get; }

        BookAuthorRepository BookAuthorRepository { get; }

        PromoRepository PromoRepository { get; }

        BookPromoRepository BookPromoRepository { get; }

        ListDetailRepository ListDetailRepository { get; }

        IRepository<Author> AuthorRepository { get; }

        IRepository<Editorial> EditorialRepository { get; }

        IRepository<Bookbinding> BookbindingRepository { get; }

        IRepository<BookCollection> BookCollectionRepository { get; }

        IRepository<Category> CategoryRepository { get; }

        IRepository<Subcategory> SubcategoryRepository { get; }

        IRepository<PriceList> PriceListRepository { get; }

        void Save();
    }
}
