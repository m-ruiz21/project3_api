using System.Data;
using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Project2Api.Models;
using Project2Api.Repositories;
using Project2Api.ServiceErrors;

namespace Project2Api.Services.CutleryItems;

public class CutleryService : ICutleryService
{
    private readonly ICutleryRepository _cutleryRepository;

    public CutleryService(ICutleryRepository cutleryRepository)
    {
        _cutleryRepository = cutleryRepository;
    }

    public async Task<ErrorOr<Cutlery>> CreateCutleryAsync(Cutlery cutlery)
    {
        int? rowsAffected = await _cutleryRepository.CreateCutleryAsync(cutlery); 

        if (rowsAffected == null || rowsAffected == 0)
        {
            return Errors.Cutlery.DbError;
        }

        return cutlery;
    }

    public async Task<ErrorOr<List<Cutlery>>> GetAllCutleryAsync()
    {
        IEnumerable<Cutlery>? cutlery = await _cutleryRepository.GetAllCutleryAsync();

        if (cutlery == null)
        {
            return Errors.Cutlery.DbError;
        }

        return cutlery.ToList();
    }

    public async Task<ErrorOr<Cutlery>> GetCutleryAsync(string name)
    {
        if (String.IsNullOrEmpty(name))
        {
            return Errors.Cutlery.InvalidCutlery;
        }

        Cutlery? cutlery = await _cutleryRepository.GetCutleryByNameAsync(name);

        if (cutlery == null)
        {
            return Errors.Cutlery.NotFound;
        }

        return cutlery;
    }

    public async Task<ErrorOr<Cutlery>> UpdateCutleryAsync(Cutlery cutlery)
    {
        int? rowsAffected = await _cutleryRepository.UpdateCutleryAsync(cutlery);
        if (rowsAffected == null || rowsAffected == 0)
        {
            return Errors.Cutlery.NotFound;
        }

        return cutlery;
    }

    public async Task<ErrorOr<IActionResult>> DeleteCutleryAsync(string name)
    {
        bool success = await _cutleryRepository.DeleteCutleryAsync(name);
        if (!success)
        {
            return Errors.Cutlery.NotFound; 
        }

        return new NoContentResult();
    }
}