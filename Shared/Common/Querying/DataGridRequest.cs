using System.ComponentModel;
using System.Text.Json.Serialization;

namespace Shared.Common.Querying;

/// <summary>
/// DTO standar untuk semua request data grid.
/// Mendukung paginasi, multi-sort, dan filter per kolom.
/// </summary>
public class DataGridRequest
{
    // --- PAGINASI ---

    /// <summary>
    /// Halaman yang diminta.
    /// </summary>

    [JsonPropertyName("page_number")]
    [DefaultValue(1)]
    public int PageNumber { get; set; } = 1;

    /// <summary>
    /// Jumlah data per halaman.
    /// </summary>
    [JsonPropertyName("page_size")]
    [DefaultValue(10)]
    public int PageSize { get; set; } = 10;

    // --- SORTING ---

    /// <summary>
    /// Daftar kolom untuk diurutkan.
    /// (Kosongkan untuk sorting default)
    /// </summary>
    public List<SortModel> Sorting { get; set; } = new();

    // --- FILTERING ---

    /// <summary>
    /// Daftar filter per kolom.
    /// </summary>
    public List<FilterModel> Filters { get; set; } = new();
}