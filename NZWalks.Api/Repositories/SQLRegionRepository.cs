using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using NZWalks.Api.Data;
using NZWalks.Api.Models.Domain;
using NZWalks.Api.Models.DTO;

namespace NZWalks.Api.Repositories
{
    public class SQLRegionRepository : IRegionRepository
    {
        private readonly NZWalksDbContext _dbContext;

        public SQLRegionRepository(NZWalksDbContext dbContext)
        {
            _dbContext = dbContext;
        }           
       
        public async Task<List<Region>> GetAllAsync()
        {
            return await _dbContext.Regions.ToListAsync();
        }

        public async Task<Region?> GetByIdAsync(Guid id)
        {
            return await _dbContext.Regions.FirstOrDefaultAsync(n => n.Id == id);
        }
        
        public async Task<Region> CreateAsync(Region region)
        {
            await _dbContext.Regions.AddAsync(region);
            await _dbContext.SaveChangesAsync();

            return region;
        }

        public async Task<Region?> UpdateAsync(Guid id, Region region)
        {
            var existingRegionDomain = await _dbContext.Regions.FirstOrDefaultAsync(n => n.Id == id);
            if (existingRegionDomain == null)
                return null;
            
            existingRegionDomain.Code = region.Code;
            existingRegionDomain.Name = region.Name;
            existingRegionDomain.RegionImageUrl = region.RegionImageUrl;

            await _dbContext.SaveChangesAsync();
            return existingRegionDomain;
        }
        public async Task<Region?> DeleteAsync(Guid id)
        {
            var region = await _dbContext.Regions.FirstOrDefaultAsync(n => n.Id == id);
            if (region == null)
                return null ;

            _dbContext.Regions.Remove(region);
            await _dbContext.SaveChangesAsync();
            return region;
        }
    }
}
