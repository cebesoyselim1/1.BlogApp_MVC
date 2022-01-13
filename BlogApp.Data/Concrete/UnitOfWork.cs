using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogApp.Data.Abstract;
using BlogApp.Data.Concrete.EntityFramework.Contexts;
using BlogApp.Data.Concrete.EntityFramework.Repositories;

namespace BlogApp.Data.Concrete.EntityFramework
{
    public class UnitOfWork : IUnitOfWork
    {
        private BlogContext _context;
        EfArticleRepository _efArticleRepository;
        EfCategoryRepository _efCategoryRepository;
        EfCommentRepository _efCommentRepository;
        EfRoleRepository _efRoleRepoistory;
        EfUserRepository _efUserRepository;

        public UnitOfWork(BlogContext context)
        {
            _context = context;
        }


        public IArticleRepository Articles => _efArticleRepository ?? new EfArticleRepository(_context);

        public ICategoryRepository Categories => _efCategoryRepository ?? new EfCategoryRepository(_context);
        
        public ICommentRepository Comments => _efCommentRepository ?? new EfCommentRepository(_context);

        public IRoleRepository Roles => _efRoleRepoistory ?? new EfRoleRepository(_context);

        public IUserRepository Users => _efUserRepository ?? new EfUserRepository(_context);

        public async ValueTask DisposeAsync()
        {
            await _context.DisposeAsync();
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}