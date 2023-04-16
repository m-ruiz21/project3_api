using Project2Api.Models;

namespace Project2Api.Repositories;

public interface ICutleryRepository
{
    /// <summary>
    /// Creates a new cutlery.
    /// </summary>
    /// <param name="cutlery">The cutlery to create.</param>
    /// <returns>The number of rows affected.</returns>
    Task<int?> CreateCutleryAsync(Cutlery cutlery);

    /// <summary>
    /// Gets all cutlery.
    /// </summary>
    /// <returns>A list of cutlery.</returns>
    Task<IEnumerable<Cutlery>?> GetCutleryAsync();

    /// <summary>
    /// Gets a cutlery by name.
    /// </summary>
    /// <param name="name">The name of the cutlery.</param>
    /// <returns>A cutlery.</returns>
    Task<Cutlery?> GetCutleryByNameAsync(string name);

    /// <summary>
    /// Updates a cutlery.
    /// </summary>
    /// <param name="cutlery">The cutlery to update.</param>
    /// <returns>The number of rows affected.</returns>
    Task<int?> UpdateCutleryAsync(Cutlery cutlery);

    /// <summary>
    /// Deletes a cutlery.
    /// </summary>
    /// <param name="name">The name of the cutlery to delete.</param>
    /// <returns>The number of rows affected.</returns>
    Task<bool> DeleteCutleryAsync(string name);
}