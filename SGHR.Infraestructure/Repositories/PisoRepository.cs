using SGHR.Application.Dtos;          // <-- NO .Piso
using SGHR.Application.Interfaces;    // <-- aquí vive IPisoRepository
using SGHR.Infrastructure.Http;


namespace SGHR.Infrastructure.Repositories;
public class PisoRepository : IPisoRepository
{
    private readonly IApiClient _api;
    private const string BasePath = "api/pisos";

    public PisoRepository(IApiClient api) => _api = api;

    public async Task<IEnumerable<PisoDto>> GetAllAsync()
        => await _api.GetAsync<IEnumerable<PisoDto>>($"{BasePath}") ?? Enumerable.Empty<PisoDto>();

    public Task<PisoDto?> GetByIdAsync(int id)
        => _api.GetAsync<PisoDto>($"{BasePath}/{id}");

    public Task<PisoDto?> CreateAsync(SavePisoDto dto)
        => _api.PostAsync<SavePisoDto, PisoDto>($"{BasePath}", dto);

    public Task<bool> UpdateAsync(UpdatePisoDto dto)
        => _api.PutAsync($"{BasePath}/{dto.Id}", dto);

    public Task<bool> DeleteAsync(int id)
        => _api.DeleteAsync($"{BasePath}/{id}");
}
