using Dentistry.Data.GeneratorDB.EF;
using Dentistry.ViewModels.Catalog.Tags;
using Dentisty.Data.Common;
using Dentisty.Data.GeneratorDB.Entities;
using Dentisty.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dentisty.Data.Repositories
{
    public class TagsRepository : Repository<Tags>, ITagsRepository
    {
        private readonly DentistryDbContext _dbContext;
        private readonly LoggerRepository _loggerRepository;


        public TagsRepository(DentistryDbContext context, LoggerRepository loggerRepository) : base(context)
        {
            _dbContext = context;
            _loggerRepository = loggerRepository;
        }

        public async Task<Tags> Create(TagsVm item)
        {
            if (item == null || string.IsNullOrEmpty(item.Name))
            {
                throw new ArgumentNullException(nameof(item));
            }
            try
            {
                var tag = await GetByName(item.Name.ToSlus_V2());
                if (tag != null)
                {
                    throw new Exception("Đã tồn tại tag name: " + item.Name);
                }
                tag = new Tags()
                {
                    Name = item.Name.ToSlus_V2(),
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };
                await AddAsync(tag);
                await SaveChangesAsync();
                return tag;
            }
            catch (Exception ex)
            {
                _loggerRepository.QueueLog(ex.Message);
                throw new Exception("Lỗi tạo mới Tags, làm ơn  thử lại");
            }
        }

        public async Task<List<Tags>> GetAll()
        {
            return await _dbContext.Tags.Include(t => t.Articles).ToListAsync();
        }

        public async Task<Tags> GetById(int id)
        {
            return await _dbContext.Tags.Include(x => x.Articles).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Tags> GetByName(string name)
        {
            return await _dbContext.Tags.Include(x => x.Articles).FirstOrDefaultAsync(x => x.Name == name);
        }

        public async Task<Tags> Update(TagsVm item)
        {
            try
            {

                var tag = await GetByIdAsync(item.Id);
                if (tag != null)
                {
                    if (await _dbContext.Tags.AnyAsync(x => x.Name == item.Name.ToSlus_V2() && item.Id != x.Id))
                    {
                        throw new Exception("Đã tồn tại " + item.Name);
                    }
                    tag.Name = item.Name.ToSlus_V2();
                    tag.UpdatedAt = DateTime.Now;
                    UpdateAsync(tag);
                    await SaveChangesAsync();
                    return tag;
                }
                throw new Exception("Không tìm thấy Tags, tag Name: " + item.Name);
            }
            catch (Exception ex) {
                _loggerRepository.QueueLog(ex.Message, "Update tags TagName: " + item.Name);
                throw new Exception("Update faild");
            }
        }
        public async Task<bool> Delete(int id)
        {

            var delete = await GetById(id);
            DeleteAsync(delete);
            await SaveChangesAsync();
            return true;
        }
    }
}
