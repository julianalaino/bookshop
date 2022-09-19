using HaberEcommerceSite.Data.Entities;
using HaberEcommerceSite.Data.Repositories;
using HaberEcommerceSite.Interfaces;
using System;

namespace HaberEcommerceSite.Data
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly HaberContext databaseContext;

        private BookRepository bookRepository;

        private BookPriceListViewModelRepository bookPriceListViewModelRepository;

        private PromoRepository promoRepository;

        private IRepository<Author> authorRepository;

        private IRepository<Editorial> editorialRepository;

        private IRepository<Category> categoryRepository;

        private IRepository<Subcategory> subcategoryRepository;

        private IRepository<Bookbinding> bookbindingRepository;

        private IRepository<BookCollection> bookCollectionRepository;

        private IRepository<PriceList> priceListRepository;

        private BookAuthorRepository bookAuthorRepository;

        private BookPromoRepository bookPromoRepository;

        private ListDetailRepository listDetailRepository;


        private bool isDisposed = false;

        public BookRepository BookRepository
        {
            get
            {
                if (this.bookRepository == null)
                {
                    this.bookRepository = new BookRepository(databaseContext);
                }

                return this.bookRepository;
            }
        }

        public BookPriceListViewModelRepository BookPriceListViewModelRepository
        {
            get
            {
                if (this.bookPriceListViewModelRepository == null)
                {
                    this.bookPriceListViewModelRepository = new BookPriceListViewModelRepository(databaseContext);
                }

                return this.bookPriceListViewModelRepository;
            }
        }

        public PromoRepository PromoRepository
        {
            get
            {
                if (this.promoRepository == null)
                {
                    this.promoRepository = new PromoRepository(databaseContext);
                }

                return this.promoRepository;
            }
        }

        public ListDetailRepository ListDetailRepository
        {
            get
            {
                if (this.listDetailRepository == null)
                {
                    this.listDetailRepository = new ListDetailRepository(databaseContext);
                }

                return this.listDetailRepository;
            }
        }

        public IRepository<Author> AuthorRepository
        {
            get
            {
                if (this.authorRepository == null)
                {
                    this.authorRepository = new Repository<Author>(databaseContext);
                }

                return this.authorRepository;
            }
        }

        public IRepository<Editorial> EditorialRepository
        {
            get
            {
                if (this.editorialRepository == null)
                {
                    this.editorialRepository = new Repository<Editorial>(databaseContext);
                }

                return this.editorialRepository;
            }
        }

        public IRepository<Category> CategoryRepository
        {
            get
            {
                if (this.categoryRepository == null)
                {
                    this.categoryRepository = new Repository<Category>(databaseContext);
                }

                return this.categoryRepository;
            }
        }

        public IRepository<Subcategory> SubcategoryRepository
        {
            get
            {
                if (this.subcategoryRepository == null)
                {
                    this.subcategoryRepository = new Repository<Subcategory>(databaseContext);
                }

                return this.subcategoryRepository;
            }
        }

        public IRepository<Bookbinding> BookbindingRepository
        {
            get
            {
                if (this.bookbindingRepository == null)
                {
                    this.bookbindingRepository = new Repository<Bookbinding>(databaseContext);
                }

                return this.bookbindingRepository;
            }
        }

        public IRepository<BookCollection> BookCollectionRepository
        {
            get
            {
                if (this.bookCollectionRepository == null)
                {
                    this.bookCollectionRepository = new Repository<BookCollection>(databaseContext);
                }

                return this.bookCollectionRepository;
            }
        }

        public IRepository<PriceList> PriceListRepository
        {
            get
            {
                if (this.priceListRepository == null)
                {
                    this.priceListRepository = new Repository<PriceList>(databaseContext);
                }

                return this.priceListRepository;
            }
        }

        public BookAuthorRepository BookAuthorRepository
        {
            get
            {
                if (this.bookAuthorRepository == null)
                {
                    this.bookAuthorRepository = new BookAuthorRepository(databaseContext);
                }

                return this.bookAuthorRepository;
            }
        }

        public BookPromoRepository BookPromoRepository
        {
            get
            {
                if (this.bookPromoRepository == null)
                {
                    this.bookPromoRepository = new BookPromoRepository(databaseContext);
                }

                return this.bookPromoRepository;
            }
        }

        public UnitOfWork(HaberContext context)
        {
            this.databaseContext = context;
        }

        public void Save()
        {
            this.databaseContext.SaveChanges();
        }

        protected virtual void Dispose(bool isDisposing)
        {
            if(this.isDisposed is false)
            {
                if(isDisposing)
                {
                    this.databaseContext.Dispose();
                }
            }

            this.isDisposed = true;
        }

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }
    }
}
