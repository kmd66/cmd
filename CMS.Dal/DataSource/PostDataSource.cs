using CMS.Model;
using CMS.Model.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace CMS.Dal.DataSource
{
    public class PostDataSource : BaseDataSource, IDataSource
    {
        public PostDataSource()
        {
            _pblContexts = new CntContexts();
        }
        readonly CntContexts _pblContexts;

        public async Task<Result<Post>> GetAsync(long id = 0, Guid? unicId = null)
        {
            try
            {
                var ett = await _pblContexts.Posts.Where(x =>
                   (id != 0 && x.Id == id)
                   || (id == 0 && x.UnicId == unicId)
                ).Take(1).FirstOrDefaultAsync();
                if (ett == null)
                    return Result<Post>.Successful();

                var returnMOdel = Map<Post, Dal.DbModel.Post>(ett);

                return Result<Post>.Successful(data: returnMOdel);
            }
            catch (Exception ex)
            {
                return Result<Post>.Failure(message: ex.Message);
            }
            finally
            {
                _pblContexts.ChangeTracker.Clear();
            }
        }

        public async Task<Result<Post>> GetAsync(string name)
        {
            try
            {
                if (string.IsNullOrEmpty(name))
                    return Result<Post>.Successful();
                var ett = await _pblContexts.Posts.Where(x =>
                    x.Title == name
                ).Take(1).FirstOrDefaultAsync();
                if (ett == null)
                    return Result<Post>.Successful();

                var returnMOdel = Map<Post, Dal.DbModel.Post>(ett);

                return Result<Post>.Successful(data: returnMOdel);
            }
            catch (Exception ex)
            {
                return Result<Post>.Failure(message: ex.Message);
            }
            finally
            {
                _pblContexts.ChangeTracker.Clear();
            }
        }

        public async Task<Result<Post>> GetByAliasAsync(string alias,bool? published = null,bool? isProduct = null)
        {
            try
            {
                if (string.IsNullOrEmpty(alias))
                    return Result<Post>.Successful();
                var ett = await _pblContexts.Posts.Where(x =>
                    x.Alias == alias
                    && (published == null || x.Published == published)
                    && (isProduct == null || x.IsProduct == isProduct)
                ).Take(1).FirstOrDefaultAsync();
                if (ett == null)
                    return Result<Post>.Successful();

                var returnMOdel = Map<Post, Dal.DbModel.Post>(ett);

                return Result<Post>.Successful(data: returnMOdel);
            }
            catch (Exception ex)
            {
                return Result<Post>.Failure(message: ex.Message);
            }
            finally
            {
                _pblContexts.ChangeTracker.Clear();
            }
        }

        public async Task<Result> AddAsync(Post model)
        {
            try
            {
                var ett = Map<Dal.DbModel.Post, Post>(model);
                _pblContexts.Add<Dal.DbModel.Post>(ett);
                await _pblContexts.SaveChangesAsync();

                return Result.Successful();
            }
            catch (Exception ex)
            {
                return Result.Failure(message: ex.Message);
            }
            finally
            {
                _pblContexts.ChangeTracker.Clear();
            }
        }

        public async Task<Result> EditAsync(Post model)
        {
            try
            {
                var record = await GetAsync(model.Id);
                if (!record.Success)
                    return Result.Failure(message: record.Message);
                if (record.Data == null)
                    return Result.Successful();

                var ett = Map<Dal.DbModel.Post, Post>(model);

                ett.Id = record.Data.Id;
                ett.UnicId = record.Data.UnicId;
                ett.Date = record.Data.Date;
                ett.Hit = record.Data.Hit;

                _pblContexts.Update<Dal.DbModel.Post>(ett);
                await _pblContexts.SaveChangesAsync();
                return Result.Successful();
            }
            catch (Exception ex)
            {
                return Result.Failure(message: ex.Message);
            }
            finally
            {
                _pblContexts.ChangeTracker.Clear();
            }
        }

        public async Task<Result<IEnumerable<Post>>> ListAsync(PostVM model)
        {
            try
            {
                var query = $"cnt.SpGetPosts"
                    + $" @Title = {model.Title.Query()}"
                    + $", @Alias = {model.Alias.Query()}"
                    + $", @Special = {model.Special.Query()}"
                    + $", @Published = {model.Published.Query()}"
                    + $", @IsProduct = {model.IsProduct.Query()}"
                    + $", @PageSize = {model.PageSize.Query()}"
                    + $", @PageIndex = {model.PageIndex.Query()}";
                var ett = await _pblContexts.PostDtos.FromSql(System.Runtime.CompilerServices.FormattableStringFactory.Create(query)).ToListAsync();

                var returnMOdel = MapList<Post, Dal.DbModel.PostDto>(ett);

                return Result<IEnumerable<Post>>.Successful(data: returnMOdel);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<Post>>.Failure(message: ex.Message);
            }
            finally
            {
                _pblContexts.ChangeTracker.Clear();
            }
        }

        public async Task<Result<IEnumerable<Post>>> ListAsync(List<string> alias)
        {
            try
            {
                alias = alias.Take(15).ToList();
                var ett = await _pblContexts.Posts.Where(x =>
                    x.Published == true &&
                    alias.Any(a => a == x.Alias)
                ).ToListAsync();

                if (ett == null)
                    return Result<IEnumerable<Post>>.Successful(data: new List<Post>());

                var returnMOdel = MapList<Post, Dal.DbModel.Post>(ett);


                return Result<IEnumerable<Post>>.Successful(data: returnMOdel);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<Post>>.Failure(message: ex.Message);
            }
            finally
            {
                _pblContexts.ChangeTracker.Clear();
            }
        }

        public async Task<Result<List<Post>>> ListAsync(PostVM model, int count)
        {
            try
            {
                var ett = await _pblContexts.Posts.Where(x =>
                    x.Published == true && (
                   (model.Special == null || x.Special == model.Special)
                   && (model.IsProduct == null || x.IsProduct == model.IsProduct)
                )).Take(count).OrderByDescending(x => x.Date).ToListAsync();
                if (ett == null)
                    return Result<List<Post>>.Successful(data: new List<Post>());

                var returnMOdel = MapList<Post, Dal.DbModel.Post>(ett);

                return Result<List<Post>>.Successful(data: returnMOdel.ToList());
            }
            catch (Exception ex)
            {
                return Result<List<Post>>.Failure(message: ex.Message);
            }
            finally
            {
                _pblContexts.ChangeTracker.Clear();
            }
        }

        public async Task<Result<List<Post>>> RelatedAsync(Post model, int count)
        {
            try
            {
                var query = $"cnt.SpGetRelatedPosts"
                    + $" @Id = {model.Id.Query()}"
                    + $", @MenuId = {model.MenuId.Query()}"
                    + $", @Count = {count.Query()}";
                var ett = await _pblContexts.PostDtos.FromSql(System.Runtime.CompilerServices.FormattableStringFactory.Create(query)).ToListAsync();

                var returnMOdel = MapList<Post, Dal.DbModel.PostDto>(ett);

                return Result<List<Post>>.Successful(data: returnMOdel.ToList());
            }
            catch (Exception ex)
            {
                return Result<List<Post>>.Failure(message: ex.Message);
            }
            finally
            {
                _pblContexts.ChangeTracker.Clear();
            }
        }

        public async Task<Result<List<Post>>> NextAndPrevAsync(Post model)
        {
            try
            {
                var list = new List<Post>();
                var ettNext = await _pblContexts.Posts.Where(x =>
                    x.Published == true
                    && x.MenuId == model.MenuId
                    && x.Date > model.Date
                ).OrderBy(x => x.Id).Take(1).FirstOrDefaultAsync();
                if (ettNext == null)
                    list.Add(new Post());
                else
                    list.Add(Map<Post, Dal.DbModel.Post>(ettNext));

                var ettPrev = await _pblContexts.Posts.Where(x =>
                    x.Published == true
                    && x.MenuId == model.MenuId
                    && x.Date < model.Date
                ).OrderByDescending(x => x.Id).Take(1).FirstOrDefaultAsync();
                if (ettPrev == null)
                    list.Add(new Post());
                else
                    list.Add(Map<Post, Dal.DbModel.Post>(ettPrev));

                return Result<List<Post>>.Successful(data: list);
            }
            catch (Exception ex)
            {
                return Result<List<Post>>.Failure(message: ex.Message);
            }
            finally
            {
                _pblContexts.ChangeTracker.Clear();
            }
        }

    }
}
