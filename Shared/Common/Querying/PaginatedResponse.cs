namespace Shared.Common.Querying;

using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

public class PaginatedResponse<T>
{
    // Konstruktor untuk membuat response
    public PaginatedResponse(List<T> items, int totalItems, int pageNumber, int pageSize)
    {
        Items = items;
        TotalItems = totalItems;
        PageNumber = pageNumber;
        PageSize = pageSize;

        // Kalkulasi TotalPages secara otomatis
        TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
    }

    // Data untuk halaman saat ini
    public List<T> Items { get; }

    // Halaman saat ini
    [JsonPropertyName("page_number")]
    public int PageNumber { get; }

    [JsonPropertyName("page_size")]
    public int PageSize { get; }

    [JsonPropertyName("total_items")]
    public int TotalItems { get; }

    [JsonPropertyName("total_pages")]
    public int TotalPages { get; }
}