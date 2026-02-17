using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECCSA.BackOffice.Server.Persistence.DTO;
using ECCSA.BackOffice.Server.Model.Academy;
using ECCSA.BackOffice.Server.Persistence.Context;
using ECCSA.BackOffice.Server.Persistence.DTO.Academy;
using Microsoft.EntityFrameworkCore;

namespace ECCSA.BackOffice.Server.Persistence.Repositories.Academy
{
    public class CategoryRepository 
    {
        private readonly EccsaContext _context;
        private readonly EccsaUserContext _userContext;
        public CategoryRepository(EccsaContext context, EccsaUserContext userContext)
        {
            _context = context;
            _userContext = userContext;
        }

        public IQueryable<Category> Categories(Guid userId)
        {
            // var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
            return _context.Categories.Include(x => x.ChildrenCategories)
.Include(x => x.Topics)
.Include(x => x.NeedAssessmentDetails)
.Include(x => x.ParentCategory)
;
        }

        public async Task<Category> CategoryById(int id, Guid userId)
        {
            // var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
            return await _context.Categories.Include(x => x.ChildrenCategories)
.Include(x => x.Topics)
.Include(x => x.NeedAssessmentDetails)
.Include(x => x.ParentCategory)
.FirstOrDefaultAsync(i => i.Id == id);
        }
        public async Task<CategoryPayload> Create(GenericInput<CreateCategoryInput> input)
        {
            Category data = new()
            {
             Code = input.PayLoad.Code
			, NameEng = input.PayLoad.NameEng
			, NameLocal = string.IsNullOrEmpty( input.PayLoad.NameLocal)?string.Empty: input.PayLoad.NameLocal
            , ParentId = input.PayLoad.ParentId
			, Status = input.PayLoad.Status

            };
            var maxId = _context.Categories.Count() > 0 ? await _context.Categories.MaxAsync(x => x.Id) : 0;
            data.Id = maxId + 1;
            if (data.ParentId == 0)
                data.ParentId = data.Id;
            await _context.Categories.AddAsync(data);
           await _context.SaveChangesAsync();
           return new CategoryPayload(data);
        }

        public async Task<CategoryPayload> Update(GenericInput<UpdateCategoryInput> data)
        {
            var tmp = await  _context.Categories.FirstOrDefaultAsync(r => r.Id == data.PayLoad.Id);
            if (tmp == null) throw new KeyNotFoundException("Category not found");
            						tmp.Code = data.PayLoad.Code;
			tmp.NameEng = data.PayLoad.NameEng;
			tmp.NameLocal = string.IsNullOrEmpty( data.PayLoad.NameLocal)?string.Empty : data.PayLoad.NameLocal;
			tmp.ParentId = data.PayLoad.ParentId;
            if (data.PayLoad.ParentId == 0)
                tmp.ParentId = tmp.Id;
            tmp.Status = data.PayLoad.Status;

            _context.Categories.Update(tmp);
            await _context.SaveChangesAsync();
            return new CategoryPayload(tmp);

        }

        public async Task Delete(Category data)
        {
            var tmp = await _context.Categories.FirstOrDefaultAsync(r => r.Id == data.Id);
            if (tmp == null) throw new KeyNotFoundException("Category not found");
            _context.Categories.Remove(tmp);
            await _context.SaveChangesAsync();
        }
    }
}