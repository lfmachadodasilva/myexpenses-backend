using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using lfmachadodasilva.MyExpenses.Api.Models;
using lfmachadodasilva.MyExpenses.Api.Models.Dtos;
using lfmachadodasilva.MyExpenses.Api.Repositories;

namespace lfmachadodasilva.MyExpenses.Api.Services
{
    public abstract class ServiceBase<TDto, TModel> : IService<TDto> where TDto : IDto where TModel : IModel
    {
        private readonly IRepository<TModel> _repository;

        private readonly IUnitOfWork _unitOfWork;

        private readonly IMapper _mapper;

        protected ServiceBase(IRepository<TModel> repository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public virtual IEnumerable<TDto> GetAll()
        {
            var models = _repository.GetAll();
            return _mapper.Map<IEnumerable<TDto>>(models);
        }

        public virtual async Task<TDto> GetByIdAsync(long id)
        {
            var model = await _repository.GetByIdAsync(id);
            return _mapper.Map<TDto>(model);
        }

        public virtual async Task<TDto> UpdateAsync(TDto dto)
        {
            _unitOfWork.BeginTransaction();

            var model = _mapper.Map<TModel>(dto);
            var result = await _repository.UpdateAsync(model);

            return (await _unitOfWork.CommitAsync()) > 0 ? _mapper.Map<TDto>(result) : default(TDto);
        }

        public virtual async Task<TDto> AddAsync(TDto dto)
        {
            _unitOfWork.BeginTransaction();

            var model = _mapper.Map<TModel>(dto);
            var result = await _repository.AddAsync(model);

            return (await _unitOfWork.CommitAsync()) > 0 ? _mapper.Map<TDto>(result) : default(TDto);
        }

        public virtual async Task<bool> RemoveAsync(long id)
        {
            _unitOfWork.BeginTransaction();

            var result = await _repository.RemoveAsync(id);

            if (!result)
            {
                return false;
            }

            return await _unitOfWork.CommitAsync() > 0;
        }
    }
}
