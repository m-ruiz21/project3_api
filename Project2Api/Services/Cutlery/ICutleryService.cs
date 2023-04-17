using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Project2Api.Models;

namespace Project2Api.Services.CutleryItems;

public interface ICutleryService
{
    Task<ErrorOr<Cutlery>> CreateCutleryAsync(Cutlery cutlery);
    Task<ErrorOr<Cutlery>> GetCutleryAsync(string name);
    Task<ErrorOr<List<Cutlery>>> GetAllCutleryAsync();
    Task<ErrorOr<Cutlery>> UpdateCutleryAsync(Cutlery cutlery);
    Task<ErrorOr<IActionResult>> DeleteCutleryAsync(string name);
}