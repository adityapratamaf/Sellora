using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Common.Querying;

/// <summary>
/// Merepresentasikan satu aturan sorting.
/// </summary>
public class SortModel
{
    /// <summary>
    /// Nama kolom/field yang akan diurutkan (harus sama dengan nama properti DTO).
    /// Contoh: "ba_long_name"
    /// </summary>
    public string? Field { get; set; }

    /// <summary>
    /// Arah sorting.
    /// Harus "asc" (ascending) atau "desc" (descending).
    /// </summary>
    public string? Direction { get; set; }
}