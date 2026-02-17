
using ECCSA.BackOffice.Server.Model.Academy;
using ECCSA.BackOffice.Server.Persistence.DTO;
using ECCSA.BackOffice.Server.Persistence.DTO.Academy;
using ECCSA.BackOffice.Server.Persistence.Repositories.Academy;
using HotChocolate;
using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ECCSA.BackOffice.Server.API.GraphQL.Mutations
{
    public partial class Mutation
    {
        public async Task<CategoryPayload> CreateCategory(
                 CreateCategoryInput data
               , [Service] IHttpContextAccessor contextAccessor
               , [Service] CategoryRepository  categoryRepository
             // , [Service] ICategoryEventSender eventSender 
       )
        {
            var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var data2 = new GenericInput<CreateCategoryInput>(data, userId);
            var val = await categoryRepository.Create(data2);
            // await eventSender.SendAsync(data.TypeId, val);
            return val;
        }

        public async Task<CategoryPayload> UpdateCategory(
            UpdateCategoryInput data
            , [Service] IHttpContextAccessor contextAccessor
            , [Service] CategoryRepository  categoryRepository
         //, [Service] ICategoryEventSender eventSender
        )
        {
            var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var data2 = new GenericInput<UpdateCategoryInput>(data, userId);
            var val = await categoryRepository.Update(data2);
            // await eventSender.SendAsync(data.ExchangeRateTypeId, val);
            return val;

        }

        public async Task DeleteCategory(Category data
        , [Service] CategoryRepository  categoryRepository
            // , [Service] ICategoryEventSender eventSender
            )
        {
           await categoryRepository.Delete(data );
            //await eventSender.SendAsync(data.ExchangeRateTypeId, val);
            //return val;
        }
    }
    
}
