using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using lfmachadodasilva.MyExpenses.Api.Models;
using lfmachadodasilva.MyExpenses.Api.Models.Dtos;
using lfmachadodasilva.MyExpenses.Api.Repositories;

namespace lfmachadodasilva.MyExpenses.Api.Services
{
    public abstract class ServiceBase<TModel, TDto> : IService<TModel, TDto> where TModel : IModel where TDto : IDto
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

        /// <inheritdoc />
        public async Task<IEnumerable<TDto>> GetAllAsync()
        {
            var models = await _repository.GetAllAsyncEnumerable().ToList();
            return _mapper.Map<IEnumerable<TDto>>(models);
        }

        /// <inheritdoc />
        public virtual async Task<TDto> GetByIdAsync(long id)
        {
            var model = await _repository.GetByIdAsync(id);
            return _mapper.Map<TDto>(model);
        }

        /// <inheritdoc />
        public async Task<TDto> AddAsync(TDto dto)
        {
            _unitOfWork.BeginTransaction();
            var model = await _repository.AddAsync(_mapper.Map<TModel>(dto));
            if (model == null && await _unitOfWork.CommitAsync() < 1)
            {
                return default(TDto);
            }
            return _mapper.Map<TDto>(model);
        }

        /// <inheritdoc />
        public async Task<TDto> UpdateAsync(TDto dto)
        {
            _unitOfWork.BeginTransaction();
            var model = await _repository.UpdateAsync(_mapper.Map<TModel>(dto));
            if (model == null || await _unitOfWork.CommitAsync() < 1)
            {
                return default(TDto);
            }
            return _mapper.Map<TDto>(model);
        }

        /// <inheritdoc />
        public async Task<bool> RemoveAsync(long id)
        {
            _unitOfWork.BeginTransaction();
            var result = await _repository.RemoveAsync(id);
            return result && await _unitOfWork.CommitAsync() < 1;
        }
    }
}
