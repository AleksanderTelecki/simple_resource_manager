using Microsoft.EntityFrameworkCore;

namespace Resource.Domain.Repositories;

public interface IResourceRepository
{
    public Task<Entity.Resource> Get(Guid resourceId);
    public Task<List<Entity.Resource>> GetResources();
    public Task Create(Entity.Resource resource);
    public Task Save(Entity.Resource resource);
}

public class ResourceRepository: IResourceRepository
{
    private readonly ResourceDomainDataContext _resourceDomainDataContext;

    public ResourceRepository(ResourceDomainDataContext resourceDomainDataContext)
    {
        _resourceDomainDataContext = resourceDomainDataContext;
    }

    public async Task<Entity.Resource> Get(Guid resourceId)
    {
        return await _resourceDomainDataContext.Resources.FirstOrDefaultAsync(x => x.Id == resourceId);
    }

    public async Task<List<Entity.Resource>> GetResources()
    {
        return await _resourceDomainDataContext.Resources.AsNoTracking().ToListAsync();
    }

    public async Task Save(Entity.Resource resource)
    {
        _resourceDomainDataContext.Resources.Update(resource);
        await _resourceDomainDataContext.SaveChangesAsync();
    }

    public async Task Create(Entity.Resource resource)
    {
        _resourceDomainDataContext.Resources.Add(resource);
        await _resourceDomainDataContext.SaveChangesAsync();
    }
}